using System;

namespace PJPaint
{
	/// <summary>
	/// Summary description for WritingClass.
	/// </summary>
	public class WritingClass
	{
		public WritingClass()
		{
		}

		public WritingClass(int x, int y, string s, string f, float z, int r, int g, int b)
		{
			textX = x;
			textY = y;
			textValue = s;
			textFont = f;
			textSize = z;
			red = r;
			green = g;
			blue = b;
		}

		public string textValue = "";
		public string textFont = "";
		public float textSize = 8;
		public int textX = 0;
		public int textY = 0;
		public int red = 0;
		public int green = 0;
		public int blue = 0;
	}
}
