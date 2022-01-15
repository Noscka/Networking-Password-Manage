namespace GUIClient
{
	partial class ChatForm
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
			this.ProfileSettingsPanel = new System.Windows.Forms.Panel();
			this.NewOutput = new System.Windows.Forms.FlowLayoutPanel();
			this.Output = new System.Windows.Forms.TextBox();
			this.Input = new System.Windows.Forms.TextBox();
			this.DockBar = new System.Windows.Forms.Panel();
			this.SlidePictureBox = new System.Windows.Forms.PictureBox();
			this.Control_Bar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MinimisePictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.SlidePictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// Control_Bar
			// 
			this.Control_Bar.BackColor = System.Drawing.Color.Transparent;
			this.Control_Bar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Control_Bar.Controls.Add(this.SlidePictureBox);
			this.Control_Bar.Controls.Add(this.MinimisePictureBox);
			this.Control_Bar.Controls.Add(this.ExitPictureBox);
			this.Control_Bar.Dock = System.Windows.Forms.DockStyle.Top;
			this.Control_Bar.Location = new System.Drawing.Point(0, 0);
			this.Control_Bar.Name = "Control_Bar";
			this.Control_Bar.Size = new System.Drawing.Size(800, 30);
			this.Control_Bar.TabIndex = 3;
			this.Control_Bar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_Bar_MouseMove);
			// 
			// MinimisePictureBox
			// 
			this.MinimisePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
			this.MinimisePictureBox.Image = global::GUIClient.Properties.Resources._;
			this.MinimisePictureBox.Location = new System.Drawing.Point(738, 0);
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
			this.ExitPictureBox.Location = new System.Drawing.Point(768, 0);
			this.ExitPictureBox.Name = "ExitPictureBox";
			this.ExitPictureBox.Size = new System.Drawing.Size(30, 28);
			this.ExitPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.ExitPictureBox.TabIndex = 3;
			this.ExitPictureBox.TabStop = false;
			this.ExitPictureBox.Click += new System.EventHandler(this.ExitBox_Click);
			this.ExitPictureBox.MouseLeave += new System.EventHandler(this.ExitBox_MouseLeave);
			this.ExitPictureBox.MouseHover += new System.EventHandler(this.ExitBox_MouseHover);
			//
			// ProfileSettingsPanel
			//
			this.ProfileSettingsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ProfileSettingsPanel.BackColor = System.Drawing.Color.White;
			this.ProfileSettingsPanel.Visible = false;
			// 
			// Output
			// 

			//new
			this.NewOutput.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.NewOutput.Location = new System.Drawing.Point(10, 40);
			this.NewOutput.Size = new System.Drawing.Size(780, 360);
			this.NewOutput.Name = "Output";


			//old
			this.Output.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.Output.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.Output.Cursor = System.Windows.Forms.Cursors.Default;
			this.Output.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.Output.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.Output.Location = new System.Drawing.Point(10, 40);
			this.Output.Multiline = true;
			this.Output.Name = "Output";
			this.Output.ReadOnly = true;
			this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
			this.Output.Size = new System.Drawing.Size(780, 360);
			this.Output.TabIndex = 4;
			this.Output.TabStop = false;
			this.Output.GotFocus += new System.EventHandler(this.Output_GotFocus);
			// 
			// Input
			// 
			this.Input.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Input.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Input.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.Input.Location = new System.Drawing.Point(10, 410);
			this.Input.MaxLength = 10000;
			this.Input.Name = "Input";
			this.Input.Size = new System.Drawing.Size(780, 29);
			this.Input.TabIndex = 0;
			this.Input.Enter += new System.EventHandler(this.Input_Enter);
			this.Input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Input_KeyDown);
			this.Input.Leave += new System.EventHandler(this.Input_Leave);
			// 
			// DockBar
			// 
			this.DockBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
			this.DockBar.Location = new System.Drawing.Point(795, 250);
			this.DockBar.Margin = new System.Windows.Forms.Padding(0);
			this.DockBar.Name = "DockBar";
			this.DockBar.Size = new System.Drawing.Size(4, 200);
			this.DockBar.TabIndex = 5;
			this.DockBar.Visible = false;
			// 
			// SlidePictureBox
			// 
			this.SlidePictureBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.SlidePictureBox.Image = global::GUIClient.Properties.Resources.Arrows;
			this.SlidePictureBox.Location = new System.Drawing.Point(0, 0);
			this.SlidePictureBox.Name = "SlidePictureBox";
			this.SlidePictureBox.Size = new System.Drawing.Size(30, 28);
			this.SlidePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.SlidePictureBox.TabIndex = 4;
			this.SlidePictureBox.TabStop = false;
			this.SlidePictureBox.Click += new System.EventHandler(this.SlidePictureBox_Click);
			// 
			// ChatForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.DockBar);
			this.Controls.Add(this.ProfileSettingsPanel);
			this.Controls.Add(this.NewOutput);
			this.Controls.Add(this.Output);
			this.Controls.Add(this.Input);
			this.Controls.Add(this.Control_Bar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ChatForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ChatForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatForm_FormClosing);
			this.Load += new System.EventHandler(this.Form2_Load);
			this.Control_Bar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MinimisePictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.SlidePictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel Control_Bar;
		private System.Windows.Forms.PictureBox MinimisePictureBox;
		private System.Windows.Forms.PictureBox ExitPictureBox;
		public System.Windows.Forms.Panel ProfileSettingsPanel;
		private System.Windows.Forms.FlowLayoutPanel NewOutput;
		private System.Windows.Forms.TextBox Output;
		private System.Windows.Forms.TextBox Input;
		public System.Windows.Forms.Panel DockBar;
		private System.Windows.Forms.PictureBox SlidePictureBox;
	}
}