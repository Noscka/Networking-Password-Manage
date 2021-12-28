﻿namespace GUIClient
{
	partial class Form3
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Control_Bar = new System.Windows.Forms.Panel();
			this.MinimisePictureBox = new System.Windows.Forms.PictureBox();
			this.ExitPictureBox = new System.Windows.Forms.PictureBox();
			this.button1 = new System.Windows.Forms.Button();
			this.Control_Bar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MinimisePictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// Control_Bar
			// 
			this.Control_Bar.BackColor = System.Drawing.Color.Transparent;
			this.Control_Bar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Control_Bar.Controls.Add(this.MinimisePictureBox);
			this.Control_Bar.Controls.Add(this.ExitPictureBox);
			this.Control_Bar.Dock = System.Windows.Forms.DockStyle.Top;
			this.Control_Bar.Location = new System.Drawing.Point(0, 0);
			this.Control_Bar.Name = "Control_Bar";
			this.Control_Bar.Size = new System.Drawing.Size(367, 30);
			this.Control_Bar.TabIndex = 4;
			this.Control_Bar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_Bar_MouseMove);
			// 
			// MinimisePictureBox
			// 
			this.MinimisePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
			this.MinimisePictureBox.Image = global::GUIClient.Properties.Resources._;
			this.MinimisePictureBox.Location = new System.Drawing.Point(305, 0);
			this.MinimisePictureBox.Name = "MinimisePictureBox";
			this.MinimisePictureBox.Size = new System.Drawing.Size(30, 28);
			this.MinimisePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.MinimisePictureBox.TabIndex = 3;
			this.MinimisePictureBox.TabStop = false;
			this.MinimisePictureBox.Click += new System.EventHandler(this.MinimisePictureBox_Click);
			this.MinimisePictureBox.MouseLeave += new System.EventHandler(this.MinimisePictureBox_MouseLeave);
			this.MinimisePictureBox.MouseHover += new System.EventHandler(this.MinimisePictureBox_MouseHover);
			// 
			// ExitPictureBox
			// 
			this.ExitPictureBox.Dock = System.Windows.Forms.DockStyle.Right;
			this.ExitPictureBox.Image = global::GUIClient.Properties.Resources.x;
			this.ExitPictureBox.Location = new System.Drawing.Point(335, 0);
			this.ExitPictureBox.Name = "ExitPictureBox";
			this.ExitPictureBox.Size = new System.Drawing.Size(30, 28);
			this.ExitPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.ExitPictureBox.TabIndex = 3;
			this.ExitPictureBox.TabStop = false;
			this.ExitPictureBox.Click += new System.EventHandler(this.ExitBox_Click);
			this.ExitPictureBox.MouseLeave += new System.EventHandler(this.ExitBox_MouseLeave);
			this.ExitPictureBox.MouseHover += new System.EventHandler(this.ExitBox_MouseHover);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(146, 153);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "Dock";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
			this.ClientSize = new System.Drawing.Size(367, 266);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.Control_Bar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form3";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Form3";
			this.Load += new System.EventHandler(this.Form3_Load);
			this.Control_Bar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MinimisePictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel Control_Bar;
		private System.Windows.Forms.PictureBox MinimisePictureBox;
		private System.Windows.Forms.PictureBox ExitPictureBox;
		private System.Windows.Forms.Button button1;
	}
}