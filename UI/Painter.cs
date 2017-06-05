using System;

namespace Csfeed.UI
{
    public class Painter
    {
		private static Paint2D.Fontify fontify = null;
		public byte ViewID = 2;
		public Paint2D.TVBVector4 TVB = null;

		public Painter()
		{
			if (fontify == null) {
				fontify = new Paint2D.Fontify();
			}
		}

		private void prepRect()
		{
			if ((TVB == null) || (TVB.vidx >= TVB.MaxVerts - 13)) {
				maybeSubmitRect();
				TVB = new Paint2D.TVBVector4(Paint2D.Sheds.Font.VertexLayout);
			}
		}

		private void maybeSubmitRect()
		{
			if (TVB != null) {
				Paint2D.ViewHelper.Submit(ViewID, Paint2D.Sheds.Font, TVB, fontify.Texture);
				TVB = null;
			}
		}




    }
}
