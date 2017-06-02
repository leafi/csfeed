using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

namespace SharpFont {
    public interface IGlyphAtlas {
        int Width { get; }
        int Height { get; }

        void Insert (int page, int x, int y, int width, int height, IntPtr data);
    }

    public unsafe sealed class TextAnalyzer {
        [ThreadStatic]
        static MemoryBuffer memoryBuffer;

        IGlyphAtlas atlas;
        public BinPacker packer;
        Dictionary<CacheKey, CachedFace> cache;
        ResizableArray<BufferEntry> buffer;
        int currentPage;
        
        int threeDotWidth = 10;

        public int Dpi {
            get;
            set;
        }

        public TextAnalyzer (IGlyphAtlas atlas) {
            this.atlas = atlas;
            Dpi = 72; // Dpi = 96;
            cache = new Dictionary<CacheKey, CachedFace>(CacheKey.Comparer);
            packer = new BinPacker(atlas.Width, atlas.Height);
            buffer = new ResizableArray<BufferEntry>(32);
        }
        
        public void HackSetThreeDotWidth(int w)
        {
            threeDotWidth = w;
        }

        public void Clear () => buffer.Clear();

        public void AppendText (string text, FontFace font, float sz) => AppendText(text, 0, text.Length, font, sz);

        public void AppendText (string text, int startIndex, int count, FontFace font, float sz) {
            fixed (char* ptr = text)
                AppendText(ptr + startIndex, count, font, sz);
        }

        public void AppendText (char[] text, int startIndex, int count, FontFace font, float sz) {
            fixed (char* ptr = text)
                AppendText(ptr + startIndex, count, font, sz);
        }

        public void AppendText (char* text, int count, FontFace font, float sz) {
            // look up the cache entry for the given font and size
            CachedFace cachedFace;
            //var font = format.Font;
            var size = FontFace.ComputePixelSize(sz, Dpi);
            var key = new CacheKey(font.Id, size);
            if (!cache.TryGetValue(key, out cachedFace))
                cache.Add(key, cachedFace = new CachedFace(font, size));

            // process each character in the string
            var nextBreak = BreakCategory.None;
            var previous = new CodePoint();
            char* end = text + count;
            while (text != end) {
                // handle surrogate pairs properly
                CodePoint codePoint;
                char c = *text++;
                if (char.IsSurrogate(c) && text != end)
                    codePoint = new CodePoint(c, *text++);
                else
                    codePoint = c;

                // ignore linefeeds directly after a carriage return
                if (c == '\n' && (char)previous == '\r')
                    continue;

                // get the glyph data
                CachedGlyph glyph;
                if (!cachedFace.Glyphs.TryGetValue(codePoint, out glyph) && !char.IsControl(c)) {
                    var data = font.GetGlyph(codePoint, size);
                    var width = data.RenderWidth;
                    var height = data.RenderHeight;
                    if (width > atlas.Width || height > atlas.Height)
                        throw new InvalidOperationException("Glyph is larger than the size of the provided atlas.");

                    var rect = new Rect();
                    if (width > 0 && height > 0) {
                        // render the glyph
                        var memSize = width * height;
                        var mem = memoryBuffer;
                        if (mem == null)
                            memoryBuffer = mem = new MemoryBuffer(memSize);

                        mem.Clear(memSize);
                        data.RenderTo(new Surface {
                            Bits = mem.Pointer,
                            Width = width,
                            Height = height,
                            Pitch = width
                        });

                        // save the rasterized glyph in the user's atlas
                        // leaf HACK: +1px padding on all sides
                        rect = packer.Insert(width + 4, height + 4);
                        rect.X += 2;
                        rect.Y += 2;
                        rect.Width -= 4;
                        rect.Height -= 4;
                        if (rect.Height == 0) {
                            // didn't fit in the atlas... start a new sheet
                            currentPage++;
                            packer.Clear(atlas.Width, atlas.Height);
                            rect = packer.Insert(width, height);
                            if (rect.Height == 0)
                                throw new InvalidOperationException("Failed to insert glyph into fresh page.");
                        }
                        atlas.Insert(currentPage, rect.X, rect.Y, rect.Width, rect.Height, mem.Pointer);
                    }

                    glyph = new CachedGlyph(rect, data.HorizontalMetrics.Bearing, data.HorizontalMetrics.Advance);
                    cachedFace.Glyphs.Add(codePoint, glyph);
                }

                // check for a kerning offset
                var kerning = font.GetKerning(previous, codePoint, size);
                previous = codePoint;

                // figure out whether this character can serve as a line break point
                // TODO: more robust character class handling
                var breakCategory = BreakCategory.None;
                if (char.IsWhiteSpace(c)) {
                    if (c == '\r' || c == '\n')
                        breakCategory = BreakCategory.Mandatory;
                    else
                        breakCategory = BreakCategory.Opportunity;
                }

                // the previous character might make us think that this one should be a break opportunity
                if (nextBreak > breakCategory)
                    breakCategory = nextBreak;
                if (c == '-')
                    nextBreak = BreakCategory.Opportunity;

                // alright, we have all the right glyph data cached and loaded
                // append relevant info to our buffer; we'll do the actual layout later
                if (c == 'i') {
                    buffer.Add(new BufferEntry {
                        GlyphData = new CachedGlyph(glyph.Bounds, new Vector2(glyph.Bearing.X, glyph.Bearing.Y + 0.5f), glyph.AdvanceWidth),
                        Kerning = kerning,
                        Break = breakCategory
                    });
                } else {
                    buffer.Add(new BufferEntry {
                        GlyphData = glyph,
                        Kerning = kerning,
                        Break = breakCategory
                    });
                }
            }
        }

