using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUIClient
{
	public partial class ChatOptionsForm : Form
	{
		ChatForm FormParent;

		public ChatOptionsForm(ChatForm formParent)
		{
			FormParent = formParent;
			InitializeComponent();
		}

		private void ChatOptionsForm_FormClosing(Object sender, FormClosingEventArgs e)
		{
			Environment.Exit(0);
		}
	}
}
