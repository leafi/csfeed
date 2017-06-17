using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;

namespace Csfeed
{
    public static class StaticIni
    {
		private static object lockBlob = new object();

		private static Dictionary<string, Tuple<object, Action<object>>> interrogateSchema(object schema)
		{
			var di = new Dictionary<string, Tuple<object, Action<object>>>();
			foreach (var pi in schema.GetType().GetFields()) {
				di[pi.Name] = Tuple.Create<object, Action<object>>(pi.GetValue(schema), o => pi.SetValue(schema, o));
			}
			return di;
		}

		public static Dictionary<string, Tuple<Func<object>, Action<object>>> InterrogateSectionGettersSetters(object section)
		{
			var di = new Dictionary<string, Tuple<Func<object>, Action<object>>>();
			foreach (var pi in section.GetType().GetFields()) {
				di[pi.Name] = Tuple.Create<Func<object>, Action<object>>(() => pi.GetValue(section), o => pi.SetValue(section, o));
			}
			return di;
		}

		private static object tryParseIniValue(string valstring, string lineDbg)
		{
			object v = null;
			if (valstring.StartsWith("\"") && valstring.EndsWith("\"")) {
				v = valstring.Substring(1, valstring.Length - 2);
			} else {
				if (valstring.ToLowerInvariant().StartsWith("vec4(")) {
					var vec4bits = valstring.Substring(4).Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
					if (vec4bits.Length != 4) {
						throw new Exception($"Failed to parse vec4() in ini. Should be 4 floats inside, comma-separated - not {vec4bits.Length}.");
					}
					var x = float.Parse(vec4bits[0].Trim());
					var y = float.Parse(vec4bits[1].Trim());
					var z = float.Parse(vec4bits[2].Trim());
					var w = float.Parse(vec4bits[3].Trim());
					v = new Vector4(x, y, z, w);
				} else if (valstring.ToLowerInvariant().StartsWith("vec3(")) {
					var vec3bits = valstring.Substring(4).Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
					if (vec3bits.Length != 3) {
						throw new Exception($"Failed to parse vec3() in ini. Should be 3 floats inside, comma-separated - not {vec3bits.Length}.");
					}
					var x = float.Parse(vec3bits[0].Trim());
					var y = float.Parse(vec3bits[1].Trim());
					var z = float.Parse(vec3bits[2].Trim());
					v = new Vector3(x, y, z);
				} else if (valstring.ToLowerInvariant().StartsWith("vec2(")) {
					var vec2bits = valstring.Substring(4).Split(new char[] { '(', ',', ')' }, StringSplitOptions.RemoveEmptyEntries);
					if (vec2bits.Length != 2) {
						throw new Exception($"Failed to parse vec2() in ini. Should be 2 floats inside, comma-separated - not {vec2bits.Length}.");
					}
					var x = float.Parse(vec2bits[0].Trim());
					var y = float.Parse(vec2bits[1].Trim());
					v = new Vector2(x, y);
				} else if (valstring.ToLowerInvariant() == "true") {
					v = true;
				} else if (valstring.ToLowerInvariant() == "false") {
					v = false;
				} else {
					int i;
					if (int.TryParse(valstring, out i)) {
						v = i;
					} else {
						float f;
						if (float.TryParse(valstring, out f)) {
							v = f;
						} else {
							Console.WriteLine($"FAILED to parse {lineDbg} in .ini file - strings need double quotes, remember!");
							Console.WriteLine("Ignoring line and continuing.");
						}
					}
				}
			}
			return v;
		}

		private static void loadSectionOnto(IEnumerable<string> lines, object schemaSection)
		{
			var sch = interrogateSchema(schemaSection);

			var correctocase = new Dictionary<string, string>();
			foreach (var k in sch.Keys) {
				correctocase[k.ToLowerInvariant()] = k;
			}

			foreach (var line in lines) {
				var s = line.Trim();
				if (s.StartsWith(";")) {
					continue;
				}
				var bits = s.Split(new char[] { '=' }, 2);
				if (bits.Length != 2) {
					continue;
				}
				var k = bits[0].Trim();
				bits[1] = bits[1].Trim();

				// fix caps?
				if (correctocase.ContainsKey(k.ToLowerInvariant())) {
					k = correctocase[k.ToLowerInvariant()];
				} else {
					Console.WriteLine($"Ignoring ini line {line}; I don't understand key {k}.");
					continue;
				}

				// parse obj from lines
				object v = tryParseIniValue(bits[1], line);

				if (v != null) {
					if (sch[k] != null) {
						//Console.WriteLine($"{k}: {sch[k].Item1.GetType()} vs. {v.GetType()}");
						bool bad = false;
						if (sch[k].Item1.GetType() != v.GetType()) {
							if (sch[k].Item1.GetType() == typeof(float) && v is int) {
								// upcast int->float OK
								v = (float)((int)v);
								//Console.WriteLine($"v type now {v.GetType()}");
								if (sch[k].Item1.GetType() != v.GetType()) {
									bad = true;
								}
							} else {
								bad = true;
							}
						}
						if (bad) {
							throw new Exception($"Bad .ini value type for key '{k}'; expected value like {sch[k].Item1.GetType().ToString()} but got value {v} of type {v.GetType().ToString()}");
						}
					}
				}

				// set prop via reflection
				sch[k].Item2(v);
			}
		}

