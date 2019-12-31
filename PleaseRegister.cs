using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Timers;

using System.Diagnostics;

namespace PJPaint
{
	/// <summary>
	/// Summary description for PleaseRegister.
	/// </summary>
	public class PleaseRegister : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonRegisterNow;
		private System.Windows.Forms.Button buttonLater;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private System.Timers.Timer timeAnimate;

		const int coinheight = 80;
		const int coinwidth = 80;

		private int coinx = 0;
		private int coiny = 0;
		private int coinxv = 5;
		private int coinyv = 5;
		private int coinspin = 0;
		private int coinangle = 0;

		protected Bitmap backBuffer = null;
		protected Graphics backG = null;

		Image currentDrawing = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\SilverDollar.gif");
		Image title = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Title.jpg");

		private void OnTimeAnimate(Object src, ElapsedEventArgs e)
		{
			coinx += coinxv;
			coiny += coinyv;	
			coinangle += coinspin;

			if (coinangle > 359)
			{
				coinangle = 0;
			}

			if (coinangle < 0)
			{
				coinangle = 359;
			}

			if (coinx >= ClientRectangle.Width-coinwidth)
			{
				coinx = ClientRectangle.Width-coinwidth;
				coinxv = -coinxv;
			}

			if (coiny >= ClientRectangle.Height-coinheight)
			{
				coiny = ClientRectangle.Height-coinheight;
				coinyv = -coinyv;
			}

			if (coinx < 0)
			{
				coinx = 0;
				coinxv = -coinxv;
			}

			if (coiny < 0)
			{
				coiny = 0;
				coinyv = -coinyv;
			}

			this.Invalidate();		
		}

		public PleaseRegister()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.DoubleBuffer, true);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.buttonRegisterNow = new System.Windows.Forms.Button();
			this.buttonLater = new System.Windows.Forms.Button();
			this.timeAnimate = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.timeAnimate)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonRegisterNow
			// 
			this.buttonRegisterNow.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonRegisterNow.Location = new System.Drawing.Point(40, 216);
			this.buttonRegisterNow.Name = "buttonRegisterNow";
			this.buttonRegisterNow.Size = new System.Drawing.Size(280, 32);
			this.buttonRegisterNow.TabIndex = 0;
			this.buttonRegisterNow.Text = "Please Register";
			this.buttonRegisterNow.Click += new System.EventHandler(this.buttonRegisterNow_Click);
			// 
			// buttonLater
			// 
			this.buttonLater.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonLater.Location = new System.Drawing.Point(40, 8);
			this.buttonLater.Name = "buttonLater";
			this.buttonLater.Size = new System.Drawing.Size(280, 32);
			this.buttonLater.TabIndex = 1;
			this.buttonLater.Text = "Maybe Later";
			this.buttonLater.Click += new System.EventHandler(this.buttonLater_Click);
			// 
			// timeAnimate
			// 
			this.timeAnimate.Enabled = true;
			this.timeAnimate.Interval = 30;
			this.timeAnimate.SynchronizingObject = this;
			this.timeAnimate.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimeAnimate);
			// 
			// PleaseRegister
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(362, 263);
			this.ControlBox = false;
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonLater,
																		  this.buttonRegisterNow});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PleaseRegister";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Please Register";
			this.Load += new System.EventHandler(this.PleaseRegister_Load);
			((System.ComponentModel.ISupportInitialize)(this.timeAnimate)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void PleaseRegister_Load(object sender, System.EventArgs e)
		{
			Random rand = new Random();	
			int chance = rand.Next(1, 100);

			if (chance > 50)
			{
				int tempY = buttonRegisterNow.Top;
				buttonRegisterNow.Top = buttonLater.Top;
				buttonLater.Top = tempY;
			}

			coinxv = rand.Next(1, 5);
			coinyv = rand.Next(1, 5);
			coinspin = rand.Next(-2, 2);
			if (coinspin == 0) coinspin = 1;

			backBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, Graphics.FromHwnd(this.Handle));
			backG = Graphics.FromImage(backBuffer);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			
			backG.DrawImage(title, 0, 0, ClientRectangle.Width, ClientRectangle.Height);

			GraphicsContainer gContainer;
			Matrix myMatrix;
			gContainer = backG.BeginContainer();
			myMatrix = backG.Transform;
			if (coinangle != 0)
			{
				myMatrix.RotateAt(coinangle, new PointF(coinx + (coinwidth/2), coiny + (coinheight/2)), MatrixOrder.Append);
				backG.Transform = myMatrix;
			}		
			backG.DrawImage(currentDrawing, coinx, coiny, coinwidth, coinheight);
			backG.EndContainer(gContainer);
			
			g.DrawImage(backBuffer, 0, 0);

			base.OnPaint(e);
		}

		private void buttonRegisterNow_Click(object sender, System.EventArgs e)
		{
			Process ieProcess = new Process();
			ieProcess.StartInfo.FileName = "iexplore.exe";
			ieProcess.StartInfo.Arguments = "http://www.fullerdata.com/pjpaint/default.aspx?Action=Register";
			ieProcess.Start();	
			
			this.Close();
		}

		private void buttonLater_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
