using Networking.Packets;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;



namespace GUIClient
{
	public partial class ChatOptionsForm : Form
	{
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

		public static ChatForm FormParent;
		private readonly ManualResetEvent MRSE = new ManualResetEvent(false);
		private System.Windows.Forms.Timer InSlideTimer = new System.Windows.Forms.Timer(), OutSlideTimer = new System.Windows.Forms.Timer();
		private Int16 Distance = 0;

		public ChatOptionsForm(ChatForm formParent)
		{
			InitializeComponent();
			FormParent = formParent;
		}

		private void ChatOptionsForm_Load(Object sender, EventArgs e)
		{
			Thread ChatOptionsDockingThread = new Thread(() => DockedThread(this));
			ChatOptionsDockingThread.IsBackground = true;
			ChatOptionsDockingThread.Start();

			InSlideTimer.Interval = 1;
			OutSlideTimer.Interval = 1;
			InSlideTimer.Tick += SlideInTimerFunctinon;
			OutSlideTimer.Tick += SlideOutTimerFunctinon;
			MRSE.Set();

			//System.Drawing.Drawing2D.GraphicsPath shape = new System.Drawing.Drawing2D.GraphicsPath();
			//shape.AddRectangle(new Rectangle(8, 40, 200, 200));
			//this.Region = new System.Drawing.Region(shape);
			//this.Top = (Screen.PrimaryScreen.WorkingArea.Height - 160) / 2;
			//this.Left = Screen.PrimaryScreen.WorkingArea.Left;
		}

		private void ChatOptionsForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}

		/// <summary>
		/// Funnction to make form slide out or in
		/// </summary>
		/// <param name="Direction">True is in and False is out</param>
		public void FormSlide(bool InDirection)
		{
			InSlideTimer.Stop();
			OutSlideTimer.Stop();
			MRSE.Reset();

			if (InDirection)
			{
				Distance = Convert.ToInt16(this.Left - FormParent.Left);
				this.Owner = null;
				FormParent.BringToFront();
				InSlideTimer.Start();
			}
			else
			{
				Distance = 0;
				this.Show();
				FormParent.BringToFront();
				OutSlideTimer.Start();
			}
		}

		private void SlideInTimerFunctinon(Object sender, EventArgs e)
		{
			if (FormParent.Left - this.Left <= 0)
			{
				this.Hide();
				InSlideTimer.Stop();
			}
			else
			{

				this.Top = FormParent.Top;
				this.Left = FormParent.Left + Distance;

				Distance += 10;
			}
		}
		
		private void SlideOutTimerFunctinon(Object sender, EventArgs e)
		{
			if (Distance <= -this.Width)
			{
				this.Owner = FormParent;
				MRSE.Set();
				OutSlideTimer.Stop();
			}
			else
			{
				this.Top = FormParent.Top;
				this.Left = FormParent.Left + Distance;

				Distance -= 10;
			}
		}

		#region Threading
		/// <summary>
		/// Follow Parent form
		/// </summary>
		/// <param name="CurrentForm">this form object instance</param>
		private void DockedThread(ChatOptionsForm CurrentForm)
		{
			while (true)
			{
				CurrentForm.Invoke((MethodInvoker)delegate
				{
					CurrentForm.Left = FormParent.Left - CurrentForm.Width;
					CurrentForm.Top = FormParent.Top;
				});

				MRSE.WaitOne();
			}
		}
		#endregion

		private void LogOutButton_Click(Object sender, EventArgs e)
		{
			MainForm.StartFormInstance.SoftRestart(SRReasons.srReasons.LogOut);
		}
		
		private void TestButton_Click(Object sender, EventArgs e)
		{
			//this.Region = new Region(new Rectangle(0, 0, this.Width, this.Height));
		}
	}
}
