using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Networking.TCP;
using Networking.ObjectStream;
using System.Runtime.Serialization.Formatters.Binary;
using Networking.Packets;
using System.Collections;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace ServerTest
{
	internal class Program
	{
		private static BinaryFormatter _bFormatter = new BinaryFormatter();

		#region Globalvariables
		// Server IP:port set up and Listener
		public static IPEndPoint ServerIpEndPoint = new IPEndPoint(IPAddress.Loopback, 6096);
		public static ObjectTcpListener ServerTCPListener = new ObjectTcpListener(ServerIpEndPoint);
		static X509Certificate2 serverCertificate = null;
		#endregion

		static void Main(string[] args)
		{
			serverCertificate = new X509Certificate2("cert.cer");

			ServerTCPListener.Start();

			Console.WriteLine(ConsoleLog($"Server Started and Listening on <{ServerTCPListener.LocalEndpoint}>"));

			while (true)
			{
				ObjectTcpClient InboundClient = ServerTCPListener.AcceptTcpClient(); // wait for inbound connect, give them custom object

				Console.WriteLine(ConsoleLog($"Client Connected // \"{InboundClient.Client.RemoteEndPoint}\""));

				// new thread for the client
				Thread TCPClientThread = new Thread(() => HandleNewTCPClient(InboundClient));
				TCPClientThread.Start();
			}
		}

		public static String ConsoleLog(String inputMessage)
		{
			return $"({DateTime.Now}) || {inputMessage}";
		}

		static void HandleNewTCPClient(ObjectTcpClient Client)
		{
			SslStream sslStream = null;

			try
			{
				sslStream = new SslStream(Client.GetStream(), false);
				sslStream.AuthenticateAsServer(serverCertificate, false, false);


				Byte[] ByteBuffer = new Byte[1024];

				while (true)
				{
					Console.WriteLine(Encoding.Unicode.GetString(ByteBuffer, 0, sslStream.Read(ByteBuffer, 0, ByteBuffer.Length)));


					//RequestPacket Received = (RequestPacket)_bFormatter.Deserialize(stream);

					ByteBuffer = new Byte[1024];
				}
			}
			catch (System.IO.IOException)
			{
				Console.WriteLine(ConsoleLog("Connection Ended"));
				sslStream.Close();
				return;
			}
		}
	}
}
