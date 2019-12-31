using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;

namespace PJPaint
{
	/// <summary>
	/// Summary description for AboutPJPaint.
	/// </summary>
	public class AboutPJPaint : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonClose;

		protected Image pjTitle;
		private System.Windows.Forms.PictureBox pictureBox1;
		public bool sharewareVersion = true;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutPJPaint()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			pjTitle = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Images\\Title.jpg");
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.DrawImage(pjTitle, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);

			Font fontText = new System.Drawing.Font("Comic Sans MS", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			Brush brushText = new SolidBrush(Color.Black);
			g.DrawString("Paige and James Paint", fontText, brushText, 50, 20);
			fontText = new System.Drawing.Font("Comic Sans MS", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			brushText = new SolidBrush(Color.Black);
			g.DrawString("Version 1.0 - December 2003", fontText, brushText, 50, 100);
			brushText = new SolidBrush(Color.Black);
			g.DrawString("Written By Lee H Fuller", fontText, brushText, 50, 120);
			g.DrawString("Tested By Paige Fuller & James Fuller", fontText, brushText, 50, 140);

            g.DrawString("Released as Freeware - October 2017", fontText, brushText, 50, 220);

            base.OnPaint(e);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutPJPaint));
            this.buttonClose = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.SystemColors.Control;
            this.buttonClose.Font = new System.Drawing.Font("Comic Sans MS", 12F);
            this.buttonClose.Location = new System.Drawing.Point(330, 8);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(96, 32);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "OK";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(40, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // AboutPJPaint
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(438, 281);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutPJPaint";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About PJPaint";
            this.Load += new System.EventHandler(this.AboutPJPaint_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void AboutPJPaint_Load(object sender, System.EventArgs e)
		{
			//if (sharewareVersion == false) this.buttonRegister.Visible = false;	
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void buttonRegister_Click(object sender, System.EventArgs e)
		{
			//Process ieProcess = new Process();
			//ieProcess.StartInfo.FileName = "iexplore.exe";
			//ieProcess.StartInfo.Arguments = "http://www.fullerdata.com/pjpaint/default.aspx?Action=Register";
			//ieProcess.Start();		
		}

	}
}
