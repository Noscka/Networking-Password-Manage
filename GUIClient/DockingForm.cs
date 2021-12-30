using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GUIClient
{
	public partial class DockingForm : Form
	{
		#region TopBar EDITED
		[DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		protected override CreateParams CreateParams
		{
			get
			{
				const int CS_DROPSHADOW = 0x20000;
				CreateParams cp = base.CreateParams;
				cp.ClassStyle |= CS_DROPSHADOW;
				return cp;
			}
		}

		private void Control_Bar_MouseDown(Object sender, MouseEventArgs e)
		{
			const int WM_NCLBUTTONDOWN = 0xA1;
			const int HT_CAPTION = 0x2;

			if (e.Button == MouseButtons.Left)
			{
				Thread DockinBarThread = new Thread(() => DockingBar(this));
				DockinBarThread.Name = "Docking Bar Thread";
				DockinBarThread.Start();

				MRSE.Reset();

				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);

				Docked = DockingValidation();
				MRSE.Set();
				DoneSettingBar = true;
			}
		}

		private void ExitBox_Click(object sender, EventArgs e)
		{
			FormParent.FormParent.Close();
		}

		private void ExitBox_MouseHover(object sender, EventArgs e)
		{
			ExitPictureBox.BackColor = Color.FromArgb(200, 30, 30);
		}

		private void ExitBox_MouseLeave(object sender, EventArgs e)
		{
			ExitPictureBox.BackColor = Color.Transparent;
		}

		private void MinimisePictureBox_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void MinimisePictureBox_MouseHover(object sender, EventArgs e)
		{
			MinimisePictureBox.BackColor = Color.FromArgb(52, 60, 56);
		}

		private void MinimisePictureBox_MouseLeave(object sender, EventArgs e)
		{
			MinimisePictureBox.BackColor = Color.Transparent;
		}

		#endregion

		#region Global Variables
		public ChatForm FormParent = null;
		private bool Docked = false, DoneSettingBar = true;
		private int DockedHeight;
		private ManualResetEvent MRSE = new ManualResetEvent(false);
		#endregion

		public DockingForm(ChatForm Parent)
		{
			InitializeComponent();
			FormParent = Parent;
		}

		private void Form3_Load(Object sender, EventArgs e)
		{
			UsernameLabel.Text = $"Username: {MainForm.CurrentUser.Username}";

			Thread DockingThread = new Thread(() => DockedThread(this));
			DockingThread.Name = "Docking Thread";
			DockingThread.Start();
		}

		#region Threading And Validating
		private void DockingBar(DockingForm CurrentForm)
		{
			DoneSettingBar = false;
			while (true)
			{
				if (DoneSettingBar)
				{
					FormParent.Invoke((MethodInvoker)delegate
					{
						FormParent.DockBar.Visible = false;
					});

					return;
				}

				bool show = this.Left >= FormParent.Left + FormParent.Width - 30 && this.Left <= FormParent.Left + FormParent.Width + 30;

				CurrentForm.Invoke((MethodInvoker)delegate
				{
					FormParent.UpdateDockingBar(CurrentForm.Top - FormParent.Top, show);
				});
			}
		}

		private void DockedThread(DockingForm CurrentForm)
		{
			while (true)
			{
				while (Docked)
				{
					CurrentForm.Invoke((MethodInvoker)delegate
					{
						CurrentForm.Left = FormParent.Left + FormParent.Width + 10;
						CurrentForm.Top = FormParent.Top + DockedHeight;
					});

					MRSE.WaitOne();
				}
			}
		}
		#endregion

		private bool DockingValidation()
		{
			if (this.Top >= FormParent.Top && this.Top <= FormParent.Top + FormParent.Height)
			{
				if(this.Left >= FormParent.Left + FormParent.Width - 30 && this.Left <= FormParent.Left + FormParent.Width + 30)
				{
					DockedHeight = this.Top - FormParent.Top;
					return true;
				}
			}
			return false;
		}
	}
}
