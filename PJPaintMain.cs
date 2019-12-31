using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Diagnostics;
//using System.Web.Mail;
using System.IO;
using System.Xml;

namespace PJPaint
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class PJPaint : System.Windows.Forms.Form
	{
		//**** RELEASED in October 2017 as Freeware
		//private bool sharewareVersion = true;
		private bool sharewareVersion = false;
		//****

		//[DllImport("gdi32.dll")]
		//public static extern int Polyline(IntPtr hdc, IntPtr points, int count);

		//[DllImport("gdi32.dll")]
		//public static extern int FloodFill(IntPtr hdc, int pointX, int pointY, int colour);

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuTopFile;
		private System.Windows.Forms.MenuItem menuTopPaint;
		private System.Windows.Forms.MenuItem menuTopHelp;
		private System.Windows.Forms.MenuItem menuNew;
		private System.Windows.Forms.MenuItem menuOpen;
		private System.Windows.Forms.MenuItem menuSaveAs;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuAbout;
		private System.Windows.Forms.MenuItem menuColour;
		private System.Windows.Forms.MenuItem menuText;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuPrint;
		private System.Windows.Forms.MenuItem menuSend;
        private IContainer components;
        private string colouringBookFolder = AppDomain.CurrentDomain.BaseDirectory + "Book";
		private string stampFolder = AppDomain.CurrentDomain.BaseDirectory + "Stamps";
		private System.Windows.Forms.MenuItem menuStamp;

		private string m_currentFile = "bears.jpg";
		private string currentJPG = "";
		private string currentXML = "";

		const int toolbarHeight = 100;

		enum toolbarSelect { New, Open, Save, Email, Print, Paint, Write, Stamp, Nothing }
		toolbarSelect currentToolHover = toolbarSelect.Nothing;
		toolbarSelect currentToolPicked = toolbarSelect.Nothing;
		toolbarSelect currentDrawMode = toolbarSelect.Paint;

		protected bool showOnce = true;
		protected Bitmap pjColouring;
		protected bool[,] PixelsChecked;
		protected int m_fillcolor = 0;
		protected Color m_selectedColor = Color.Red;
		protected byte[] m_Tolerance=new byte[]{32,32,32};

		protected Bitmap backBuffer = null;
		protected Graphics backG = null;

		protected Image toolNew = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Pictures.gif");

		protected Image toolPaint = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Colour.gif");
		protected Image toolWrite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Write.gif");
		protected Image toolStamp = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Stamp.gif");

		protected Image toolOpen = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\File.gif");
		protected Image toolSave = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Save.gif");
		protected Image toolPrint = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Print.gif");
		protected Image toolEmail = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Email.gif");

		protected Color [] m_listcolors = {	
											Color.Red, 
											Color.MistyRose,
											Color.Tomato,
											Color.IndianRed,
											Color.DarkRed, 
											Color.Firebrick,
											Color.Fuchsia,
											Color.LightPink,
											Color.Pink,
											Color.DeepPink,
											Color.HotPink,
											Color.LightSalmon,
											Color.OrangeRed,
											Color.Orange,
											Color.Goldenrod,
											Color.DarkOrange,
											Color.PeachPuff,
											Color.LemonChiffon,
											Color.Yellow,
											Color.LightYellow,
											Color.Wheat,
											Color.Honeydew,
											Color.Cornsilk,
											Color.YellowGreen,
											Color.GreenYellow,
											Color.Green,
											Color.DarkGreen,
											Color.LawnGreen,
											Color.LightGreen,
											Color.SpringGreen,
											Color.Aquamarine,
		 								    Color.MintCream,
											Color.AliceBlue,
											Color.Blue,
											Color.DeepSkyBlue,
											Color.SkyBlue,
											Color.Cyan,
											Color.CadetBlue,
											Color.SlateBlue,
											Color.Navy,
											Color.MidnightBlue,
											Color.Indigo,
											Color.Purple,
											Color.Violet,
											Color.Plum,
											Color.LavenderBlush,
											Color.Tan,
											Color.Brown,
											Color.BurlyWood,
											Color.SlateGray,
											Color.Gray,
											Color.LightGray,
											Color.FloralWhite,
										};

		bool gotText = false;
		bool gotStamp = false;
		string currentText = "";
		string currentTextFont = "Comic Sans MS";
		float currentTextSize = 20;
		Bitmap currentStamp = null;
		string currentStampFile = "";
		int moveX = 0;
		int moveY = 0;

		const int maxStamps = 50;
		const int maxText = 50;
		int topStamp = 0;
		int topText = 0;
		WritingClass [] writingList = new WritingClass[maxText];
		ImageClass [] stampList = new ImageClass[maxStamps];

		PageSettings storedPageSettings = null;

		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuClearColouring;
		private System.Windows.Forms.MenuItem menuClearWriting;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Windows.Forms.PrintDialog printDialog1;
		private System.Windows.Forms.MenuItem menuPageSetup;
		private System.Windows.Forms.MenuItem menuClearStamps;

		public PJPaint()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			m_currentFile = pickRandomPicture();
			this.Text = "PJPaint - " + m_currentFile;
			pjColouring = (Bitmap) Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Book\\" + m_currentFile);

			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.DoubleBuffer, true);

			this.Cursor = Cursors.Cross;
		}

		private string pickRandomPicture()
		{
			string[] filesList = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Book");
			int maxFile = filesList.Length-1;
			Random rand = new Random();
			int index = rand.Next(0, maxFile);		
			string fullPath = filesList[index];
			int i = fullPath.LastIndexOf("\\");
			string justFile = fullPath.Substring(i+1);

			return(justFile);
		}

		private void paintToolbar(Graphics g)
		{
			Brush b = new SolidBrush(Color.White);
			Rectangle rect = new Rectangle(0, 0, this.ClientRectangle.Width, toolbarHeight-10);
			g.FillRectangle(b, rect); 

			Pen p = new Pen(Color.Black);

			// toolbar menu commands
			g.DrawImage(toolNew, 5, 5, 48, 48);
			g.DrawImage(toolOpen, 65, 5, 48, 48);
			g.DrawImage(toolSave, 125, 5, 48, 48);
			//g.DrawImage(toolEmail, 185, 5, 48, 48);
			//g.DrawImage(toolPrint, 245, 5, 48, 48);
			g.DrawImage(toolPrint, 185, 5, 48, 48);

			// drawing modes
			g.DrawImage(toolPaint, this.ClientRectangle.Width-175, 5, 48, 48);
			if (currentDrawMode == toolbarSelect.Paint) g.DrawEllipse(p, this.ClientRectangle.Width-175-2, 3, 50, 50);

			g.DrawImage(toolWrite, this.ClientRectangle.Width-115, 5, 48, 48);
			if (currentDrawMode == toolbarSelect.Write) g.DrawEllipse(p, this.ClientRectangle.Width-115-2, 3, 50, 50);

			g.DrawImage(toolStamp, this.ClientRectangle.Width-55, 5, 48, 48);
			if (currentDrawMode == toolbarSelect.Stamp) g.DrawEllipse(p, this.ClientRectangle.Width-55-2, 3, 50, 50);
		}

		private void paintPalette(Graphics g)
		{
			int x=0;
			int numColours = m_listcolors.Length+1;
			int countColours = 0;
			int yColour = 60;

			double calcWidth = (double)this.ClientRectangle.Width / (double)numColours;
			int paletteWidth = Convert.ToInt32(calcWidth);

			foreach (Color c in m_listcolors)
			{
				Brush b = new SolidBrush(c);
				Rectangle rect = new Rectangle(x, yColour, paletteWidth, 20);
				//g.FillRectangle(b, rect); 
				g.FillEllipse(b, rect);

				if (c.ToArgb() == m_selectedColor.ToArgb())
				{
					Pen p = new Pen(Color.Black, 1);
					g.DrawEllipse(p, x, yColour, paletteWidth, 20);
				}

				x+=paletteWidth;

				countColours++;	
			}
		}

		#region Superceded

		protected void gdiFloodFill(int paintx, int painty)
		{	
			Graphics g = Graphics.FromImage(this.pjColouring);
			m_fillcolor = ColorTranslator.ToWin32(this.m_selectedColor);
			m_fillcolor = BGRA(GetB(m_fillcolor),GetG(m_fillcolor),GetR(m_fillcolor),GetA(m_fillcolor));

			IntPtr hDC = g.GetHdc();
			//FloodFill(hDC, paintx, painty, m_fillcolor);
			g.ReleaseHdc(hDC);

			this.Invalidate();
		}

		protected void slowFloodFill(int paintx, int painty)
		{	
		}

		private void paintPalette2(Graphics g)
		{
			int x=0;

			KnownColor t = new KnownColor();
			int numColours = System.Enum.GetNames(t.GetType()).Length;
			int countColours = 0;
			int yColour = 60;

			double calcWidth = ((double)this.ClientRectangle.Width / (double)numColours)*2;
			int paletteWidth = Convert.ToInt32(calcWidth);

			foreach (string s in System.Enum.GetNames(t.GetType()))
			{
				Brush b = new SolidBrush(Color.FromName(s));
				Rectangle rect = new Rectangle(x, yColour, paletteWidth, 10);
				g.FillEllipse(b, rect);
				//g.FillRectangle(b, rect); 
				x+=paletteWidth;

				countColours++;				
				if (countColours >= numColours / 2)
				{
					yColour = 70;
					if (countColours == numColours/2) x = 0;
				}
				else
				{
					yColour = 60;
				}
			}
		}
		#endregion

		private void paintPicture(Graphics g)
		{
			//g.DrawImage(pjColouring, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
			//g.DrawImage(pjColouring, 0, 0);

			g.DrawImage(pjColouring, 0, toolbarHeight-10, this.ClientRectangle.Width, this.ClientRectangle.Height-toolbarHeight);
		}

		private void paintStamps(Graphics g)
		{
			// already chosen stamps
			for (int i = 0; i < topStamp; i++)
			{
				int translatedx = stampList[i].imageX;
				int translatedy = stampList[i].imageY;
				makeRawAbsolute(ref translatedx, ref translatedy);

				backG.DrawImage(stampList[i].imageValue, translatedx, translatedy);
			}
			
			// selected stamp image
			if (this.gotStamp == true && this.currentStamp != null)
			{
				backG.DrawImage(this.currentStamp, moveX, moveY);
			}
		}

		private void paintWriting(Graphics g)
		{
			// already chosen text strings
			for (int i = 0; i < topText; i++)
			{
				int translatedx = writingList[i].textX;
				int translatedy = writingList[i].textY;
				makeRawAbsolute(ref translatedx, ref translatedy);

				Font fontText = new System.Drawing.Font(writingList[i].textFont, writingList[i].textSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				Brush brushText = new SolidBrush(Color.FromArgb(writingList[i].red, writingList[i].green, writingList[i].blue));
				backG.DrawString(writingList[i].textValue, fontText, brushText, translatedx, translatedy);
			}

			// selected text
			if (this.gotText == true && this.currentText != "")
			{
				Font fontText = new System.Drawing.Font(this.currentTextFont, this.currentTextSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				Brush brushText = new SolidBrush(m_selectedColor);
				backG.DrawString(this.currentText, fontText, brushText, moveX, moveY);
			}
		}

		private void paintStatus(Graphics g)
		{
			Brush b = new SolidBrush(Color.White);
			Rectangle rect = new Rectangle(0, this.ClientRectangle.Height-40, this.ClientRectangle.Width, 40);
			g.FillRectangle(b, rect); 

			Font fontText = new System.Drawing.Font("Comic Sans MS", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			Brush brushText = new SolidBrush(m_selectedColor);

			switch (currentDrawMode)
			{
				case (toolbarSelect.Paint):
					b = new SolidBrush(m_selectedColor);
					rect = new Rectangle(1, this.ClientRectangle.Height-10, 8, 8);
					g.FillEllipse(b, rect);	
					Pen p = new Pen(Color.Black);
					g.DrawEllipse(p, 1, this.ClientRectangle.Height-10, 8, 8);
					break;
				case (toolbarSelect.Write):
					g.DrawString(this.currentText, fontText, brushText, 1, this.ClientRectangle.Height-12);
					break;
				case (toolbarSelect.Stamp):
					g.DrawString("Stamp Image", fontText, brushText, 1, this.ClientRectangle.Height-12);
					break;
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			//g.Clear(Color.White);

			paintStatus(backG);

			paintPicture(backG);
			paintStamps(backG);
			paintWriting(backG);

			paintToolbar(backG);
			paintPalette(backG);

			g.DrawImage(backBuffer, 0, 0);

			base.OnPaint(e);

			if (this.sharewareVersion == true && showOnce == true)
			{
				showOnce = false;
				AboutPJPaint aboutForm = new AboutPJPaint();
				aboutForm.sharewareVersion = this.sharewareVersion;
				aboutForm.ShowDialog();
			}
		}

		protected override void OnResize(EventArgs e)
		{
			if (backBuffer != null)
			{
				if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
				{
					backBuffer.Dispose();
					backG.Dispose();
					backBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, Graphics.FromHwnd(this.Handle));
					backG = Graphics.FromImage(backBuffer);
				}
			}

			this.Invalidate();	
			base.OnResize(e);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		protected void floodFillPoint(int paintx, int painty)
		{
			if (paintx >= pjColouring.Width || painty >= pjColouring.Height)
			{
				Trace.WriteLine("Abort Fill: Trying to colour out of bitmap bounds!");
				return;
			}

			Color thisCol = pjColouring.GetPixel(paintx, painty);

			Trace.WriteLine(thisCol.ToString());

			if (thisCol.R < 30 && thisCol.G < 30 && thisCol.B < 30)
			{
				Trace.WriteLine("Don't paint black or near-black");
				return;
			}

			if (thisCol.ToArgb() == m_selectedColor.ToArgb())
			{
				Trace.WriteLine("Don't repaint same colour");
				return;
			}

			//pjColouring.SetPixel(paintx, painty, m_selectedColor);
			//this.Invalidate();
			//return;

			Bitmap bmp = pjColouring;
			m_fillcolor = ColorTranslator.ToWin32(this.m_selectedColor);
			m_fillcolor = BGRA(GetB(m_fillcolor),GetG(m_fillcolor),GetR(m_fillcolor),GetA(m_fillcolor));
		
			BitmapData bmpData = bmp.LockBits(new Rectangle(0,0,bmp.Width,bmp.Height),ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
			System.IntPtr Scan0 = bmpData.Scan0;		
			unsafe
			{
				try
				{
					byte * scan0=(byte *)(void *)Scan0;
					int loc = CoordsToIndex(paintx, painty, bmpData.Stride);
					int color= *((int*)(scan0+loc));
					PixelsChecked = new bool[bmpData.Width+1, bmpData.Height+1];		    
					LinearFloodFill4(scan0, paintx, painty, new Size(bmpData.Width,bmpData.Height), bmpData.Stride,(byte*)&color);
				}
				finally
				{
				}
			}	
			bmp.UnlockBits(bmpData);
		
			this.Invalidate();
		}

		#region FloodFillHelpers

		unsafe void LinearFloodFill4(byte* scan0, int x, int y, Size bmpsize, int stride, byte* startcolor)
		{
			int* p=(int*) (scan0+(CoordsToIndex(x,y, stride)));
			
			int LFillLoc=x; 
			int* ptr=p; 
			while(true)
			{
				ptr[0]=m_fillcolor;
				PixelsChecked[LFillLoc,y]=true;
				LFillLoc--;
				ptr-=1;

				if (LFillLoc<=0 || !CheckPixel((byte*)ptr,startcolor) ||  (PixelsChecked[LFillLoc,y]))
					break;
				
			}
			LFillLoc++;
			
			int RFillLoc=x;
			ptr=p;
			
			while(true)
			{
				ptr[0]=m_fillcolor;
				PixelsChecked[RFillLoc,y]=true;
				RFillLoc++;
				ptr+=1;
				if(RFillLoc>=bmpsize.Width || !CheckPixel((byte*)ptr,startcolor) ||  (PixelsChecked[RFillLoc,y]))
					break;			
			}
			RFillLoc--;
			
			ptr=(int*)(scan0+CoordsToIndex(LFillLoc,y,stride));
			for(int i=LFillLoc;i<=RFillLoc;i++)
			{
				if(y>0 && CheckPixel((byte*)(scan0+CoordsToIndex(i,y-1,stride)),startcolor) && (!(PixelsChecked[i,y-1])))
					LinearFloodFill4(scan0, i,y-1,bmpsize,stride,startcolor);

				if(y<(bmpsize.Height-1) && CheckPixel((byte*)(scan0+CoordsToIndex(i,y+1,stride)),startcolor) && (!(PixelsChecked[i,y+1])))
					LinearFloodFill4(scan0, i,y+1,bmpsize,stride,startcolor);

				ptr+=1;
			}			
		}

		int CoordsToIndex(int x, int y, int stride)
		{
			return (stride*y)+(x*4);
		}

		unsafe bool CheckPixel(byte* px, byte* startcolor)
		{
			bool ret=true;
			for(byte i=0;i<3;i++)
				ret&= (px[i]>= (startcolor[i]-m_Tolerance[i])) && px[i] <= (startcolor[i]+m_Tolerance[i]);		    
			return ret;
		}

		public static byte GetR(int ARGB)
		{
			return LoByte((byte)LoWord(ARGB));
			
		}
		
		public static byte GetG(int ARGB)
		{
			return HiByte((short)LoWord(ARGB));
			
		}
		
		public static byte GetB(int ARGB)
		{
			return LoByte((byte)HiWord(ARGB));
			
		}
		
		public static byte GetA(int ARGB)
		{
			return HiByte((byte)HiWord(ARGB));			
		}

		public static int RGBA(byte R, byte G, byte B, byte A)
		{
			return (int)(R+(G<<8)+(B<<16)+(A<<24));
		}

		public static int RGB(byte R, byte G, byte B)
		{
			return (int)(R+(G<<8)+(B<<16));
		}
		
		public static int BGRA(byte B, byte G, byte R, byte A)
		{
			return (int)(B+(G<<8)+(R<<16)+(A<<24));
		}
		
		public static short LoWord(int n)
		{
			return (short)(n & 0xffff);
		}

		public static short HiWord(int n)
		{
			return (short)((n >> 16) & 0xffff);
		}
		
		public static byte LoByte(short n)
		{
			return (byte)(n & 0xff);
		}

		public static byte HiByte(short n)
		{
			return (byte)((n >> 8) & 0xff);
		}

		#endregion


		protected void makeRelative(ref int picx, ref int picy)
		{
			double relativex = (double) picx / (double) this.ClientRectangle.Width;
			double relativey = (double) (picy-90) / (double) (this.ClientRectangle.Height - toolbarHeight);

			relativex = (double) this.pjColouring.Width * relativex;
			relativey = (double) this.pjColouring.Height * relativey;

			picx = Convert.ToInt32(relativex);
			picy = Convert.ToInt32(relativey);
		}

		protected void makeRawRelative(ref int picx, ref int picy)
		{
			double relativex = (double) picx / (double) this.ClientRectangle.Width;
			double relativey = (double) picy / (double) this.ClientRectangle.Height;

			relativex = (double) this.pjColouring.Width * relativex;
			relativey = (double) this.pjColouring.Height * relativey;

			picx = Convert.ToInt32(relativex);
			picy = Convert.ToInt32(relativey);
		}

		protected void makeRawAbsolute(ref int picx, ref int picy)
		{
			double relativex = (double) picx / (double) this.pjColouring.Width;
			double relativey = (double) picy / (double) this.pjColouring.Height;

			relativex = (double) this.ClientRectangle.Width * relativex;
			relativey = (double) this.ClientRectangle.Height * relativey;

			picx = Convert.ToInt32(relativex);
			picy = Convert.ToInt32(relativey);
		}

		protected override void OnMouseMove(MouseEventArgs e) 
		{
			if (e.Y < 60)
			{
				if (this.Cursor != Cursors.Hand)
					this.Cursor = Cursors.Hand;
			}
			else
			{
				moveX = e.X;
				moveY = e.Y;

				switch (currentDrawMode)
				{
					case (toolbarSelect.Paint):
						if (this.Cursor != Cursors.Cross)
							this.Cursor = Cursors.Cross;
						break;
					case (toolbarSelect.Write):
						if (this.Cursor != Cursors.IBeam)
							this.Cursor = Cursors.IBeam;

						//Trace.WriteLine("OnMove: " + this.currentText);

						if (this.currentText != "")
						{
							this.Invalidate();
						}

						break;
					case (toolbarSelect.Stamp):
						if (this.Cursor != Cursors.UpArrow)
							this.Cursor = Cursors.UpArrow;

						if (this.currentStamp != null)
						{
							this.Invalidate();
						}
						break;
				}
			}
		}

		private void doWriting()
		{
			if (this.gotText == false)
			{
				setModeWriting();
			}
			else
			{
				if (topText < maxText)
				{
					int translatedx = moveX;
					int translatedy = moveY;
					makeRawRelative(ref translatedx, ref translatedy);

					writingList[topText] = new WritingClass(translatedx, translatedy, currentText, currentTextFont, currentTextSize, m_selectedColor.R, m_selectedColor.G, m_selectedColor.B);
					topText++;
				}
				else
				{
					MessageBox.Show("You are at the maximum allowed writing, you can clear all of the writing from the Paint menu.", "Maximum Writing", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					this.currentText = "";
					this.gotText = false;
					this.currentDrawMode = toolbarSelect.Paint;
					this.Invalidate();
				}
			}
		}

		private void doStamps()
		{
			if (this.gotStamp == false)
			{
				setModeStamping();
			}
			else
			{
				if (topStamp < maxStamps)
				{
					int translatedx = moveX;
					int translatedy = moveY;
					makeRawRelative(ref translatedx, ref translatedy);

					stampList[topStamp] = new ImageClass();
					stampList[topStamp].imageValue = (Image) this.currentStamp.Clone();
					stampList[topStamp].imageFilename = this.currentStampFile;
					stampList[topStamp].imageX = translatedx;
					stampList[topStamp].imageY = translatedy;

					topStamp++;
				}
				else
				{
					MessageBox.Show("You are at the maximum allowed stamps, you can clear all of the stamps from the Paint menu.", "Maximum Stamps", MessageBoxButtons.OK, MessageBoxIcon.Stop);
					this.currentStamp = null;
					this.currentStampFile = "";
					this.gotStamp = false;
					this.currentDrawMode = toolbarSelect.Paint;
					this.Invalidate();
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e) 
		{
			if (e.Y < toolbarHeight-10)
			{
				if (e.Y > 60)
				{
					Color thisCol = backBuffer.GetPixel(e.X, e.Y);

					Trace.WriteLine("thisCol: " + thisCol.Name);

					if (thisCol.ToArgb() == Color.White.ToArgb() || thisCol.ToArgb() == Color.Black.ToArgb())
					{
						Trace.WriteLine("Don't select black or white colour");
						return;
					}

					if (thisCol.ToArgb() == m_selectedColor.ToArgb())
					{
						Trace.WriteLine("Don't select same colour");
						return;
					}

					m_selectedColor = Color.FromArgb(thisCol.ToArgb());

					this.Invalidate();
				}
				else
				{
					testToolbar(e.X, e.Y);

					if (currentToolHover != toolbarSelect.Nothing)
					{
						currentToolPicked = currentToolHover;
						clickToolbar(currentToolPicked);

						this.Invalidate();
					}
				}
			}
			else
			{
				/*
				double relativex = (double) e.X / (double) this.ClientRectangle.Width;
				double relativey = (double) (e.Y-90) / (double) (this.ClientRectangle.Height - toolbarHeight);

				Trace.WriteLine(relativex.ToString());
				Trace.WriteLine(relativey.ToString());

				relativex = (double) this.pjColouring.Width * relativex;
				relativey = (double) this.pjColouring.Height * relativey;

				Trace.WriteLine(relativex.ToString());
				Trace.WriteLine(relativey.ToString());

				int translatedx = Convert.ToInt32(relativex);
				int translatedy = Convert.ToInt32(relativey);
				*/

				int translatedx = e.X;
				int translatedy = e.Y;
				makeRelative(ref translatedx, ref translatedy);

				switch (currentDrawMode)
				{
					case (toolbarSelect.Paint):
						floodFillPoint(translatedx, translatedy);
						break;
					case (toolbarSelect.Write):
						doWriting();
						break;
					case (toolbarSelect.Stamp):
						doStamps();
						break;
				}
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PJPaint));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuTopFile = new System.Windows.Forms.MenuItem();
            this.menuNew = new System.Windows.Forms.MenuItem();
            this.menuOpen = new System.Windows.Forms.MenuItem();
            this.menuSaveAs = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuSend = new System.Windows.Forms.MenuItem();
            this.menuPageSetup = new System.Windows.Forms.MenuItem();
            this.menuPrint = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuTopPaint = new System.Windows.Forms.MenuItem();
            this.menuColour = new System.Windows.Forms.MenuItem();
            this.menuStamp = new System.Windows.Forms.MenuItem();
            this.menuText = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuClearColouring = new System.Windows.Forms.MenuItem();
            this.menuClearWriting = new System.Windows.Forms.MenuItem();
            this.menuClearStamps = new System.Windows.Forms.MenuItem();
            this.menuTopHelp = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuTopFile,
            this.menuTopPaint,
            this.menuTopHelp});
            // 
            // menuTopFile
            // 
            this.menuTopFile.Index = 0;
            this.menuTopFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuSaveAs,
            this.menuItem2,
            this.menuSend,
            this.menuPageSetup,
            this.menuPrint,
            this.menuItem7,
            this.menuExit});
            this.menuTopFile.Text = "Pictures";
            // 
            // menuNew
            // 
            this.menuNew.Index = 0;
            this.menuNew.Text = "New...";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Index = 1;
            this.menuOpen.Text = "Open...";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // menuSaveAs
            // 
            this.menuSaveAs.Index = 2;
            this.menuSaveAs.Text = "Save As...";
            this.menuSaveAs.Click += new System.EventHandler(this.menuSaveAs_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 3;
            this.menuItem2.Text = "-";
            // 
            // menuSend
            // 
            this.menuSend.Index = 4;
            this.menuSend.Text = "Send to a Friend...";
            this.menuSend.Visible = false;
            this.menuSend.Click += new System.EventHandler(this.menuSend_Click);
            // 
            // menuPageSetup
            // 
            this.menuPageSetup.Index = 5;
            this.menuPageSetup.Text = "Page Setup...";
            this.menuPageSetup.Click += new System.EventHandler(this.menuPageSetup_Click);
            // 
            // menuPrint
            // 
            this.menuPrint.Index = 6;
            this.menuPrint.Text = "Print Picture...";
            this.menuPrint.Click += new System.EventHandler(this.menuPrint_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 7;
            this.menuItem7.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Index = 8;
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuTopPaint
            // 
            this.menuTopPaint.Index = 1;
            this.menuTopPaint.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuColour,
            this.menuStamp,
            this.menuText,
            this.menuItem1,
            this.menuItem3});
            this.menuTopPaint.Text = "Paint";
            // 
            // menuColour
            // 
            this.menuColour.Checked = true;
            this.menuColour.Index = 0;
            this.menuColour.Text = "Colour...";
            this.menuColour.Click += new System.EventHandler(this.menuColour_Click);
            // 
            // menuStamp
            // 
            this.menuStamp.Checked = true;
            this.menuStamp.Index = 1;
            this.menuStamp.Text = "Stamp...";
            this.menuStamp.Click += new System.EventHandler(this.menuStamp_Click);
            // 
            // menuText
            // 
            this.menuText.Checked = true;
            this.menuText.Index = 2;
            this.menuText.Text = "Write...";
            this.menuText.Click += new System.EventHandler(this.menuText_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 4;
            this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuClearColouring,
            this.menuClearWriting,
            this.menuClearStamps});
            this.menuItem3.Text = "Clear";
            // 
            // menuClearColouring
            // 
            this.menuClearColouring.Index = 0;
            this.menuClearColouring.Text = "Colouring";
            this.menuClearColouring.Click += new System.EventHandler(this.menuClearColouring_Click);
            // 
            // menuClearWriting
            // 
            this.menuClearWriting.Index = 1;
            this.menuClearWriting.Text = "Writing";
            this.menuClearWriting.Click += new System.EventHandler(this.menuClearWriting_Click);
            // 
            // menuClearStamps
            // 
            this.menuClearStamps.Index = 2;
            this.menuClearStamps.Text = "Stamps";
            this.menuClearStamps.Click += new System.EventHandler(this.menuClearStamps_Click);
            // 
            // menuTopHelp
            // 
            this.menuTopHelp.Index = 2;
            this.menuTopHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAbout});
            this.menuTopHelp.Text = "Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 0;
            this.menuAbout.Text = "About PJPaint...";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // PJPaint
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(592, 453);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "PJPaint";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "PJ Paint";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.exitEvent);
            this.Load += new System.EventHandler(this.PJPaint_Load);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new PJPaint());
		}

		private void menuAbout_Click(object sender, System.EventArgs e)
		{
			AboutPJPaint aboutPJ = new AboutPJPaint();
			aboutPJ.sharewareVersion = this.sharewareVersion;
			aboutPJ.ShowDialog();
		}

		private void PJPaint_Load(object sender, System.EventArgs e)
		{
			backBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, Graphics.FromHwnd(this.Handle));
			backG = Graphics.FromImage(backBuffer);

			setModeColouring();
		}

		private void newPicture()
		{
			ChoosePicture newDrawing = new ChoosePicture();
			newDrawing.findPicture(m_currentFile);
			newDrawing.ShowDialog();

			if (newDrawing.selectedPicture == true)
			{
				this.m_currentFile = newDrawing.currentFilename;			
				this.Text = "PJPaint - " + m_currentFile;

				pjColouring = (Bitmap) Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Book\\" + m_currentFile);
				topStamp = 0;
				topText = 0;
				this.Invalidate();
			}
		}

		private void openPicture()
		{
			displayShareware();

			if (this.sharewareVersion == false)
			{
				OpenPicture openFile = new OpenPicture();

				DialogResult ok = openFile.ShowDialog();

				if (ok == DialogResult.OK)
				{	
					string openImage = openFile.currentPath + "\\" + openFile.currentFilename;
					string openXMLFile = openFile.currentPath + "\\" + Path.GetFileNameWithoutExtension(openImage) + ".xml";

					try
					{
						this.m_currentFile = openImage;			
						this.Text = "PJPaint - " + m_currentFile;

						//FileStream fs =  new FileStream(openImage, FileMode.Open, FileAccess.Read);
						//pjColouring = new Bitmap(fs);
						//fs.Close();

						pjColouring = (Bitmap) Bitmap.FromFile(openImage);

						topStamp = 0;
						topText = 0;

						openXML(openXMLFile);
					}
					catch (Exception e)
					{
						MessageBox.Show("Picture did not open: " + e.Message, "Picture not opened", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

					this.Invalidate();
				}
			}
		}

		private void openXML(string xmlFile)
		{
			topText = 0;
			topStamp = 0;

			FileStream fRead = new FileStream(xmlFile, FileMode.Open);
			XmlTextReader xmlReader = new XmlTextReader(fRead);

			while (xmlReader.Read())
			{
				if (xmlReader.NodeType == XmlNodeType.Element)
				{
					if (xmlReader.Name == "Stamp")
					{
						stampList[topStamp] = new ImageClass();
						this.currentStamp = (Bitmap) Image.FromFile(stampFolder + "\\" + xmlReader.GetAttribute("Filename"));
						this.currentStamp.MakeTransparent(Color.White);
						stampList[topStamp].imageValue = (Image) currentStamp.Clone();
						stampList[topStamp].imageFilename = xmlReader.GetAttribute("Filename");
						stampList[topStamp].imageX = Convert.ToInt32(xmlReader.GetAttribute("X"));
						stampList[topStamp].imageY = Convert.ToInt32(xmlReader.GetAttribute("Y"));

						topStamp++;
					}

					if (xmlReader.Name == "Writing")
					{
						writingList[topText] = new WritingClass();
						writingList[topText].textValue = xmlReader.GetAttribute("Value");
						writingList[topText].textFont = xmlReader.GetAttribute("Font");
						writingList[topText].textX = Convert.ToInt32(xmlReader.GetAttribute("X"));
						writingList[topText].textY = Convert.ToInt32(xmlReader.GetAttribute("Y"));
						writingList[topText].textSize = Convert.ToSingle(xmlReader.GetAttribute("Size"));
						writingList[topText].red = Convert.ToInt32(xmlReader.GetAttribute("Red"));
						writingList[topText].green = Convert.ToInt32(xmlReader.GetAttribute("Green"));
						writingList[topText].blue = Convert.ToInt32(xmlReader.GetAttribute("Blue"));
						
						topText++;
					}
				}
			}

			xmlReader.Close();
			fRead.Close();
		}

		private void savePicture()
		{
			displayShareware();

			if (this.sharewareVersion == false)
			{
				SavePicture saveFile = new SavePicture();
				DialogResult ok = saveFile.ShowDialog();

				if (ok == DialogResult.OK)
				{
					string dir = Path.GetDirectoryName(saveFile.currentPicture);
					string file = Path.GetFileNameWithoutExtension(saveFile.currentPicture);

					currentJPG = dir + "\\" + file + ".jpg";
					currentXML = dir + "\\" + file + ".xml";

					try
					{
						// delete if existing
						if (File.Exists(currentJPG))
							File.Delete(currentJPG);
						if (File.Exists(currentXML))
							File.Delete(currentXML);
						
						// save JPG and XML
						pjColouring.Save(currentJPG, ImageFormat.Jpeg);
						saveXML(currentXML);
					}
					catch (Exception e)
					{
						MessageBox.Show("Picture did not save: " + e.Message, "Picture not saved", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}

		private void saveXML(string xmlFile)
		{
			FileStream fWrite = new FileStream(xmlFile, FileMode.Create);
			XmlTextWriter xmlSave = new XmlTextWriter(fWrite, System.Text.Encoding.ASCII);

			xmlSave.WriteStartDocument();
			xmlSave.WriteStartElement("PJPaint");

			xmlSave.WriteStartElement("ImageDetails");
			xmlSave.WriteAttributeString("ImageFile", this.currentJPG);

			for (int i = 0; i < topStamp; i++)
			{
				xmlSave.WriteStartElement("Stamp");
				xmlSave.WriteAttributeString("X", stampList[i].imageX.ToString());
				xmlSave.WriteAttributeString("Y", stampList[i].imageY.ToString());
				xmlSave.WriteAttributeString("Filename", stampList[i].imageFilename);
				xmlSave.WriteEndElement();
			}

			for (int i = 0; i < topText; i++)
			{
				xmlSave.WriteStartElement("Writing");
				xmlSave.WriteAttributeString("X", writingList[i].textX.ToString());
				xmlSave.WriteAttributeString("Y", writingList[i].textY.ToString());
				xmlSave.WriteAttributeString("Font", writingList[i].textFont);
				xmlSave.WriteAttributeString("Size", writingList[i].textSize.ToString());
				xmlSave.WriteAttributeString("Red", writingList[i].red.ToString());
				xmlSave.WriteAttributeString("Green", writingList[i].green.ToString());
				xmlSave.WriteAttributeString("Blue", writingList[i].blue.ToString());
				xmlSave.WriteAttributeString("Value", writingList[i].textValue);		
				xmlSave.WriteEndElement();
			}	
			
			xmlSave.WriteEndElement();
			xmlSave.Close();
			fWrite.Close();
		}

		private void sendPicture()
		{
			displayShareware();

			if (this.sharewareVersion == false)
			{
				//Process.Start("mailto:TODO?subject=PJPaint - http://www.fullerdata.com/pjpaint&body=TODO");

				try
				{
					/*
					MailMessage objMessage = new MailMessage();
					string sFile = "c:\\logon.txt";
					MailAttachment objAttachment = new MailAttachment(sFile);

					objMessage.To = "TODO Recipient";
					objMessage.From = "TODO From?";
					objMessage.Bcc = "";
					objMessage.Subject = "PJPaint - http://www.fullerdata.com/pjpaint";
					objMessage.Body = "TODO Comment";
					objMessage.Attachments.Add(objAttachment);

					//SmtpMail.SmtpServer = "48.11.15.4";
					SmtpMail.Send(objMessage);

					objAttachment = null;
					objMessage = null;
					*/
				}
				catch (Exception e)
				{
					MessageBox.Show("Sorry, we couldn't send the email - " + e.Message);
				}
			}
		}

		private void printPicture()
		{
			displayShareware();

			if (this.sharewareVersion == false)
			{
				PrintDocument pd = new PrintDocument();

				if (storedPageSettings == null) 
				{
					setupPrinter();
				}

				pd.DefaultPageSettings = storedPageSettings;

				printDialog1.Document = pd;
				DialogResult result = this.printDialog1.ShowDialog();

				if (result == DialogResult.OK) 
				{
					pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
					pd.Print();
				}			
			}
		}

		private void pd_PrintPage(object sender, PrintPageEventArgs ev) 
		{
			Graphics g = ev.Graphics;

			// draw picture
			g.DrawImage(pjColouring, ev.MarginBounds.X, ev.MarginBounds.Y, ev.MarginBounds.Width, ev.MarginBounds.Height);

			// draw stamps
			for (int i = 0; i < topStamp; i++)
			{
				int translatedx = stampList[i].imageX;
				int translatedy = stampList[i].imageY;
				makeRawPrintAbsolute(ref translatedx, ref translatedy, ev);
				translatedx+=ev.MarginBounds.X;
				translatedy+=ev.MarginBounds.Y;

				g.DrawImage(stampList[i].imageValue, translatedx, translatedy);
			}
			
			// draw writing
			for (int i = 0; i < topText; i++)
			{
				int translatedx = writingList[i].textX;
				int translatedy = writingList[i].textY;
				makeRawPrintAbsolute(ref translatedx, ref translatedy, ev);
				translatedx+=ev.MarginBounds.X;
				translatedy+=ev.MarginBounds.Y;

				Font fontText = new System.Drawing.Font(writingList[i].textFont, writingList[i].textSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
				Brush brushText = new SolidBrush(Color.FromArgb(writingList[i].red, writingList[i].green, writingList[i].blue));
				g.DrawString(writingList[i].textValue, fontText, brushText, translatedx, translatedy);
			}

			ev.HasMorePages = false ;
		}

		protected void makeRawPrintAbsolute(ref int picx, ref int picy, PrintPageEventArgs ev)
		{
			double relativex = (double) picx / (double) this.pjColouring.Width;
			double relativey = (double) picy / (double) this.pjColouring.Height;

			relativex = (double) ev.MarginBounds.Width * relativex;
			relativey = (double) ev.MarginBounds.Height * relativey;

			picx = Convert.ToInt32(relativex);
			picy = Convert.ToInt32(relativey);
		}


		private void setupPrinter()
		{
			PageSetupDialog psDlg = new PageSetupDialog() ;

			if (storedPageSettings == null) 
			{
				storedPageSettings =  new PageSettings();
				storedPageSettings.Landscape = true;
			}

			psDlg.PageSettings = storedPageSettings;
			psDlg.ShowDialog();	
		}

		private void menuNew_Click(object sender, System.EventArgs e)
		{
			newPicture();
		}

		private void menuOpen_Click(object sender, System.EventArgs e)
		{
			openPicture();
		}

		private void menuSend_Click(object sender, System.EventArgs e)
		{
			sendPicture();
		}

		private void menuPrint_Click(object sender, System.EventArgs e)
		{
			printPicture();
		}

		private void menuSaveAs_Click(object sender, System.EventArgs e)
		{
			savePicture();
		}

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			DialogResult ok = MessageBox.Show("Do you want to quit PJPaint ??", "Exit PJPaint", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (ok == DialogResult.Yes)
			{
				displayShareware();

				this.Close();
			}
		}

		private void exitEvent(object sender, CancelEventArgs e)
		{
			DialogResult ok = MessageBox.Show("Do you want to quit PJPaint ??", "Exit PJPaint", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (ok != DialogResult.Yes)
			{
				e.Cancel = true;
			}
			else
			{
				displayShareware();
			}
		}

		private void setModeColouring()
		{
			currentDrawMode = toolbarSelect.Paint;

			this.menuColour.Checked = true;
			this.menuText.Checked = false;
			this.menuStamp.Checked = false;

			this.gotText = false;
			this.gotStamp = false;
		}

		private void setModeWriting()
		{
			currentDrawMode = toolbarSelect.Write;

			this.menuColour.Checked = false;
			this.menuText.Checked = true;
			this.menuStamp.Checked = false;

			this.currentText = "";

			CreateText newWriting = new CreateText();
			newWriting.setFont = this.currentTextFont;
			newWriting.setSize = this.currentTextSize;
			DialogResult ok = newWriting.ShowDialog();

			if (ok == DialogResult.OK)
			{
				this.currentText = newWriting.setText;
				this.currentTextFont = newWriting.setFont;
				this.currentTextSize = newWriting.setSize;
				this.gotText = true;
				this.gotStamp = false;
			}			
		}

		private void setModeStamping()
		{
			currentDrawMode = toolbarSelect.Stamp;

			this.menuColour.Checked = false;
			this.menuText.Checked = false;
			this.menuStamp.Checked = true;

			ChooseStamp newStamp = new ChooseStamp();
			DialogResult ok = newStamp.ShowDialog();

			if (ok == DialogResult.OK)
			{
				this.currentStamp = (Bitmap) Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Stamps\\" + newStamp.currentFilename);
				this.currentStampFile = newStamp.currentFilename;
				this.currentStamp.MakeTransparent(Color.White);
				this.gotStamp = true;
				this.gotText = false;
			}
		}

		private void menuColour_Click(object sender, System.EventArgs e)
		{
			setModeColouring();

			this.Invalidate();
		}

		private void menuStamp_Click(object sender, System.EventArgs e)
		{
			setModeStamping();

			this.Invalidate();
		}

		private void menuText_Click(object sender, System.EventArgs e)
		{
			setModeWriting();

			this.Invalidate();
		}

		private void menuHelp_Click(object sender, System.EventArgs e)
		{
			Process ieProcess = new Process();
			ieProcess.StartInfo.FileName = "iexplore.exe";
			ieProcess.StartInfo.Arguments = "http://www.fullerdata.com/pjpaint/default.aspx?Action=Help";
			ieProcess.Start();		
		}

		private void testToolbar(int hitX, int hitY)
		{
			currentToolHover = toolbarSelect.Nothing;

			// New
			if (hitX >= 5 && hitX <= 5+48)	
				currentToolHover = toolbarSelect.New;

			// Open
			if (hitX >= 65 && hitX < 65+48)	
				currentToolHover = toolbarSelect.Open;

			// Save
			if (hitX >= 125 && hitX < 125+48)	
				currentToolHover = toolbarSelect.Save;

			// Email
			//if (hitX >= 185 && hitX < 185+48)	
			//	currentToolHover = toolbarSelect.Email;

			// Print
			//if (hitX >= 245 && hitX < 245+48)	
			//	currentToolHover = toolbarSelect.Print;
			if (hitX >= 185 && hitX < 185+48)	
				currentToolHover = toolbarSelect.Print;

			// Paint
			if (hitX >= this.ClientRectangle.Width-175 && hitX < this.ClientRectangle.Width-175+48)
				currentToolHover = toolbarSelect.Paint;

			// Write
			if (hitX >= this.ClientRectangle.Width-115 && hitX < this.ClientRectangle.Width-115+48)
				currentToolHover = toolbarSelect.Write;

			// Stamp
			if (hitX >= this.ClientRectangle.Width-55 && hitX < this.ClientRectangle.Width-55+48)
				currentToolHover = toolbarSelect.Stamp;
		}

		private void clickToolbar(toolbarSelect selected)
		{
			switch (selected)
			{
				case (toolbarSelect.New):
					newPicture();
					break;
				case (toolbarSelect.Open):
					openPicture();
					break;
				case (toolbarSelect.Save):
					savePicture();
					break;
				case (toolbarSelect.Email):
					sendPicture();
					break;
				case (toolbarSelect.Print):
					printPicture();
					break;
				case (toolbarSelect.Paint):
					setModeColouring();
					break;
				case (toolbarSelect.Write):
					setModeWriting();
					break;
				case (toolbarSelect.Stamp):
					setModeStamping();
					break;
				default:
					break;
			}
		}

		private void displayShareware()
		{
			if (this.sharewareVersion == true)
			{
				PleaseRegister register = new PleaseRegister();
				register.ShowDialog();
			}
		}

		private void menuClearColouring_Click(object sender, System.EventArgs e)
		{
			DialogResult ok = MessageBox.Show("Are you sure you want to clear all your colouring ??", "Clear Colouring", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (ok == DialogResult.Yes)
			{
				pjColouring = (Bitmap) Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Book\\" + m_currentFile);
				this.Invalidate();
			}
		}

		private void menuClearWriting_Click(object sender, System.EventArgs e)
		{
			DialogResult ok = MessageBox.Show("Are you sure you want to clear all of your writing ??", "Clear Writing", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (ok == DialogResult.Yes)
			{
				topText = 0;
				this.Invalidate();
			}
		}

		private void menuClearStamps_Click(object sender, System.EventArgs e)
		{
			DialogResult ok = MessageBox.Show("Are you sure you want to clear all of your stamps ??", "Clear Stamps", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (ok == DialogResult.Yes)
			{
				topStamp = 0;
				this.Invalidate();
			}
		}



		private void menuPageSetup_Click(object sender, System.EventArgs e)
		{
			setupPrinter();
		}
	}
}
