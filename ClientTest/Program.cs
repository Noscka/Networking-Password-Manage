using Networking.CustomNetObjects;
using System;
using System.Collections;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace ClientTest
{
	internal class Program
	{
		private static BinaryFormatter _bFormatter = new BinaryFormatter();

		// The following method is invoked by the RemoteCertificateValidationDelegate.
		public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain,SslPolicyErrors sslPolicyErrors)
		{
			if (sslPolicyErrors == SslPolicyErrors.None)
			{
				return true;
			}

			Console.WriteLine("Certificate error: {0}", sslPolicyErrors);

			// refuse connection
			return false;
		}

		static void Main(string[] args)
		{
			ObjectTcpClient tcpClient = new ObjectTcpClient(IPAddress.Loopback.ToString(), 6096);

			Console.WriteLine("Client Connected");

			SslStream sslStream = new SslStream(tcpClient.GetStream(),false,new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

			sslStream.AuthenticateAsClient("SERVERWAR");

			while (true)
			{
				//sslStream.Write(new RequestPacket(NetworkOperationTypes.SignUp, "cunt", "bitch"));

				Byte[] Send = Encoding.ASCII.GetBytes("gdsgds");

				sslStream.Write(Send, 0, Send.Length);
				sslStream.Flush();

				Console.ReadLine();
			}
		}
	}
}