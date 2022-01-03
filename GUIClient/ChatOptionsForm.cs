using Networking.Packets;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace GUIClient
{
	public partial class ChatOptionsForm : Form
	{
		public static ChatForm FormParent;

		public ChatOptionsForm(ChatForm formParent)
		{
			FormParent = formParent;
			InitializeComponent();
		}

		private void ChatOptionsForm_Load(Object sender, EventArgs e)
		{
			Thread ChatOptionsDockingThread = new Thread(() => DockedThread(this));
			ChatOptionsDockingThread.Start();
		}

		private void ChatOptionsForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
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
			}
		}
		#endregion

		private void LogOutButton_Click(Object sender, EventArgs e)
		{
			MainForm.TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.LogOut));

		}
	}

	public class LabelButton : Label
	{
		private System.Windows.Forms.Timer FadeOutTimer = new System.Windows.Forms.Timer(), FadeInTimer = new System.Windows.Forms.Timer();

		private UInt16 Opacity = 0;

		public LabelButton()
		{
			this.FadeOutTimer.Interval = 1;
			this.FadeInTimer.Interval = 1;

			this.MouseLeave += new EventHandler(this.LabelButton_MouseLeave);
			this.MouseHover += new EventHandler(this.LabelButton_MouseHover);

			this.FadeInTimer.Tick += FadeIn;
			this.FadeOutTimer.Tick += FadeOut;

			this.Cursor = Cursors.Hand;
		}

		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
			this.ForeColor = Color.FromArgb(150, 150, 150);
			this.Font = new Font("Helvetica Rounded", 13F, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.BackColor = Color.FromArgb(Opacity, 160, 120, 230);
			this.AutoSize = false;
			this.TextAlign = ContentAlignment.MiddleCenter;
		}

		private void RedrawBackground()
		{
			this.BackColor = Color.FromArgb(Opacity, 160, 120, 230);
		}

		private void LabelButton_MouseHover(Object sender, EventArgs e)
		{
			this.FadeOutTimer.Stop();
			this.FadeInTimer.Start();
		}

		private void LabelButton_MouseLeave(Object sender, EventArgs e)
		{
			this.FadeInTimer.Stop();
			this.FadeOutTimer.Start();
		}

		private void FadeIn(Object Sender, EventArgs e)
		{
			if (Opacity >= 150)
			{
				FadeOutTimer.Stop();
			}
			else
			{
				Opacity += 30;
			}
			RedrawBackground();
		}

		private void FadeOut(Object Sender, EventArgs e)
		{
			if (Opacity <= 1)
			{
				FadeOutTimer.Stop();
			}
			else
			{
				Opacity -= 15;
			}
			RedrawBackground();
		}
	}
}
