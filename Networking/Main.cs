using System;
using System.Net.Sockets;
using System.IO;
using Networking.ObjectStream;
using Networking.Packets;
using Networking.TCP;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Net.Security;
using System.Security.Authentication;

namespace Networking.Packets
{
	public enum NetworkOperationTypes : ushort
	{
		SignIn = 0,
		SignUp = 1,
		LogOut = 2,
		Message = 3,
	}

	public static class NetworkReponse
	{
		public enum ResponseCodes : ushort
		{
			successful = 0,
			WrongPass = 1,
			NotFound = 2,
			UserExists = 3,
			AlreadyLogged = 4,
			MessageSend = 5,
		}
	}

	[Serializable]
	public class Packet
	{
		public Packet() { }

		public virtual String PacketType { get; } = "Packet";

	}

	[Serializable]
	public class RequestPacket : Packet
	{
		public RequestPacket() { }
		public RequestPacket(NetworkOperationTypes opType)
		{
			OpType = opType;
		}
		public RequestPacket(NetworkOperationTypes opType, String message)
		{
			OpType = opType;
			Message = message;
		}
		public RequestPacket(NetworkOperationTypes opType, String username, String password)
		{
			OpType = opType;
			Username = username;
			Password = password;
		}

		public override String PacketType { get; } = "SignPacket";

		public NetworkOperationTypes OpType { get; set; }

		public String Username { get; set; }

		public String Password { set; get; }

		public String Message { set; get; }
	}

	[Serializable]
	public class ResponsePacket : Packet
	{
		public ResponsePacket() { }

		public ResponsePacket(NetworkReponse.ResponseCodes response, NetworkOperationTypes ResponseOp)
		{
			Response = response;
			ReponseOperation = ResponseOp;
		}

		public ResponsePacket(NetworkReponse.ResponseCodes response, String message)
		{
			Response = response;
			ResponseString = message;
		}

		public ResponsePacket(NetworkReponse.ResponseCodes response, NetworkOperationTypes ResponseOp, String SentText)
		{
			Response = response;
			ReponseOperation = ResponseOp;
			ResponseString = SentText;
		}



		public override String PacketType { get; } = "ResponsePacket";

		public NetworkReponse.ResponseCodes Response { get; set; }
		public NetworkOperationTypes ReponseOperation { get; set; }

		public String ResponseString { get; set; }
	}
}

namespace Networking.ObjectStream
{
	public class ObjectNetworkStream : NetworkStream
	{
		private readonly BinaryFormatter _bFormatter = new BinaryFormatter();

		public ObjectNetworkStream(Socket socket) : base(socket)
		{
		}
		public ObjectNetworkStream(Socket socket, bool ownsSocket) : base(socket, ownsSocket)
		{
		}
		public ObjectNetworkStream(Socket socket, FileAccess access) : base(socket, access) 
		{
		}
		public ObjectNetworkStream(Socket socket, FileAccess access, bool ownsSocket) : base(socket, access, ownsSocket)
		{
		}

		/// <summary>
		/// Send Object over network
		/// </summary>
		/// <param name="ObjectToSend">Send Object Over Network</param>
		public void Write(Object ObjectToSend)
		{
			_bFormatter.Serialize(this, ObjectToSend);
		}
	}
}

namespace Networking.TCP
{
	public class ObjectTcpClient : TcpClient
	{
		private ObjectNetworkStream ObjStream;

		public ObjectTcpClient() { }
		public ObjectTcpClient(IPEndPoint localEP) : base(localEP) { }
		public ObjectTcpClient(AddressFamily family) : base(family) { }
		public ObjectTcpClient(string hostname, int port) : base(hostname, port) { }

		internal ObjectTcpClient(Socket acceptedSocket)
		{
			Client = acceptedSocket;
		}

