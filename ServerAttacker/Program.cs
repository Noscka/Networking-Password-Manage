using Networking.CustomNetObjects;
using Networking.Packets;
using System;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace ServerAttacker
{
	internal class Program
	{
		private static readonly BinaryFormatter _bFormatter = new BinaryFormatter();
		static void Main(string[] args)
		{
			ObjectTcpClient TCPClient = new ObjectTcpClient();
			TCPClient.Connect(Dns.GetHostAddresses(IPAddress.Loopback.ToString()), 6096);
			ObjectNetworkStream TCPNetworkStream = TCPClient.GetStream();

			//TCPNetworkStream.Write(Encoding.UTF8.GetBytes("abbc"),0, Encoding.UTF8.GetBytes("abbc").Length); 

			TCPNetworkStream.Write(new RequestPacket(NetworkOperationTypes.SignUp, new String('k', 10 ^ 10020229), new String('k', 10 ^ 10020229)));

			ResponsePacket response = (ResponsePacket)_bFormatter.Deserialize(TCPNetworkStream);

			Console.WriteLine($@"=
        Response:   {response.Response}
      EntryField:   {response.EntryField}
ReponseOperation:   {response.ReponseOperation}
  ResponseString:   {response.ResponseString}
     currentUser:   {response.currentUser}
=");

			while (true) { }
		}
	}
}
