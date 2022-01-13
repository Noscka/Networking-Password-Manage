using Networking.CustomNetObjects;
using System;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace ServerTest
{
	internal class Program
	{
		private static BinaryFormatter _bFormatter = new BinaryFormatter();

		#region Globalvariables
		// Server IP:port set up and Listener
		public static ObjectTcpListener ServerTCPListener = new ObjectTcpListener(IPAddress.Any, 6096);
		static X509Certificate2 serverCertificate = null;
		#endregion

		static void Main(string[] args)
		{
			serverCertificate = new X509Certificate2("certificate.p12");

			ServerTCPListener.Start();

			Console.WriteLine(ConsoleLog($"Server Started and Listening on <{ServerTCPListener.LocalEndpoint}>"));

			Console.WriteLine($@"{serverCertificate.SubjectName.Name}");

			while (true)
			{
				ObjectTcpClient InboundClient = ServerTCPListener.AcceptTcpClient(); // wait for inbound connect, give them custom object

				Console.WriteLine(ConsoleLog($"Client Connected // \"{InboundClient.Client.RemoteEndPoint}\""));

				// new thread for the client
				Thread TCPClientThread = new Thread(() => HandleNewTCPClient(InboundClient));
				TCPClientThread.IsBackground = true;
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
				sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);

				//DisplaySecurityLevel(sslStream);
				//DisplaySecurityServices(sslStream);
				//DisplayCertificateInformation(sslStream);
				//DisplayStreamProperties(sslStream);

				Byte[] ByteBuffer = new Byte[1024];

				while (true)
				{
					Console.WriteLine(Encoding.ASCII.GetString(ByteBuffer, 0, sslStream.Read(ByteBuffer, 0, ByteBuffer.Length)));

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

		#region Debug Display
		static void DisplaySecurityLevel(SslStream stream)
		{
			Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
			Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
			Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
			Console.WriteLine("Protocol: {0}", stream.SslProtocol);
		}
		static void DisplaySecurityServices(SslStream stream)
		{
			Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
			Console.WriteLine("IsSigned: {0}", stream.IsSigned);
			Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
		}
		static void DisplayStreamProperties(SslStream stream)
		{
			Console.WriteLine("Can read: {0}, write {1}", stream.CanRead, stream.CanWrite);
			Console.WriteLine("Can timeout: {0}", stream.CanTimeout);
		}
		static void DisplayCertificateInformation(SslStream stream)
		{
			Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

			X509Certificate localCertificate = stream.LocalCertificate;
			if (stream.LocalCertificate != null)
			{
				Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
					localCertificate.Subject,
					localCertificate.GetEffectiveDateString(),
					localCertificate.GetExpirationDateString());
			}
			else
			{
				Console.WriteLine("Local certificate is null.");
			}
			// Display the properties of the client's certificate.
			X509Certificate remoteCertificate = stream.RemoteCertificate;
			if (stream.RemoteCertificate != null)
			{
				Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
					remoteCertificate.Subject,
					remoteCertificate.GetEffectiveDateString(),
					remoteCertificate.GetExpirationDateString());
			}
			else
			{
				Console.WriteLine("Remote certificate is null.");
			}
		}
		#endregion
	}
}
