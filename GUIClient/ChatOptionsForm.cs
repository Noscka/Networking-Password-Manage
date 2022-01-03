using Networking.Packets;
using System;
using System.Threading;
using System.Windows.Forms;



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
}
