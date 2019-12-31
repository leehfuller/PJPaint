using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace PJPaint
{
	/// <summary>
	/// Summary description for CreateText.
	/// </summary>
	public class CreateText : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonPlaceText;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.FontDialog fontDialog1;
		private System.Windows.Forms.Button buttonFont;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label1;

		public string setText = "";
		public string setFont = "";
		public float setSize = 8;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CreateText()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			this.buttonPlaceText = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.fontDialog1 = new System.Windows.Forms.FontDialog();
			this.buttonFont = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// buttonPlaceText
			// 
			this.buttonPlaceText.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonPlaceText.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonPlaceText.Location = new System.Drawing.Point(192, 144);
			this.buttonPlaceText.Name = "buttonPlaceText";
			this.buttonPlaceText.Size = new System.Drawing.Size(128, 32);
			this.buttonPlaceText.TabIndex = 1;
			this.buttonPlaceText.Text = "Place Text";
			this.buttonPlaceText.Click += new System.EventHandler(this.buttonPlaceText_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonCancel.Location = new System.Drawing.Point(328, 144);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(128, 32);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonFont
			// 
			this.buttonFont.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.buttonFont.Location = new System.Drawing.Point(16, 144);
			this.buttonFont.Name = "buttonFont";
			this.buttonFont.Size = new System.Drawing.Size(128, 32);
			this.buttonFont.TabIndex = 2;
			this.buttonFont.Text = "Font...";
			this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
			// 
			// textBox1
			// 
			this.textBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.textBox1.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.textBox1.Location = new System.Drawing.Point(16, 40);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox1.Size = new System.Drawing.Size(440, 80);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Comic Sans MS", 12F);
			this.label1.Location = new System.Drawing.Point(16, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(440, 24);
			this.label1.TabIndex = 4;
			this.label1.Text = "What would you like to write?";
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// CreateText
			// 
			this.AcceptButton = this.buttonPlaceText;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(466, 191);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.label1,
																		  this.textBox1,
																		  this.buttonFont,
																		  this.buttonCancel,
																		  this.buttonPlaceText});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CreateText";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Writing";
			this.ResumeLayout(false);

		}
		#endregion

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void buttonPlaceText_Click(object sender, System.EventArgs e)
		{
			setText = textBox1.Text;
		}

		private void buttonFont_Click(object sender, System.EventArgs e)
		{
			Font fontText = new System.Drawing.Font(setFont, setSize, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			fontDialog1.Font = fontText;
			DialogResult ok = fontDialog1.ShowDialog();

			if (ok == DialogResult.OK)
			{
				setFont = fontDialog1.Font.FontFamily.Name;
				setSize = fontDialog1.Font.Size;
			}
		}

		private void label1_Click(object sender, System.EventArgs e)
		{
		
		}
	}
}
