using Networking.CustomNetObjects;
using Networking.Packets;
using System;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace ServerAttacker
{
	internal class Program
	{
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

		private static readonly BinaryFormatter _bFormatter = new BinaryFormatter();
		static void Main(string[] args)
		{
			ObjectTcpClient tcpClient = new ObjectTcpClient();
			tcpClient.Connect(Dns.GetHostAddresses(IPAddress.Loopback.ToString()), 6096);
			ObjectSSLStream sslStream = new ObjectSSLStream(tcpClient.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate), null);

			sslStream.AuthenticateAsClient("SERVERWAR");

			//TCPNetworkStream.Write(Encoding.UTF8.GetBytes("abbc"),0, Encoding.UTF8.GetBytes("abbc").Length); 

			sslStream.Write(new RequestPacket(NetworkOperationTypes.SignIn, "ggg", "ggg"));

			ResponsePacket response = (ResponsePacket)_bFormatter.Deserialize(sslStream);

			Console.WriteLine($@"=
        Response:   {response.Response}
      EntryField:   {response.EntryField}
ReponseOperation:   {response.ReponseOperation}
  ResponseString:   {response.ResponseString}
     currentUser:   {response.currentUser}
CurrentUser Username :	{(response.currentUser is null ? "" : response.currentUser.Username)}
=");

			while (true)
			{
				sslStream.Write(new RequestPacket(NetworkOperationTypes.Message, "ggg"));
				Thread.Sleep(10);
			}
		}
	}
}
