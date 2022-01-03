using Networking.ObjectStream;
using Networking.Packets;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace GUIClient
{
	public partial class ChatForm : Form
	{
		[DllImport("user32.dll")]
		static extern bool HideCaret(IntPtr hWnd);

		[DllImport("user32.dll")]
		static extern bool ShowCaret(IntPtr hWnd);

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
			}
		}

		private void ExitBox_Click(object sender, EventArgs e)
		{
			FormParent.Close();
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
		public static MainForm FormParent = null;
		#endregion

		private void ChatForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}
		public void WriteToConsole(String Message)
		{
			this.Output.AppendText(Message + Environment.NewLine);
		}

		public void UpdateDockingBar(int Height, bool LeftRightClose)
		{
			if (Height < 450 && Height > 30)
			{
				if (LeftRightClose)
				{
					DockBar.Visible = true;
					DockBar.Top = Height;
				}
				else
				{
					DockBar.Visible = false;
				}
			}
			else
			{
				DockBar.Visible = false;
			}
		}

		public ChatForm(MainForm Parent)
		{
			FormParent = Parent;
			InitializeComponent();
		}
		private void Form2_Load(Object sender, EventArgs e)
		{
			Thread TPCConnectionThread = new Thread(() => ListenThread(MainForm.TCPNetworkStream, this));
			TPCConnectionThread.Start();

			DockingForm InfoDockingForm = new DockingForm(this);
			InfoDockingForm.Left = this.Left + this.Width + 10;
			InfoDockingForm.Top = this.Top + (this.Height - InfoDockingForm.Height) / 2;
			InfoDockingForm.Show(this);

            ChatOptionsForm ChatOptionFormInstance = new ChatOptionsForm(this);
			ChatOptionFormInstance.Show(this);
		}

		private void Output_GotFocus(Object sender, EventArgs e)
		{
			HideCaret(((TextBox)sender).Handle);
		}

		private void Input_Enter(Object sender, EventArgs e)
		{
			if (Input.ForeColor == Color.Gray)
			{
				Input.Text = "";
				Input.ForeColor = Color.FromArgb(0, 192, 0);
			}
		}

		private void Input_Leave(Object sender, EventArgs e)
		{
			if (Input.Text.Length == 0)
			{
				Input.Text = "Send to everyone...";
				Input.ForeColor = Color.Gray;
			}
		}

		private void Input_KeyDown(Object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				MainForm.TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.Message, Input.Text));

				Input.Text = "";

				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		#region Threading
		private static void ListenThread(ObjectNetworkStream ListenStream, ChatForm CurrentForm)
		{
			while (true)
			{
				try
				{
					Byte[] ByteBuffer = new Byte[1024];

					ResponsePacket Received = (ResponsePacket)MainForm._bFormatter.Deserialize(ListenStream);

					switch (Received.Response)
					{
						case NetworkReponse.ResponseCodes.MessageSend:
							CurrentForm.Output.Invoke((MethodInvoker)delegate { CurrentForm.Output.AppendText($"<{DateTime.Now.ToString("HH:mm")}> {Received.ResponseString}" + Environment.NewLine); });
							break;
						//Else
						default:
							break;
					}
				}
				catch (Exception ex)
				{
					if (ex is System.IO.IOException)
					{
						CurrentForm.Output.Invoke((MethodInvoker)delegate { CurrentForm.Output.AppendText($"({DateTime.Now}) || Connection Ended" + Environment.NewLine); });
						ListenStream.Close();
						return;
					}
#if DEBUG
					else if (ex is System.ArgumentNullException)
					{
						CurrentForm.Output.Invoke((MethodInvoker)delegate { CurrentForm.Output.AppendText($"({DateTime.Now}) || Testing Env" + Environment.NewLine); });
						return;
					}
#endif
					else
					{
						throw ex;
					}
				}

			}
		}
		#endregion
	}
}
