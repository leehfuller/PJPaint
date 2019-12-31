using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PJPaint
{
	/// <summary>
	/// Summary description for ChoosePicture.
	/// </summary>
	public class ChoosePicture : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonOpen;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonRandom;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected Image currentDrawing;
		public string currentFilename = "Bears.jpg";
		protected int currentFileIndex = 0;
		string [] listPictures;
		public bool selectedPicture = false;
		private Random rand = new Random();

		public ChoosePicture()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

			string[] filesList = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Book");
			int i = 0;
			listPictures = new string[filesList.Length];

			int index = 0;
			foreach (string file in filesList)
			{
				index = file.LastIndexOf("\\");
				listPictures[i] = file.Substring(index+1);
				i++;
			}

			currentFileIndex = 0;

			showPicture();
		}

		public void findPicture(string pic)
		{
			int i=0;
			bool foundPic = false;
			while (i < listPictures.Length && foundPic == false)
			{
				if (listPictures[i] == pic)
				{
					foundPic = true;
					currentFileIndex = i;
				}

				i++;
			}

			showPicture();
		}

		private void showPicture()
		{
			currentFilename = listPictures[currentFileIndex];
			currentDrawing = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Book\\" + currentFilename);
			this.Invalidate();
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
			this.button1 = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonOpen = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonRandom = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.button1.Location = new System.Drawing.Point(16, 224);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 32);
			this.button1.TabIndex = 0;
			this.button1.Text = "<< Previous";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonNext.Location = new System.Drawing.Point(168, 224);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(128, 32);
			this.buttonNext.TabIndex = 1;
			this.buttonNext.Text = "Next >>";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonOpen
			// 
			this.buttonOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOpen.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonOpen.Location = new System.Drawing.Point(312, 8);
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.Size = new System.Drawing.Size(88, 32);
			this.buttonOpen.TabIndex = 2;
			this.buttonOpen.Text = "Open";
			this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonCancel.Location = new System.Drawing.Point(312, 56);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(88, 32);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonRandom
			// 
			this.buttonRandom.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonRandom.Location = new System.Drawing.Point(312, 224);
			this.buttonRandom.Name = "buttonRandom";
			this.buttonRandom.Size = new System.Drawing.Size(88, 32);
			this.buttonRandom.TabIndex = 4;
			this.buttonRandom.Text = "Random";
			this.buttonRandom.Click += new System.EventHandler(this.buttonRandom_Click);
			// 
			// ChoosePicture
			// 
			this.AcceptButton = this.buttonOpen;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(410, 271);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonRandom,
																		  this.buttonCancel,
																		  this.buttonOpen,
																		  this.buttonNext,
																		  this.button1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChoosePicture";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose a New Picture";
			this.Load += new System.EventHandler(this.ChoosePicture_Load);
			this.ResumeLayout(false);

		}
		#endregion

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.DrawImage(currentDrawing, 10, 10, 293, 200);

			Pen p = new Pen(Color.Black);
			g.DrawRectangle(p, 10, 10, 293, 200);

			Font fontText = new System.Drawing.Font("Comic Sans MS", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			Brush brushText = new SolidBrush(Color.Black);
			g.DrawString(currentFilename, fontText, brushText, 20, 20);

			base.OnPaint(e);
		}

		private void ChoosePicture_Load(object sender, System.EventArgs e)
		{
		}

		private void buttonOpen_Click(object sender, System.EventArgs e)
		{
			selectedPicture = true;
			this.Close();
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			selectedPicture = false;
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (currentFileIndex > 0)
				currentFileIndex--;
			else
				currentFileIndex = listPictures.Length-1;

			showPicture();
		}

		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			if (currentFileIndex < listPictures.Length-1)
				currentFileIndex++;
			else
				currentFileIndex = 0;

			showPicture();
		}

		private void buttonRandom_Click(object sender, System.EventArgs e)
		{
			currentFileIndex = rand.Next(0, listPictures.Length-1);

			showPicture();
		}

	}
}
