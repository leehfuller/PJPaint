using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace PJPaint
{
	/// <summary>
	/// Summary description for OpenPicture.
	/// </summary>
	public class OpenPicture : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOpen;
		private System.Windows.Forms.Button buttonNext;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button buttonFolder;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		protected Image currentDrawing;
		public string currentFilename = "";
		public string currentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		protected int currentFileIndex = -1;
		string [] listPictures;
		public bool selectedPicture = false;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.Label labelPath;
		int countJPG = 0;

		private void walkFolder()
		{
			string[] filesList = System.IO.Directory.GetFiles(currentPath);
			listPictures = new string[filesList.Length];

			int index = 0;
			foreach (string file in filesList)
			{
				if (Path.GetExtension(file) == ".jpg")
				{
					index = file.LastIndexOf("\\");
					listPictures[countJPG] = file.Substring(index+1);
					countJPG++;
				}
			}

			if (countJPG > 0) 
				currentFileIndex = 0;
			else
				currentFileIndex = -1;
		}

		public OpenPicture()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.DoubleBuffer, true);

			labelPath.Text = currentPath;

			walkFolder();
			showPicture();
		}

		private void showPicture()
		{
			if (currentFileIndex >= 0)
			{
				currentFilename = listPictures[currentFileIndex];
				currentDrawing = Image.FromFile(currentPath + "\\" + currentFilename);
				this.Invalidate();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			if (currentDrawing != null)
			{
				g.DrawImage(currentDrawing, 10, 10, 293, 200);
			}

			Pen p = new Pen(Color.Black);
			g.DrawRectangle(p, 10, 10, 293, 200);

			Font fontText = new System.Drawing.Font("Comic Sans MS", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			Brush brushText = new SolidBrush(Color.Black);
			g.DrawString(currentFilename, fontText, brushText, 20, 20);

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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOpen = new System.Windows.Forms.Button();
			this.buttonNext = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.buttonFolder = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.labelPath = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonCancel.Location = new System.Drawing.Point(312, 56);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(88, 32);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOpen
			// 
			this.buttonOpen.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOpen.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonOpen.Location = new System.Drawing.Point(312, 8);
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.Size = new System.Drawing.Size(88, 32);
			this.buttonOpen.TabIndex = 4;
			this.buttonOpen.Text = "Open";
			this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
			// 
			// buttonNext
			// 
			this.buttonNext.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonNext.Location = new System.Drawing.Point(160, 240);
			this.buttonNext.Name = "buttonNext";
			this.buttonNext.Size = new System.Drawing.Size(128, 32);
			this.buttonNext.TabIndex = 7;
			this.buttonNext.Text = "Next >>";
			this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.button1.Location = new System.Drawing.Point(8, 240);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 32);
			this.button1.TabIndex = 6;
			this.button1.Text = "<< Previous";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// buttonFolder
			// 
			this.buttonFolder.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonFolder.Location = new System.Drawing.Point(312, 240);
			this.buttonFolder.Name = "buttonFolder";
			this.buttonFolder.Size = new System.Drawing.Size(88, 32);
			this.buttonFolder.TabIndex = 8;
			this.buttonFolder.Text = "Folder...";
			this.buttonFolder.Visible = false;
			this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
			// 
			// labelPath
			// 
			this.labelPath.Location = new System.Drawing.Point(8, 216);
			this.labelPath.Name = "labelPath";
			this.labelPath.Size = new System.Drawing.Size(392, 16);
			this.labelPath.TabIndex = 9;
			this.labelPath.Text = "path";
			// 
			// OpenPicture
			// 
			this.AcceptButton = this.buttonOpen;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(410, 287);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.labelPath,
																		  this.buttonFolder,
																		  this.buttonNext,
																		  this.button1,
																		  this.buttonCancel,
																		  this.buttonOpen});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OpenPicture";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Open a Saved Picture";
			this.ResumeLayout(false);

		}
		#endregion

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

		private void buttonFolder_Click(object sender, System.EventArgs e)
		{
			openFileDialog1.CheckFileExists = false;
			openFileDialog1.DefaultExt = "xml";
			openFileDialog1.Filter = "My Pictures (*.xml)|*.xml|All files (*.*)|*.*";
			openFileDialog1.Title = "Save Picture";
			openFileDialog1.InitialDirectory = this.currentPath;
			DialogResult ok = openFileDialog1.ShowDialog();

			if (ok == DialogResult.OK)
			{
				//currentFilename = Path.GetFileName(openFileDialog1.FileName);
				currentPath = Path.GetDirectoryName(openFileDialog1.FileName);
				labelPath.Text = currentPath;
				currentFileIndex = -1;
				countJPG = -1;
				currentDrawing = null;
				currentFileIndex = -1;

				walkFolder();

				showPicture();
			}		
		}

		private void buttonNext_Click(object sender, System.EventArgs e)
		{
			if (currentFileIndex < countJPG-1)
				currentFileIndex++;
			else
				currentFileIndex = 0;

			showPicture();	
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (currentFileIndex > 0)
				currentFileIndex--;
			else
				currentFileIndex = countJPG-1;

			showPicture();
		
		}
	}
}
