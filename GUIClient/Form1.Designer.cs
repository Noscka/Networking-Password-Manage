namespace GUIClient
{
    partial class Form1
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
			this.BackgroundPictureBox = new System.Windows.Forms.PictureBox();
			this.Control_Bar = new System.Windows.Forms.Panel();
			this.MinimisePictureBox = new System.Windows.Forms.PictureBox();
			this.ExitPictureBox = new System.Windows.Forms.PictureBox();
			this.SignInAUpPanel = new System.Windows.Forms.Panel();
			this.SignUpButton = new System.Windows.Forms.Label();
			this.SignInButton = new System.Windows.Forms.Label();
			this.SignInUpPanel = new System.Windows.Forms.Panel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.ErrorInfoLogin = new System.Windows.Forms.Label();
			this.DoneButton = new System.Windows.Forms.Label();
			this.SignType = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.PasswordTextBox = new System.Windows.Forms.TextBox();
			this.UsernameTextBox = new System.Windows.Forms.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).BeginInit();
			this.Control_Bar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.MinimisePictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).BeginInit();
			this.SignInAUpPanel.SuspendLayout();
			this.SignInUpPanel.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// BackgroundPictureBox
			// 
			this.BackgroundPictureBox.Location = new System.Drawing.Point(0, 0);
			this.BackgroundPictureBox.Name = "BackgroundPictureBox";
			this.BackgroundPictureBox.Size = new System.Drawing.Size(859, 469);
			this.BackgroundPictureBox.TabIndex = 5;
			this.BackgroundPictureBox.TabStop = false;
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
			this.Control_Bar.Size = new System.Drawing.Size(649, 30);
			this.Control_Bar.TabIndex = 2;
			this.Control_Bar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Control_Bar_MouseMove);
			// 
			// MinimisePictureBox
			// 
			this.MinimisePictureBox.Dock = System.Windows.Forms.DockStyle.Right;
			this.MinimisePictureBox.Image = global::GUIClient.Properties.Resources._;
			this.MinimisePictureBox.Location = new System.Drawing.Point(587, 0);
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
			this.ExitPictureBox.Location = new System.Drawing.Point(617, 0);
			this.ExitPictureBox.Name = "ExitPictureBox";
			this.ExitPictureBox.Size = new System.Drawing.Size(30, 28);
			this.ExitPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.ExitPictureBox.TabIndex = 3;
			this.ExitPictureBox.TabStop = false;
			this.ExitPictureBox.Click += new System.EventHandler(this.ExitBox_Click);
			this.ExitPictureBox.MouseLeave += new System.EventHandler(this.ExitBox_MouseLeave);
			this.ExitPictureBox.MouseHover += new System.EventHandler(this.ExitBox_MouseHover);
			// 
			// SignInAUpPanel
			// 
			this.SignInAUpPanel.BackColor = System.Drawing.Color.Transparent;
			this.SignInAUpPanel.Controls.Add(this.SignUpButton);
			this.SignInAUpPanel.Controls.Add(this.SignInButton);
			this.SignInAUpPanel.Dock = System.Windows.Forms.DockStyle.Left;
			this.SignInAUpPanel.Location = new System.Drawing.Point(0, 30);
			this.SignInAUpPanel.Name = "SignInAUpPanel";
			this.SignInAUpPanel.Size = new System.Drawing.Size(100, 311);
			this.SignInAUpPanel.TabIndex = 3;
			// 
			// SignUpButton
			// 
			this.SignUpButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SignUpButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SignUpButton.Font = new System.Drawing.Font("Helvetica Rounded", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SignUpButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
			this.SignUpButton.Location = new System.Drawing.Point(7, 60);
			this.SignUpButton.Name = "SignUpButton";
			this.SignUpButton.Size = new System.Drawing.Size(87, 26);
			this.SignUpButton.TabIndex = 2;
			this.SignUpButton.Text = "Sign Up";
			this.SignUpButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.SignUpButton.Click += new System.EventHandler(this.SignUpButton_Click);
			this.SignUpButton.DoubleClick += new System.EventHandler(this.SignUpButton_DoubleClick);
			// 
			// SignInButton
			// 
			this.SignInButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SignInButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.SignInButton.Font = new System.Drawing.Font("Helvetica Rounded", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SignInButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
			this.SignInButton.Location = new System.Drawing.Point(7, 18);
			this.SignInButton.Name = "SignInButton";
			this.SignInButton.Size = new System.Drawing.Size(87, 26);
			this.SignInButton.TabIndex = 2;
			this.SignInButton.Text = "Sign In";
			this.SignInButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.SignInButton.Click += new System.EventHandler(this.SignInButton_Click);
			// 
			// SignInUpPanel
			// 
			this.SignInUpPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
			this.SignInUpPanel.Controls.Add(this.panel1);
			this.SignInUpPanel.Controls.Add(this.DoneButton);
			this.SignInUpPanel.Controls.Add(this.SignType);
			this.SignInUpPanel.Controls.Add(this.label2);
			this.SignInUpPanel.Controls.Add(this.label1);
			this.SignInUpPanel.Controls.Add(this.PasswordTextBox);
			this.SignInUpPanel.Controls.Add(this.UsernameTextBox);
			this.SignInUpPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SignInUpPanel.Location = new System.Drawing.Point(100, 30);
			this.SignInUpPanel.Name = "SignInUpPanel";
			this.SignInUpPanel.Size = new System.Drawing.Size(549, 311);
			this.SignInUpPanel.TabIndex = 4;
			this.SignInUpPanel.Visible = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.ErrorInfoLogin);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 223);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(549, 88);
			this.panel1.TabIndex = 5;
			// 
			// ErrorInfoLogin
			// 
			this.ErrorInfoLogin.AutoSize = true;
			this.ErrorInfoLogin.BackColor = System.Drawing.Color.Transparent;
			this.ErrorInfoLogin.Font = new System.Drawing.Font("Helvetica Rounded", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ErrorInfoLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(60)))), ((int)(((byte)(70)))));
			this.ErrorInfoLogin.Location = new System.Drawing.Point(6, 6);
			this.ErrorInfoLogin.Name = "ErrorInfoLogin";
			this.ErrorInfoLogin.Size = new System.Drawing.Size(96, 22);
			this.ErrorInfoLogin.TabIndex = 3;
			this.ErrorInfoLogin.Text = "ErrorInfo";
			this.ErrorInfoLogin.Visible = false;
			// 
			// DoneButton
			// 
			this.DoneButton.AutoSize = true;
			this.DoneButton.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.DoneButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.DoneButton.Enabled = false;
			this.DoneButton.Font = new System.Drawing.Font("Helvetica Rounded", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DoneButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
			this.DoneButton.Location = new System.Drawing.Point(378, 150);
			this.DoneButton.Name = "DoneButton";
			this.DoneButton.Size = new System.Drawing.Size(62, 24);
			this.DoneButton.TabIndex = 4;
			this.DoneButton.Text = "Done";
			this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
			// 
			// SignType
			// 
			this.SignType.AutoSize = true;
			this.SignType.BackColor = System.Drawing.Color.Transparent;
			this.SignType.Font = new System.Drawing.Font("Helvetica Rounded", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SignType.ForeColor = System.Drawing.Color.Teal;
			this.SignType.Location = new System.Drawing.Point(18, 18);
			this.SignType.Name = "SignType";
			this.SignType.Size = new System.Drawing.Size(57, 22);
			this.SignType.TabIndex = 3;
			this.SignType.Text = "Type";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.Font = new System.Drawing.Font("Helvetica Rounded", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.Purple;
			this.label2.Location = new System.Drawing.Point(94, 108);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(108, 22);
			this.label2.TabIndex = 3;
			this.label2.Text = "Password:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Helvetica Rounded", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Purple;
			this.label1.Location = new System.Drawing.Point(94, 66);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(121, 22);
			this.label1.TabIndex = 3;
			this.label1.Text = "Username: ";
			// 
			// PasswordTextBox
			// 
			this.PasswordTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
			this.PasswordTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.PasswordTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.PasswordTextBox.ForeColor = System.Drawing.Color.Green;
			this.PasswordTextBox.Location = new System.Drawing.Point(221, 106);
			this.PasswordTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.PasswordTextBox.MaxLength = 100;
			this.PasswordTextBox.Name = "PasswordTextBox";
			this.PasswordTextBox.Size = new System.Drawing.Size(219, 26);
			this.PasswordTextBox.TabIndex = 1;
			this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
			this.PasswordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordTextBox_KeyDown);
			// 
			// UsernameTextBox
			// 
			this.UsernameTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(5)))), ((int)(((byte)(5)))), ((int)(((byte)(5)))));
			this.UsernameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.UsernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
			this.UsernameTextBox.ForeColor = System.Drawing.Color.Green;
			this.UsernameTextBox.Location = new System.Drawing.Point(221, 64);
			this.UsernameTextBox.Margin = new System.Windows.Forms.Padding(0);
			this.UsernameTextBox.MaxLength = 100;
			this.UsernameTextBox.Name = "UsernameTextBox";
			this.UsernameTextBox.Size = new System.Drawing.Size(219, 26);
			this.UsernameTextBox.TabIndex = 1;
			this.UsernameTextBox.TextChanged += new System.EventHandler(this.UsernameTextBox_TextChanged);
			this.UsernameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UsernameTextBox_KeyDown);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(7)))), ((int)(((byte)(7)))));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.ClientSize = new System.Drawing.Size(649, 341);
			this.Controls.Add(this.SignInUpPanel);
			this.Controls.Add(this.SignInAUpPanel);
			this.Controls.Add(this.Control_Bar);
			this.Controls.Add(this.BackgroundPictureBox);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.BackgroundPictureBox)).EndInit();
			this.Control_Bar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.MinimisePictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ExitPictureBox)).EndInit();
			this.SignInAUpPanel.ResumeLayout(false);
			this.SignInUpPanel.ResumeLayout(false);
			this.SignInUpPanel.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.PictureBox ExitPictureBox;
		private System.Windows.Forms.PictureBox MinimisePictureBox;
		private System.Windows.Forms.Panel Control_Bar;
		private System.Windows.Forms.Panel SignInAUpPanel;
		private System.Windows.Forms.Panel SignInUpPanel;
		private System.Windows.Forms.PictureBox BackgroundPictureBox;
		private System.Windows.Forms.Label SignInButton;
		private System.Windows.Forms.Label SignUpButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox UsernameTextBox;
		private System.Windows.Forms.Label DoneButton;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox PasswordTextBox;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label SignType;
		public System.Windows.Forms.Label ErrorInfoLogin;
	}
}

