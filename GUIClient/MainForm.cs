using Networking.CustomNetObjects;
using Networking.Packets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Security;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIClient
{
	public partial class MainForm : Form
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

		/// <summary>
		/// If user wants to SignIn (true) or SignUp (False)
		/// </summary>
		public static bool SignIn = false;

		/// <summary>
		/// if is connected to server
		/// </summary>
		public static bool connected = false;

		/// <summary>
		/// if should go to next form
		/// </summary>
		private static bool NextForm = false;

		/// <summary>
		/// Global and main ObjectTCPClient
		/// </summary>
		public static ObjectTcpClient TCPClient { get; set; } = new ObjectTcpClient();

		/// <summary>
		///  Global Network Stream
		/// </summary>
		public static ObjectSSLStream SSLTCPNetworkStream { get; set; }

		/// <summary>
		/// Global User Information
		/// </summary>
		public static UserInformationPack CurrentUser { get; set; } = new UserInformationPack("Remove 108 MainForm.cs");

		/// <summary>
		/// Binary Formatter for objects
		/// </summary>
		public static BinaryFormatter _bFormatter = new BinaryFormatter();

		/// <summary>
		/// public instance for all forms to access
		/// </summary>
		public static MainForm StartFormInstance = null;

		/// <summary>
		/// List of all threads that need to aborted on soft restart
		/// </summary>
		public static List<Thread> GlobalThreadList = new List<Thread>();
		#endregion

		private void MainForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}

		public MainForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			StartFormInstance = this;

			//Create second thread to connect to server so it doesn't freeze the gui
			Thread TPCConnectionThread = new Thread(() => ConnectToServer(this));
			TPCConnectionThread.Name = "Server Connection Thread";
			TPCConnectionThread.IsBackground = true;
			TPCConnectionThread.Start();

			//ChatForm ChatForm = new ChatForm(this);
			//ChatForm.Show();
			//this.Hide();
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

		private void UsernameTextBox_TextChanged(object sender, EventArgs e)
		{
			EnableDoneButtonOn();
		}

		private void PasswordTextBox_TextChanged(object sender, EventArgs e)
		{
			EnableDoneButtonOn();
		}

		/// <summary>
		/// Validate if all conditions are met
		/// </summary>
		/// <returns>True if conditions are met, false if not</returns>
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

		/// <summary>
		/// async function so program waits for Sign process is complete
		/// </summary>
		private async void AwaitThread()
		{
			await Task.Run(() => LoginSign(this, UsernameTextBox.Text, PasswordTextBox.Text));
			if (NextForm)
			{
				ChatForm ChatForm = new ChatForm(this);
				ChatForm.Show(this);
				this.Hide();
			}
		}

		private void UsernameTextBox_KeyDown(Object sender, KeyEventArgs e)
		{
			// if enter is done and all conditions are met, do as if Done Button was clicked
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
			// if enter is done and all conditions are met, do as if Done Button was clicked
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

		/// <summary>
		/// Function for sending and receiving logging/sign up details to server
		/// </summary>
		/// <param name="CurrentForm">this form</param>
		/// <param name="Username">Wanted Username</param>
		/// <param name="Password">Wanted Password</param>
		private static void LoginSign(MainForm CurrentForm, String Username, String Password)
		{
			CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Visible = true; });
			ResponsePacket Received;

			// if user wants to log in
			if (SignIn)
			{
				CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Logging In\nMight take a sec"; });

				// write object to server
				SSLTCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.SignIn, Username, Password));
				Received = (ResponsePacket)_bFormatter.Deserialize(SSLTCPNetworkStream);
				switch (Received.Response)
				{
					case NetworkReponse.ResponseCodes.successful:
						NextForm = true;
						CurrentUser = Received.currentUser;
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
			// if user wants to sign up
			else
			{
				CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Creating Account\nMight take a sec"; });

				//write object to server
				SSLTCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.SignUp, Username, Password));

				// get response back
				Received = (ResponsePacket)_bFormatter.Deserialize(SSLTCPNetworkStream);

				// switch for response codes
				switch (Received.Response)
				{
					case NetworkReponse.ResponseCodes.successful:
						NextForm = true;
						CurrentUser = Received.currentUser;
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "Account Created"; });
						break;
					case NetworkReponse.ResponseCodes.UserExists:
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = "User Already Exists"; });
						break;
				}
			}

			// if responses is user being already logged in, goes for both
			if (Received.Response == NetworkReponse.ResponseCodes.AlreadyLogged)
			{
				CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate { CurrentForm.ErrorInfoLogin.Text = Received.ResponseString; });
			}

			return;
		}


		/// <summary>
		/// Thread for connecting to server |
		/// made as thread to not hold up gui thread
		/// </summary>
		private static void ConnectToServer(MainForm CurrentForm)
		{
			while (true)
			{
				try
				{
					// Try to connect to server
					TCPClient.Connect(Dns.GetHostAddresses("192.168.1.109"), 6096);
					SSLTCPNetworkStream = new ObjectSSLStream(TCPClient.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);
					SSLTCPNetworkStream.AuthenticateAsClient("SERVERWAR");

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
					else if (ex is System.Security.Authentication.AuthenticationException)
					{
						CurrentForm.ErrorInfoLogin.Invoke((MethodInvoker)delegate
						{
							CurrentForm.ErrorInfoLogin.Text = "Server Authentication Failed\nTrying again 10 seconds";
							CurrentForm.ErrorInfoLogin.Visible = true;
						});
						Thread.Sleep(10000);
					}
					else
					{
						// throw incase new unaccounted error
						throw ex;
					}
				}
			}
		}
		#endregion

		#region Functions
		public void SoftRestart(SRReasons.srReasons RestartReason)
		{
			switch (RestartReason)
			{
				case SRReasons.srReasons.Error:
					break;

				case SRReasons.srReasons.LogOut:
					MainForm.SSLTCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.LogOut));
					break;
			}

			NextForm = false;

			for (int i = GlobalThreadList.Count - 1; i >= 0; i--)
			{
				try
				{
					GlobalThreadList[i].Abort();
					GlobalThreadList.RemoveAt(i);
				}
				catch { }
			}

			FormCollection fc = Application.OpenForms;

			for (int i = fc.Count - 1; i >= 0; i--)
			{
				if (fc[i] != this)
				{
					fc[i].Dispose();
				}
			}

			this.Show();
		}

		public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return true;
			}

			Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

			// refuse connection
			return false;
		}
		#endregion
	}
}

public static class SRReasons
{
	public enum srReasons : UInt16
	{
		Error = 0,
		LogOut = 1,
	}

}
