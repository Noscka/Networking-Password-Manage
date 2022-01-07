using Networking.Packets;
using System;
using System.Threading;
using System.Windows.Forms;



namespace GUIClient
{
	public partial class ChatOptionsForm : Form
	{
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
			ChatOptionsDockingThread.Start();

			InSlideTimer.Interval = 1;
			OutSlideTimer.Interval = 1;
			InSlideTimer.Tick += SlideInTimerFunctinon;
			OutSlideTimer.Tick += SlideOutTimerFunctinon;
			MRSE.Set();
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

			Distance = Convert.ToInt16(FormParent.Left - this.Left);

			if (InDirection)
			{
				FormParent.WriteToConsole("Going In");
				InSlideTimer.Start();
			}
			else
			{
				this.Show(FormParent);
				FormParent.WriteToConsole("Going Out");
				OutSlideTimer.Start();
			}
		}

		private void SlideInTimerFunctinon(Object sender, EventArgs e)
		{
			FormParent.WriteToConsole(Distance >= FormParent.Left - this.Left ? "True" : "False");
			if (Distance >= FormParent.Left - this.Left)
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
			FormParent.WriteToConsole($"{FormParent.Left} | {this.Left} | {-this.Width} \\/ {Distance} | {this.Left} " + (Distance <= -this.Width ? "True" : "False"));
			if (Distance <= -this.Width)
			{
				OutSlideTimer.Stop();
				MRSE.Set();
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
				Thread.Sleep(1);

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
			MainForm.TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.LogOut));
		}
		
		private void TestButton_Click(Object sender, EventArgs e)
		{
			MRSE.Set();
		}
	}
}
