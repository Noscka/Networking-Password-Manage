using Networking.CustomNetObjects;
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
		private static extern bool HideCaret(IntPtr hWnd);

		[DllImport("user32.dll")]
		private static extern bool ShowCaret(IntPtr hWnd);

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

		private void SlidePictureBox_Click(Object sender, EventArgs e)
		{
			Image img = SlidePictureBox.Image;
			img.RotateFlip(RotateFlipType.Rotate180FlipNone);
			SlidePictureBox.Image = img;

			//ChatOptionFormInstance.Controls.SetChildIndex(ChatOptionFormInstance, this.Controls.GetChildIndex(this)-1);

			ChatOptionFormInstance.FormSlide(SlideSettingsOut);
			SlideSettingsOut = !SlideSettingsOut;
		}
		#endregion

		#region Global Variables
		public static MainForm FormParent = null;
		public ChatOptionsForm ChatOptionFormInstance;

		private bool SlideSettingsOut = false;
		#endregion

		private void ChatForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}
		public void WriteToConsole(String Message)
		{
			Panel MessageContainerPanel = new Panel();
			Label MessageLabel = new Label();

			MessageLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
			MessageLabel.ForeColor = Color.FromArgb(0, 192, 0);
			MessageLabel.Location = new Point(40, 0);
			MessageLabel.Text = Message;

			MessageContainerPanel.Size = new Size(780, 80);
			MessageContainerPanel.Controls.Add(MessageLabel);

			NewOutput.Controls.Add(MessageContainerPanel);

			//this.Output.AppendText(Message + Environment.NewLine);
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
			Thread TCPListenThread = new Thread(() => ListenThread(MainForm.SSLTCPNetworkStream, this));
			MainForm.GlobalThreadList.Add(TCPListenThread);
			TCPListenThread.Name = "Listen Thread";
			TCPListenThread.IsBackground = true;
			TCPListenThread.Start();

			DockingForm InfoDockingForm = new DockingForm(this);
			InfoDockingForm.Left = this.Left + this.Width + 10;
			InfoDockingForm.Top = this.Top + (this.Height - InfoDockingForm.Height) / 2;
			InfoDockingForm.Show(this);

			ChatOptionFormInstance = new ChatOptionsForm(this);
			//ChatOptionFormInstance.Show(this);
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
				MainForm.SSLTCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.Message, Input.Text));

				Input.Text = "";

				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		#region Threading
		private static void ListenThread(ObjectSSLStream ListenStream, ChatForm CurrentForm)
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
							Panel MessageContainerPanel = new Panel();
							PictureBox ProfilePictureBox = new PictureBox();
							Label UsernameLabel = new Label();
							Label MessageLabel = new Label();

							// Profile Picture Box
							ProfilePictureBox.Size = new Size(50, 50);
							System.Drawing.Drawing2D.GraphicsPath gp = new System.Drawing.Drawing2D.GraphicsPath();
							gp.AddEllipse(0, 0, ProfilePictureBox.Width - 3, ProfilePictureBox.Height - 3);
							Region rg = new Region(gp);
							ProfilePictureBox.Region = rg;
							ProfilePictureBox.Image = Image.FromFile(@"D:\Users\Adam\Downloads\MEMORIEs\DSC04182.JPG");
							ProfilePictureBox.SizeMode = PictureBoxSizeMode.Zoom;

							//Username Label
							UsernameLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
							UsernameLabel.ForeColor = Color.FromArgb(0, 192, 0);
							UsernameLabel.Text = Received.MessageObject.Username;
							UsernameLabel.Location = new Point(50, 0);

							//Message Label
							MessageLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
							MessageLabel.ForeColor = Color.FromArgb(0, 192, 0);
							MessageLabel.Text = Received.MessageObject.Message;
							MessageLabel.Location = new Point(50, 30);

							//Message Container Panel
							MessageContainerPanel.Size = new Size(780, 80);
							MessageContainerPanel.Controls.Add(ProfilePictureBox);
							MessageContainerPanel.Controls.Add(UsernameLabel);
							MessageContainerPanel.Controls.Add(MessageLabel);

							CurrentForm.NewOutput.Invoke((MethodInvoker)delegate { CurrentForm.NewOutput.Controls.Add(MessageContainerPanel); });
							break;
						//Else
						default:
							break;
					}
				}
				catch (Exception ex)
				{
					Panel MessageContainerPanel = new Panel();
					Label MessageLabel = new Label();

					MessageLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
					MessageLabel.ForeColor = Color.FromArgb(0, 192, 0);
					MessageLabel.Location = new Point(40, 0);

					MessageContainerPanel.Size = new Size(780, 80);
					MessageContainerPanel.Controls.Add(MessageLabel);

					if (ex is System.IO.IOException)
					{
						MessageLabel.Text = "Connection Ended";
						//CurrentForm.Output.Invoke((MethodInvoker)delegate { CurrentForm.Output.AppendText($"({DateTime.Now}) || Connection Ended" + Environment.NewLine); });
						CurrentForm.NewOutput.Invoke((MethodInvoker)delegate { CurrentForm.NewOutput.Controls.Add(MessageContainerPanel); });
						ListenStream.Close();
						return;
					}
#if DEBUG
					else if (ex is System.ArgumentNullException)
					{
						MessageLabel.Text = "Testing Env";
						//CurrentForm.Output.Invoke((MethodInvoker)delegate { CurrentForm.Output.AppendText($"({DateTime.Now}) || Testing Env" + Environment.NewLine); });
						CurrentForm.NewOutput.Invoke((MethodInvoker)delegate { CurrentForm.NewOutput.Controls.Add(MessageContainerPanel); });
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
