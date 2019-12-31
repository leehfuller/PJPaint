using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PJPaint
{
	/// <summary>
	/// Summary description for ChooseStamp.
	/// </summary>
	public class ChooseStamp : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button buttonPrevious;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected Image currentDrawing;
		public string currentFilename = "";
		protected int currentFileIndex = 0;
		string [] listPictures;

		public ChooseStamp()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

			string[] filesList = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Stamps");
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

		private void showPicture()
		{
			currentFilename = listPictures[currentFileIndex];
			currentDrawing = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Stamps\\" + currentFilename);
			this.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			g.DrawImage(currentDrawing, 20, 10, 80, 80);

			Pen p = new Pen(Color.Black);
			g.DrawRectangle(p, 20, 10, 80, 80);

			Font fontText = new System.Drawing.Font("Comic Sans MS", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			Brush brushText = new SolidBrush(Color.Black);
			g.DrawString(currentFilename, fontText, brushText, 20, 90);

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
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.buttonPrevious = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonOK.Location = new System.Drawing.Point(128, 8);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(96, 32);
			this.buttonOK.TabIndex = 0;
			this.buttonOK.Text = "Select";
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonCancel.Location = new System.Drawing.Point(128, 48);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 32);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			// 
			// buttonNext
			// 
			this.buttonNext.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonNext.Location = new System.Drawing.Point(64, 112);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(40, 32);
			this.buttonNext.TabIndex = 2;
			this.buttonNext.Text = ">>";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// buttonPrevious
			// 
			this.buttonPrevious.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonPrevious.Location = new System.Drawing.Point(16, 112);
			this.buttonPrevious.Name = "buttonPrevious";
			this.buttonPrevious.Size = new System.Drawing.Size(40, 32);
			this.buttonPrevious.TabIndex = 3;
			this.buttonPrevious.Text = "<<";
			this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
			// 
			// ChooseStamp
			// 
			this.AcceptButton = this.buttonOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(234, 159);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonPrevious,
																		  this.buttonNext,
																		  this.buttonCancel,
																		  this.buttonOK});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ChooseStamp";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Choose a Stamp";
			this.Load += new System.EventHandler(this.ChooseStamp_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void ChooseStamp_Load(object sender, System.EventArgs e)
		{
		
		}

		private void buttonPrevious_Click(object sender, System.EventArgs e)
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
	}
}
