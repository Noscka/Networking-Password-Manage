namespace GUIClient
{
	partial class ChatOptionsForm
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
			this.LogOutButton = new Networking.Controls.LabelButton();
			this.TestButton = new Networking.Controls.LabelButton();
			this.SuspendLayout();
			//
			// TestButton
			//
			this.TestButton.Text = "Test Button";
			this.TestButton.Name = "TestButton";
			this.TestButton.Location = new System.Drawing.Point(48, 380);
			this.TestButton.Size = new System.Drawing.Size(124, 25);
			this.TestButton.Click += new System.EventHandler(TestButton_Click);
			// 
			// LogOutButton
			// 
			this.LogOutButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(160)))), ((int)(((byte)(120)))), ((int)(((byte)(230)))));
			this.LogOutButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LogOutButton.Font = new System.Drawing.Font("Helvetica Rounded", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LogOutButton.ForeColor = System.Drawing.Color.DarkRed;
			this.LogOutButton.Location = new System.Drawing.Point(48, 416);
			this.LogOutButton.Name = "LogOutButton";
			this.LogOutButton.Opacity = ((ushort)(0));
			this.LogOutButton.Size = new System.Drawing.Size(124, 25);
			this.LogOutButton.TabIndex = 0;
			this.LogOutButton.Text = "Log Out";
			this.LogOutButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.LogOutButton.Click += new System.EventHandler(this.LogOutButton_Click);
			// 
			// ChatOptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
			this.ClientSize = new System.Drawing.Size(220, 450);
			this.Controls.Add(this.LogOutButton);
			this.Controls.Add(this.TestButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "ChatOptionsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "ChatOptionsForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChatOptionsForm_FormClosing);
			this.Load += new System.EventHandler(this.ChatOptionsForm_Load);
			this.ResumeLayout(false);
		}

		#endregion

		private Networking.Controls.LabelButton LogOutButton;
		private Networking.Controls.LabelButton TestButton;
	}
}