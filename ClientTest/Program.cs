using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using Networking.ObjectStream;
using Networking.TCP;
using Networking.Packets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.IO;


namespace ClientTest
{
	internal class Program
	{
		private static Hashtable certificateErrors = new Hashtable();

		private static BinaryFormatter _bFormatter = new BinaryFormatter();

		// The following method is invoked by the RemoteCertificateValidationDelegate.
		public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
				return true;

			Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

			// Do not allow this client to communicate with unauthenticated servers.
			return false;
		}

		static void Main(string[] args)
		{
			ObjectTcpClient tcpClient = new ObjectTcpClient("127.0.0.1", 6096);

			SslStream stream = new SslStream(tcpClient.GetStream(), false,new RemoteCertificateValidationCallback(ValidateServerCertificate),null);

			stream.AuthenticateAsClient("Asshat");

			while (true)
			{
				//stream.Write(new RequestPacket(NetworkOperationTypes.SignUp, "cunt", "bitch"));

				Byte[] Send = Encoding.Unicode.GetBytes("cunt someshit");

				stream.Write(Send, 0, Send.Length);

				Console.ReadLine();
			}
		}
	}
}