		public new ObjectNetworkStream GetStream()
		{
			if (!Client.Connected)
			{
				throw new InvalidOperationException("net_notconnected");
			}

			if (ObjStream == null)
			{
				ObjStream = new ObjectNetworkStream(Client, ownsSocket: true);
			}

			return ObjStream;
		}
	}

	public class ObjectTcpListener : TcpListener
	{
		public ObjectTcpListener(IPEndPoint localEP) : base(localEP) { }
		public ObjectTcpListener(int port) : base(port) { }
		public ObjectTcpListener(IPAddress localaddr, int port) : base(localaddr, port) { }

		public new ObjectTcpClient AcceptTcpClient()
		{
			if (!this.Active)
			{
				throw new InvalidOperationException("net_stopped");
			}

			Socket acceptedSocket = this.Server.Accept();
			ObjectTcpClient tcpClient = new ObjectTcpClient(acceptedSocket);

			return tcpClient;
		}
	}
}

namespace HexDump
{
	class Utils
	{
		public static string HexDump(byte[] bytes, int bytesPerLine = 16)
		{
			if (bytes == null) return "<null>";
			int bytesLength = bytes.Length;

			char[] HexChars = "0123456789ABCDEF".ToCharArray();

			int firstHexColumn =
				  8                   // 8 characters for the address
				+ 3;                  // 3 spaces

			int firstCharColumn = firstHexColumn
				+ bytesPerLine * 3       // - 2 digit for the hexadecimal value and 1 space
				+ (bytesPerLine - 1) / 8 // - 1 extra space every 8 characters from the 9th
				+ 2;                  // 2 spaces 

			int lineLength = firstCharColumn
				+ bytesPerLine           // - characters to show the ascii value
				+ Environment.NewLine.Length; // Carriage return and line feed (should normally be 2)

			char[] line = (new String(' ', lineLength - 2) + Environment.NewLine).ToCharArray();
			int expectedLines = (bytesLength + bytesPerLine - 1) / bytesPerLine;
			StringBuilder result = new StringBuilder(expectedLines * lineLength);

			for (int i = 0; i < bytesLength; i += bytesPerLine)
			{
				line[0] = HexChars[(i >> 28) & 0xF];
				line[1] = HexChars[(i >> 24) & 0xF];
				line[2] = HexChars[(i >> 20) & 0xF];
				line[3] = HexChars[(i >> 16) & 0xF];
				line[4] = HexChars[(i >> 12) & 0xF];
				line[5] = HexChars[(i >> 8) & 0xF];
				line[6] = HexChars[(i >> 4) & 0xF];
				line[7] = HexChars[(i >> 0) & 0xF];

				int hexColumn = firstHexColumn;
				int charColumn = firstCharColumn;

				for (int j = 0; j < bytesPerLine; j++)
				{
					if (j > 0 && (j & 7) == 0) hexColumn++;
					if (i + j >= bytesLength)
					{
						line[hexColumn] = ' ';
						line[hexColumn + 1] = ' ';
						line[charColumn] = ' ';
					}
					else
					{
						byte b = bytes[i + j];
						line[hexColumn] = HexChars[(b >> 4) & 0xF];
						line[hexColumn + 1] = HexChars[b & 0xF];
						line[charColumn] = asciiSymbol(b);
					}
					hexColumn += 3;
					charColumn++;
				}
				result.Append(line);
			}
			return result.ToString();
		}
		static char asciiSymbol(byte val)
		{
			if (val < 32) return '.';  // Non-printable ASCII
			if (val < 127) return (char)val;   // Normal ASCII
											   // Handle the hole in Latin-1
			if (val == 127) return '.';
			if (val < 0x90) return "€.‚ƒ„…†‡ˆ‰Š‹Œ.Ž."[val & 0xF];
			if (val < 0xA0) return ".‘’“”•–—˜™š›œ.žŸ"[val & 0xF];
			if (val == 0xAD) return '.';   // Soft hyphen: this symbol is zero-width even in monospace fonts
			return (char)val;   // Normal Latin-1
		}
	}
}
