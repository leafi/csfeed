using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using SharpFont;

namespace Csfeed.RetainedUI
{
	public class ButtonTheme
	{
		public Vector4 BackgroundTopColor = new Vector4(1f, 1f, 1f, 1f);
		public Vector4 BackgroundBottomColor = new Vector4(1f, 1f, 1f, 1f);
		public Vector4 DisabledBackgroundTopColor = new Vector4(0.5f, 0.5f, 0.5f, 1f);
		public Vector4 DisabledBackgroundBottomColor = new Vector4(0.5f, 0.5f, 0.5f, 1f);
		public Vector4 DisabledOutlineTopColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 DisabledOutlineBottomColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 DisabledTextColor = new Vector4(0.1f, 0.1f, 0.1f, 1f);
		public Vector4 OutlineTopColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 OutlineBottomColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 TextColor = new Vector4(0f, 0f, 0f, 1f);
	}

	public class CheckBoxTheme
	{
		public Vector4 CheckBackgroundColor = new Vector4(1f, 1f, 1f, 1f);
		public Vector4 CheckOutlineColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 CheckIconColor = new Vector4(0f, 0f, 0f, 1f);
		public int CheckHeight = 25;
		public int CheckWidth = 25;
		public int CheckTextPadding = 5;
		public Vector4 TextColor = new Vector4(0f, 0f, 0f, 1f);
		public int PaddingX = 1;
		public int PaddingY = 1;
	}

	public class GlobalTheme
	{
		public float FontMaterialDesignIcons = 14f;
		public float FontUIStandardSmall = 13f;
		public float FontUIStandardNormal = 15f;
		public float FontUIStandardBig = 17f;
		public Vector4 TextColor = new Vector4(1f, 1f, 1f, 1f);
	}

	public class HLayoutTheme
	{
		public int InternalPadding = 1;
	}

	public class MenuTheme
	{
		public Vector4 BarBackgroundColor = new Vector4(1f, 1f, 1f, 1f);
		public Vector4 BarBorderColor = new Vector4(0.5f, 0.5f, 0.5f, 1f);
		public Vector4 BarTextColor = new Vector4(0f, 0f, 0f, 1f);
		public int BarTextOffsetY = 5;
		public Vector4 BarHighlightedBackgroundColor1 = new Vector4(0f, 1f, 1f, 1f);
		public Vector4 BarHighlightedBackgroundColor2 = new Vector4(0f, 1f, 1f, 1f);
		public Vector4 BarHighlightedOutlineColor1 = new Vector4(0f, 0f, 1f, 1f);
		public Vector4 BarHighlightedOutlineColor2 = new Vector4(0f, 0f, 1f, 1f);
		public Vector4 BarHighlightedTextColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 BarOpenBackgroundColor1 = new Vector4(0.8f, 0.8f, 0.8f, 1f);
		public Vector4 BarOpenBackgroundColor2 = new Vector4(0.8f, 0.8f, 0.8f, 1f);
		public Vector4 BarOpenOutlineColor1 = new Vector4(0.5f, 0.5f, 0.5f, 1f);
		public Vector4 BarOpenOutlineColor2 = new Vector4(1f, 1f, 1f, 1f);
		public Vector4 BarOpenTextColor = new Vector4(0f, 0f, 0f, 1f);
		public int BarOpenYOff = 0;
		public int BarHeight = 20;
		public int BarLeftPadding = 20;
		public int BarHighlightedTopPadding = 1;
		public int BarHighlightedBottomPadding = 1;
		public int BarOpenTopPadding = 1;
		public int BarOpenBottomPadding = 1;
		public int BarInterPadding = 10;
		public int BarHitBoxPadding = 4;
		public Vector4 ItemBackgroundColor = new Vector4(1f, 1f, 1f, 1f);
		public Vector4 ItemHighlightedBackgroundColor = new Vector4(0f, 1f, 1f, 1f);
		public Vector4 ItemHighlightedOutlineColor = new Vector4(0f, 0f, 1f, 1f);
		public int ItemHeight = 20;
		public int ItemLeftPadding = 40;
		public int ItemRightPadding = 10;
		public Vector4 ItemHighlightedTextColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 ItemTextColor = new Vector4(0f, 0f, 0f, 1f);
		public int ItemTextX = 0;
		public int ItemTextY = 3;
		public Vector4 OpenBorderColor = new Vector4(0f, 0f, 0f, 1f);
	}

	public class RadioButtonTheme
	{
		public int RadioTextPadding = 5;
		public Vector4 TextColor = new Vector4(0f, 0f, 0f, 1f);
		public int PaddingX = 1;
		public int PaddingY = 1;
	}

	public class RadioButtonGroupTheme
	{
		public int MidPadding = 1;
	}

	public class ScrollBarTheme
	{
		public int Fatness = 20;
		public int MinButtonSize = 30;
		public Vector4 BackgroundColor = new Vector4(0.5f, 0.5f, 0.5f, 1f);
		public Vector4 ForegroundColor = new Vector4(1f, 1f, 1f, 1f);
		public int VScrollBarPadding = 4;
	}

