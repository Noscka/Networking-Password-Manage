using Networking.ObjectStream;
using Networking.Packets;
using Networking.TCP;
using System;
using System.Drawing;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIClient
{
	public partial class Form1 : Form
	{
		#region TopBar
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
			this.Close();
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
		public static bool SignIn = false, connected = false, NextForm = false;
		public static ObjectTcpClient TCPClient { get; set; } = new ObjectTcpClient();
		public static ObjectNetworkStream TCPNetworkStream { get; set; }
		public static BinaryFormatter _bFormatter = new BinaryFormatter();
		#endregion

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			Thread TPCConnectionThread = new Thread(() => ConnectToServer(this));
			TPCConnectionThread.Start();

			Form2 ChatForm = new Form2(this);
			ChatForm.Show();
			this.Hide();

		}

		#region SignIn/Up logic
		private void SignInButton_Click(object sender, EventArgs e)
		{
			SignInUpPanel.Visible = true;
			SignType.Text = "Sign In";
			SignIn = true;
		}

		private void SignUpButton_Click(object sender, EventArgs e)
		{
			SignInUpPanel.Visible = true;
			SignType.Text = "Sign Up";
			SignIn = false;
		}
		private void SignUpButton_DoubleClick(object sender, EventArgs e)
		{
			TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.LogOut));
		}
		private void UsernameTextBox_TextChanged(object sender, EventArgs e)
		{
			EnableDoneButtonOn();
		}

		private void PasswordTextBox_TextChanged(object sender, EventArgs e)
		{
			EnableDoneButtonOn();
		}

		private bool EnableDoneButtonOn()
		{
			if (UsernameTextBox.TextLength > 0 && PasswordTextBox.TextLength > 0 && connected) { DoneButton.Enabled = true; return true; } else { DoneButton.Enabled = false; return false; }
		}
		#endregion

		private void DoneButton_Click(object sender, EventArgs e)
		{
			AwaitThread();
		}

		#region Threading

		private async void AwaitThread()
		{
			await Task.Run(() => LoginSign(this, UsernameTextBox.Text, PasswordTextBox.Text));
			if (NextForm)
			{
				Form2 ChatForm = new Form2(this);
				ChatForm.Show();
				this.Hide();
			}
		}

		private void UsernameTextBox_KeyDown(Object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (EnableDoneButtonOn())
				{
					DoneButton_Click(this, new EventArgs());

					e.Handled = true;
					e.SuppressKeyPress = true;
				}
			}
		}

		private void PasswordTextBox_KeyDown(Object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				if (EnableDoneButtonOn())
				{
					DoneButton_Click(this, new EventArgs());

					e.Handled = true;
					e.SuppressKeyPress = true;
				}
			}
		}

		private static void LoginSign(Form1 CurrentForm, String Username, String Password)
		{
			CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Visible = true; });
			ResponsePacket Received;
			if (SignIn)
			{
				CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Logging In\nMight take a sec"; });

				TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.SignIn, Username, Password));
				Received = (ResponsePacket)_bFormatter.Deserialize(TCPNetworkStream);
				switch (Received.Response)
				{
					case NetworkReponse.ResponseCodes.successful:
						NextForm = true;
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Logged In"; });
						break;
					case NetworkReponse.ResponseCodes.WrongPass:
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Wrong Password"; });
						break;
					case NetworkReponse.ResponseCodes.NotFound:
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "User Not Found"; });
						break;
				}
			}
			else
			{
				CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Creating Account\nMight take a sec"; });

				TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.SignUp, Username, Password));
				Received = (ResponsePacket)_bFormatter.Deserialize(TCPNetworkStream);
				switch (Received.Response)
				{
					case NetworkReponse.ResponseCodes.successful:
						NextForm = true;
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Account Created"; });
						break;
					case NetworkReponse.ResponseCodes.UserExists:
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "User Already Exists"; });
						break;
				}
			}

			if (Received.Response == NetworkReponse.ResponseCodes.AlreadyLogged)
			{
				CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = Received.ResponseString; });
			}

			CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Visible = true; });
			return;
		}


		/// <summary>
		/// Thread for connecting to server |
		/// made as thread to not hold up gui thread
		/// </summary>
		private static void ConnectToServer(Form1 CurrentForm)
		{
			while (true)
			{
				try
				{
					// Try to connect to server
					TCPClient.Connect(Dns.GetHostAddresses("192.168.1.109"), 6096);
					TCPNetworkStream = TCPClient.GetStream();

					// if connects shows message saying it has for 2 seconds
					CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate
					{
						CurrentForm.ErrorInfoLogin.Text = "Connected To server";
						CurrentForm.ErrorInfoLogin.Visible = true;
					});

					Thread.Sleep(2000);

					// dissappear message
					CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)(() => CurrentForm.ErrorInfoLogin.Visible = false));
					connected = true;
					CurrentForm.EnableDoneButtonOn();
					return;
				}
				// if any error
				catch (Exception ex)
				{
					// if connection errors, show error and retry after 2 seconds
					if (ex is System.IO.IOException || ex is System.Net.Sockets.SocketException)
					{
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate
						{
							CurrentForm.ErrorInfoLogin.Text = "Cannot connect to server\nTrying again 2 seconds";
							CurrentForm.ErrorInfoLogin.Visible = true;
						});
						Thread.Sleep(2000);
					}
					else
					{
						// throw incase new unaccounted error
						throw ex;
					}
				}
			}
		}

	}
	#endregion
}
