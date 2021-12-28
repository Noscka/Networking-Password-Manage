using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GUIClient
{
	public partial class Form3 : Form
	{
		#region TopBar EDITED
		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;

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

		private void Control_Bar_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
				DockingValidation();
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
		public Form2 FormParent = null;
		private bool Docked = false;
		private ManualResetEvent MRSE = new ManualResetEvent(false);
		#endregion

		public Form3(Form2 Parent)
		{
			InitializeComponent();
			FormParent = Parent;
		}

		private void Form3_Load(Object sender, EventArgs e)
		{
			Thread DockingThread = new Thread(() => DockedThread(this));
			DockingThread.Name = "Docking Thread";
			DockingThread.Start();

			Thread DockingValidationThread = new Thread(() => DockingValidation(this));
			DockingValidationThread.Name = "Docking Validation Thread";
			DockingValidationThread.Start();
		}

		private void button1_Click(Object sender, EventArgs e)
		{
			Docked = !Docked;
		}

		#region Threading
		private void DockedThread(Form3 CurrentForm)
		{
			while (true)
			{
				while (Docked)
				{
					CurrentForm.Invoke((MethodInvoker)delegate
					{
						CurrentForm.Left = FormParent.Left + FormParent.Width + 10;
						CurrentForm.Top = FormParent.Top + (FormParent.Height - CurrentForm.Height) / 2;
					});
				}
			}
		}

		private void DockingValidation()
		{
			FormParent.WriteToConsole($"Validating");
			Docked = false;
			if (this.Top >= FormParent.Top && this.Top <= FormParent.Top + FormParent.Height)
			{
				//if (this.Left <= FormParent.Left + FormParent.Width - 10 && this.Left >= FormParent.Left + FormParent.Width + 10)
				if(true)
				{
					Docked = true;
					FormParent.WriteToConsole($"Validating");
				}
			}
		}

		private void DockingValidation(Form3 CurrentForm)
		{
			return;
			uint number = 0;

			FormParent.WriteToConsole($"{CurrentForm.Top} | {FormParent.Top}");

			while (true)
			{
				Docked = false;
				MRSE.WaitOne();

				if (CurrentForm.Top >= FormParent.Top && CurrentForm.Top <= FormParent.Top + FormParent.Height)
				{
					if (CurrentForm.Left <= FormParent.Left + FormParent.Width - 10 && CurrentForm.Left >= FormParent.Left + FormParent.Width + 10)
					{
						Docked = true;
						FormParent.WriteToConsole($"Stuff {number}");
					}
				}

				Thread.Sleep(10);
				number++;
			}
		}
		#endregion
	}
}