	public class TextBoxTheme
	{
		public Vector4 BackgroundColor = new Vector4(1f, 1f, 1f, 1f);
		public Vector4 CursorColor = new Vector4(0f, 0f, 0f, 1f);
		public int TextPaddingX = 5;
		public int TextPaddingY = 4;
		public Vector4 OutlineColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 FocusedOutlineColor = new Vector4(0f, 1f, 1f, 1f);
		public Vector4 TextColor = new Vector4(0f, 0f, 0f, 1f);
	}

	public class UpDownTheme
	{
		public int ButtonWidth = 20;
		public int InterPaddingX = 3;
		public int MultiPadding = 5;
	}

	public class VLayoutTheme
	{
		public int InternalPadding = 1;
	}

	public class WindowTheme
	{
		public Vector4 CloseButtonBackgroundColor = new Vector4(0.75f, 0.75f, 0.75f, 1f);
		public int CloseButtonPadding = 1;
		public int CloseButtonHeight = 30;
		public int CloseButtonWidth = 30;
		public int BodyPaddingX = 2;
		public int BodyPaddingY = 2;
		public int ResizeRadius = 4;
		public int TitleX = 0;
		public int TitleY = 0;
		public int TitleHeight = 30;
		public Vector4 BackgroundColor = new Vector4(0.5f, 0.5f, 0.5f, 1f);
		public Vector4 TitleTopColor = new Vector4(0f, 1f, 1f, 1f);
		public Vector4 TitleBottomColor = new Vector4(0f, 0.5f, 0.5f, 1f);
		public Vector4 TitleTextColor = new Vector4(0f, 0f, 0f, 1f);
		public Vector4 OutlineColor = new Vector4(0f, 0f, 0f, 1f);
	}

	public static class Fonts
	{
		private static Dictionary<string, FontFace> fonts = new Dictionary<string, FontFace>();

		private static FontFace getOrLoad(string name)
		{
			if (!fonts.ContainsKey(name)) {
				var strm = File.OpenRead($"../../data/fonts/{name}.ttf");
				fonts[name] = new FontFace(strm);
				strm.Dispose();
			}
			return fonts[name];
		}

		public static ValueTuple<FontFace, float> UIStandardSmall {
			get {
				return ValueTuple.Create(getOrLoad("OpenSans-Regular"), Theme.Global.FontUIStandardSmall);
			}
		}
		public static ValueTuple<FontFace, float> UIStandard {
			get {
				return ValueTuple.Create(getOrLoad("OpenSans-Regular"), Theme.Global.FontUIStandardNormal);
			}
		}
		public static ValueTuple<FontFace, float> UIStandardBig {
			get {
				return ValueTuple.Create(getOrLoad("OpenSans-Regular"), Theme.Global.FontUIStandardBig);
			}
		}
		public static ValueTuple<FontFace, float> MaterialDesignIcons {
			get {
				return ValueTuple.Create(getOrLoad("materialdesignicons-webfont"), Theme.Global.FontMaterialDesignIcons);
			}
		}
	}

	public static class Theme
	{
		// SLOW - use only once, ideally!
		private static IDictionary<string, object> getAll()
		{
			var d = new Dictionary<string, object>();
			foreach (var fi in typeof(Theme).GetFields(BindingFlags.Public | BindingFlags.Static)) {
				d.Add(fi.Name, fi.GetValue(null));
			}
			return d;
		}

		public static ButtonTheme Button = new ButtonTheme();
		public static CheckBoxTheme CheckBox = new CheckBoxTheme();
		public static GlobalTheme Global = new GlobalTheme();
		public static HLayoutTheme HLayout = new HLayoutTheme();
		public static MenuTheme Menu = new MenuTheme();
		public static RadioButtonTheme RadioButton = new RadioButtonTheme();
		public static RadioButtonGroupTheme RadioButtonGroup = new RadioButtonGroupTheme();
		public static ScrollBarTheme ScrollBar = new ScrollBarTheme();
		public static TextBoxTheme TextBox = new TextBoxTheme();
		public static UpDownTheme UpDown = new UpDownTheme();
		public static VLayoutTheme VLayout = new VLayoutTheme();
		public static WindowTheme Window = new WindowTheme();

		public static void Load() { StaticIni.LoadOnto("../../data/theme.ini", true, getAll()); }
		public static void Save() { StaticIni.SaveFrom("../../data/theme.ini", getAll()); }

		public static Dictionary<string, Dictionary<string, Tuple<Func<object>, Action<object>>>> GetAllGettersSetters()
		{
			var d = new Dictionary<string, Dictionary<string, Tuple<Func<object>, Action<object>>>>();
			foreach (var kvp in getAll()) {
				d[kvp.Key] = StaticIni.InterrogateSectionGettersSetters(kvp.Value);
			}
			return d;
		}
	}
}