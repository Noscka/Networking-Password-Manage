using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Networking.ObjectStream;
using Networking.Packets;
using Networking.TCP;
using System.Runtime.Serialization.Formatters.Binary;

namespace DOSClient
{
	internal class Program
	{
		public static ObjectTcpClient tcpClient = new ObjectTcpClient();
		public static ObjectNetworkStream stream;
		private static BinaryFormatter _bFormatter = new BinaryFormatter();

		static void Main(string[] args)
		{
			tcpClient.Connect(Dns.GetHostAddresses("192.168.1.109"), 6096);
			stream = tcpClient.GetStream();

			Console.Write("Input (1|2): ");
			ushort cunt = ushort.Parse(Console.ReadLine());
			while (true)
			{
				Console.Write("Input Username: ");
				String Username = Console.ReadLine();

				Console.Write("Input Password: ");
				String Password = Console.ReadLine();

				stream.Write(new RequestPacket((NetworkOperationTypes)cunt, Username, Password));

				ResponsePacket Received = (ResponsePacket)_bFormatter.Deserialize(stream);

				switch (Received.Response)
				{
					case NetworkReponse.ResponseCodes.successful:
						Console.WriteLine("successful");
						goto End;

					case NetworkReponse.ResponseCodes.WrongPass:
						Console.WriteLine("WrongPass");
						break;

					case NetworkReponse.ResponseCodes.NotFound:
						Console.WriteLine("NotFound");
						break;

					case NetworkReponse.ResponseCodes.UserExists:
						Console.WriteLine("UserExists");
						break;
				}
			}


		End:
			Console.WriteLine("End");
		}
	}
}