		public static void LoadOnto(string filename, bool okIfMissing, object[] schemaSections)
		{
			Dictionary<string, object> schemas = new Dictionary<string, object>();
			foreach (var sch in schemaSections) {
				schemas[sch.GetType().Name] = sch;
			}
			LoadOnto(filename, okIfMissing, schemas);
		}

		public static void LoadOnto(string filename, bool okIfMissing, IDictionary<string, object> schemas)
		{
			List<string> already = new List<string>();
			List<string> accum = new List<string>();

			Dictionary<string, string> correctocase = new Dictionary<string, string>();
			foreach (var schk in schemas.Keys) {
				correctocase[schk.ToLowerInvariant()] = schk;
			}

			string currSectName = "";

			foreach (var s in File.ReadAllLines(filename)) {
				var t = s.Trim();
				if (t.StartsWith("[") && t.EndsWith("]")) {
					var k = t.Substring(1, t.Length - 2);

					if (correctocase.ContainsKey(k.ToLowerInvariant())) {
						k = correctocase[k.ToLowerInvariant()];
					} else {
						Console.WriteLine($"Unrecognized section {t} in {filename}. Skipping.");
						if (currSectName != "") {
							loadSectionOnto(accum, schemas[currSectName]);
						}
						currSectName = "";
						accum.Clear();
						continue;
					}

					if (already.Contains(k)) {
						throw new Exception($"Duplicate key {t} in {filename}! Fix your .ini!");
					}
					already.Add(k);

					if (currSectName != "") {
						loadSectionOnto(accum, schemas[currSectName]);
					}
					currSectName = k;
					accum.Clear();
				} else {
					accum.Add(s);
				}
			}

			if (currSectName != "") {
				loadSectionOnto(accum, schemas[currSectName]);
			}
		}

		public static void SaveFrom(string filename, IDictionary<string, object> sections)
		{
			lock (lockBlob) {
				var sw = new StreamWriter(filename, false);
				sw.NewLine = "\r\n";

				foreach (var kvp in sections.OrderBy(akvp => akvp.Key)) {
					sw.WriteLine($"[{kvp.Key}]");
					var crt = interrogateSchema(kvp.Value);
					foreach (var kvpkvp in crt.OrderBy(akvpkvp => akvpkvp.Key)) {
						var k = kvpkvp.Key;
						var v = kvpkvp.Value.Item1;

						if (v is string) {
							var s = v as string;
							if (s.Contains("\r") || s.Contains("\n")) {
								throw new Exception($"When saving {filename}, found key {k} with string value containing newline '{s}'!");
							}
							sw.WriteLine($"{k}=\"{v}\"");
						} else if (v is bool) {
							string sbv = (bool)v == true ? "True" : "False";
							sw.WriteLine($"{k}={sbv}");
						} else if (v is int || v is float) {
							sw.WriteLine($"{k}={v}");
						} else if (v is Vector4) {
							var v4 = (Vector4)v;
							sw.WriteLine($"{k}=vec4({v4.X},{v4.Y},{v4.Z},{v4.W})");
						} else if (v is Vector3) {
							var v3 = (Vector3)v;
							sw.WriteLine($"{k}=vec3({v3.X},{v3.Y},{v3.Z})");
						} else if (v is Vector2) {
							var v2 = (Vector2)v;
							sw.WriteLine($"{k}=vec2({v2.X},{v2.Y})");
						} else {
							throw new Exception($"WTF... can't deal with {filename} value type {v.GetType().ToString()} (for key {k}). Allowed types: bool, int, float, string, vector2, vector3, vector4.");
						}
					}
					sw.WriteLine();
				}

				sw.Dispose();

				Console.WriteLine("Ssaavveedd..");
			}
		}

		public static void SaveFrom(string filename, object[] sections)
		{
			var d = new Dictionary<string, object>();
			foreach (var o in sections) {
				d.Add(o.GetType().Name, o);
			}
			SaveFrom(filename, d);
		}
    }
}
