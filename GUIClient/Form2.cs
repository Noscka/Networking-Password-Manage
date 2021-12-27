using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Networking.ObjectStream;
using Networking.Packets;
using Networking.TCP;


namespace GUIClient
{
	public partial class Form2 : Form
	{
		[DllImport("user32.dll")]
		static extern bool HideCaret(IntPtr hWnd);

		[DllImport("user32.dll")]
		static extern bool ShowCaret(IntPtr hWnd);

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
		public Form1 FormParent = null;
		#endregion

		public Form2(Form1 Parent)
		{
			FormParent = Parent;
			InitializeComponent();
		}
		private void Output_GotFocus(Object sender, EventArgs e)
		{
			HideCaret(((TextBox)sender).Handle);
		}

		private void Input_Enter(Object sender, EventArgs e)
		{
			if(Input.ForeColor == Color.Gray)
			{
				Input.Text = "";
				Input.ForeColor = Color.FromArgb(0, 192, 0);
			}
		}

		private void Input_Leave(Object sender, EventArgs e)
		{
			if(Input.Text.Length == 0)
			{
				Input.Text = "Send to everyone...";
				Input.ForeColor = Color.Gray;
			}
		}

		private void Input_KeyDown(Object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				Form1.TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.Message, Input.Text));

				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private static void ListenThread(ObjectNetworkStream ListenStream, Form2 CurrentForm)
		{
			while (true)
			{
				Byte[] ByteBuffer = new Byte[1024];

				ResponsePacket Received = (ResponsePacket)Form1._bFormatter.Deserialize(ListenStream);

				switch (Received.Response)
				{
					case NetworkReponse.ResponseCodes.MessageSend:
						CurrentForm.Output.Invoke((MethodInvoker)delegate { CurrentForm.Output.Text += $"({DateTime.Now}) || {Received.ResponseString}\n"; });
						break;
					//Else
					default:
						break;
				}
			}
		}
	}
}