        public float PerformLayout (bool ignorePixelGrid, float x, float y, float width, float height, TextLayout layout, bool widthConstrain = false, bool skip3IfMet = false) {
            layout.SetCount(buffer.Count);

            var pen = new Vector2(x, y);
            var maxh = 0f;
            bool brokeEarly = false;
            for (int i = 0; i < buffer.Count - (skip3IfMet ? 3 : 0); i++) {
                var entry = buffer[i];
                if (entry.Break == BreakCategory.Mandatory) {
                    pen.X = x;
                    pen.Y += 32; // TODO: line spacing
                }

                // data can be null for control characters,
                // or for glyphs without image data
                var data = entry.GlyphData;
                if (data == null)
                    continue;
                
                if (widthConstrain && pen.X + entry.Kerning + (ignorePixelGrid ? data.AdvanceWidth : (float)Math.Round(data.AdvanceWidth)) > width) {
                    brokeEarly = true;
                    break;
                }

                pen.X += entry.Kerning;
                layout.AddGlyph(
                    ignorePixelGrid ? pen.X + data.Bearing.X : (float)Math.Round(pen.X + data.Bearing.X),
                    (float)Math.Round(pen.Y - data.Bearing.Y),
                    data.Bounds.X,
                    data.Bounds.Y,
                    data.Bounds.Width,
                    data.Bounds.Height
                );
                maxh = Math.Max(maxh, data.Bounds.Height);

                pen.X += ignorePixelGrid ? data.AdvanceWidth : (float)Math.Round(data.AdvanceWidth);

                // leaf hack: get back to pixel grid after spaces
                if (ignorePixelGrid && entry.Break != BreakCategory.None) {
                    pen.X = (float)Math.Ceiling(pen.X);
                }
            }
            if (brokeEarly && widthConstrain && skip3IfMet) {
                for (int i = buffer.Count - 3; i < buffer.Count; i++) {
                    var entry = buffer[i];
                    var data = entry.GlyphData;
                    if (data == null) {
                        continue;
                    }
                    pen.X += entry.Kerning;
                    layout.AddGlyph(
                        ignorePixelGrid ? pen.X + data.Bearing.X : (float)Math.Round(pen.X + data.Bearing.X),
                        (float)Math.Round(pen.Y - data.Bearing.Y),
                        data.Bounds.X,
                        data.Bounds.Y,
                        data.Bounds.Width,
                        data.Bounds.Height
                    );
                    maxh = Math.Max(maxh, data.Bounds.Height);

                    pen.X += ignorePixelGrid ? data.AdvanceWidth : (float)Math.Round(data.AdvanceWidth);

                    // leaf hack: get back to pixel grid after spaces
                    if (ignorePixelGrid && entry.Break != BreakCategory.None) {
                        pen.X = (float)Math.Ceiling(pen.X);
                    }
                }
            }
            return pen.X - x;
        }

        struct BufferEntry {
            public CachedGlyph GlyphData;
            public float Kerning;
            public BreakCategory Break;
        }

        struct CachedFace {
            public FaceMetrics Metrics;
            public Dictionary<CodePoint, CachedGlyph> Glyphs;

            public CachedFace (FontFace font, float size) {
                Metrics = font.GetFaceMetrics(size);
                Glyphs = new Dictionary<CodePoint, CachedGlyph>();
            }
        }

        class CachedGlyph {
            public Rect Bounds;
            public Vector2 Bearing;
            public float AdvanceWidth;

            public CachedGlyph (Rect bounds, Vector2 bearing, float advance) {
                Bounds = bounds;
                Bearing = bearing;
                AdvanceWidth = advance;
            }
        }

        struct CacheKey {
            public int Id;
            public float Size;

            public CacheKey (int id, float size) {
                Id = id;
                Size = size;
            }

            public static readonly IEqualityComparer<CacheKey> Comparer = new CacheKeyComparer();

            class CacheKeyComparer : IEqualityComparer<CacheKey> {
                public bool Equals (CacheKey x, CacheKey y) => x.Id == y.Id && x.Size == y.Size;
                public int GetHashCode (CacheKey obj) => obj.Id.GetHashCode() ^ obj.Size.GetHashCode();
            }
        }

        class MemoryBuffer {
            public IntPtr Pointer;
            int size;

            public MemoryBuffer (int initialSize) {
                size = RoundSize(initialSize);
                Pointer = Marshal.AllocHGlobal(size);
            }

            public void Clear (int newSize) {
                newSize = RoundSize(newSize);
                if (newSize > size) {
                    Pointer = Marshal.ReAllocHGlobal(Pointer, (IntPtr)newSize);
                    size = newSize;
                }

                // clear the memory
                for (int* ptr = (int*)Pointer, end = ptr + (newSize >> 2); ptr != end; ptr++)
                    *ptr = 0;
            }

            static int RoundSize (int size) => (size + 3) & ~3;
        }

        enum BreakCategory {
            None,
            Opportunity,
            Mandatory
        }
    }
}
