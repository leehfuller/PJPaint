using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace PJPaint
{
	/// <summary>
	/// Summary description for SavePicture.
	/// </summary>
	public class SavePicture : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonFolder;
		private System.Windows.Forms.TextBox textBoxFile;
		private System.Windows.Forms.Label labelFile;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public string currentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		public string currentPicture = "";
		private System.Windows.Forms.Label labelPath;
		public string saveAs = "";

		public SavePicture()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			labelPath.Text = currentPath;
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
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonFolder = new System.Windows.Forms.Button();
			this.textBoxFile = new System.Windows.Forms.TextBox();
			this.labelFile = new System.Windows.Forms.Label();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.labelPath = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonSave
			// 
			this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonSave.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonSave.Location = new System.Drawing.Point(208, 136);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(112, 32);
			this.buttonSave.TabIndex = 1;
			this.buttonSave.Text = "Save";
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonCancel.Location = new System.Drawing.Point(328, 136);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(112, 32);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonFolder
			// 
			this.buttonFolder.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonFolder.Location = new System.Drawing.Point(16, 136);
			this.buttonFolder.Name = "buttonFolder";
			this.buttonFolder.Size = new System.Drawing.Size(112, 32);
			this.buttonFolder.TabIndex = 2;
			this.buttonFolder.Text = "Folder...";
			this.buttonFolder.Visible = false;
			this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
			// 
			// textBoxFile
			// 
			this.textBoxFile.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBoxFile.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.textBoxFile.Location = new System.Drawing.Point(16, 72);
			this.textBoxFile.Name = "textBoxFile";
			this.textBoxFile.Size = new System.Drawing.Size(424, 30);
			this.textBoxFile.TabIndex = 0;
			this.textBoxFile.Text = "";
			// 
			// labelFile
			// 
			this.labelFile.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.labelFile.Location = new System.Drawing.Point(16, 16);
			this.labelFile.Name = "labelFile";
			this.labelFile.Size = new System.Drawing.Size(424, 56);
			this.labelFile.TabIndex = 4;
			this.labelFile.Text = "What do you want to call this picture?";
			// 
			// labelPath
			// 
			this.labelPath.Location = new System.Drawing.Point(16, 104);
			this.labelPath.Name = "labelPath";
			this.labelPath.Size = new System.Drawing.Size(424, 16);
			this.labelPath.TabIndex = 5;
			this.labelPath.Text = "path";
			// 
			// SavePicture
			// 
			this.AcceptButton = this.buttonSave;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(450, 183);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.labelPath,
																		  this.labelFile,
																		  this.textBoxFile,
																		  this.buttonFolder,
																		  this.buttonCancel,
																		  this.buttonSave});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SavePicture";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Save My Picture";
			this.Load += new System.EventHandler(this.SavePicture_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void SavePicture_Load(object sender, System.EventArgs e)
		{
		
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
				currentPicture = Path.GetFileName(openFileDialog1.FileName);
				currentPath = Path.GetDirectoryName(openFileDialog1.FileName);
				labelPath.Text = currentPath;
			}
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void buttonSave_Click(object sender, System.EventArgs e)
		{
			if (textBoxFile.Text.Length > 0)
			{		
				string fileSave = textBoxFile.Text;
				fileSave = fileSave.Trim();
				saveAs = currentPath + "\\" +  fileSave;
				currentPicture = saveAs;

				//MessageBox.Show(saveAs);

				this.Close();
			}
			else
			{
				MessageBox.Show("Please type a name for this picture to save.", "Name Your Picture", MessageBoxButtons.OK, MessageBoxIcon.Stop);
			}		
		}
	}
}
