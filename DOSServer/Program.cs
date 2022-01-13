using Networking.CustomNetObjects;
using Networking.Packets;
using Networking.CustomNetObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace DOSServer
{
	internal class Program
	{
		#region Const
		/// <summary>
		/// Path to user store
		/// </summary>
		private static readonly String UserDataStore = $@"{Directory.GetCurrentDirectory()}/Users.json";
		#endregion

		#region Globalvariables
		// Server IP:port set up and Listener
		public static IPEndPoint ServerIpEndPoint = new IPEndPoint(IPAddress.Any, 6096);
		public static ObjectTcpListener ServerTCPListener = new ObjectTcpListener(ServerIpEndPoint);
		private static readonly BinaryFormatter _bFormatter = new BinaryFormatter();
		#endregion

		static void Main(string[] args)
		{
			#region Read JSON
			CreateAndReadUserFile();
			#endregion

			#region Client Management
			ServerTCPListener.Start();

			Console.Title = $"Server {ServerTCPListener.LocalEndpoint} || Users: {User.TotalUsers}";
			Console.WriteLine(ConsoleLog($"Server Started and Listening on <{ServerTCPListener.LocalEndpoint}>"));

			while (true)
			{
				ObjectTcpClient InboundClient = ServerTCPListener.AcceptTcpClient(); // wait for inbound connect, give them custom object

				Console.WriteLine(ConsoleLog($"Client Connected // \"{InboundClient.Client.RemoteEndPoint}\""));

				User.TotalUsers++;
				UpdateTitle();

				// new thread for the client
				Thread TCPClientThread = new Thread(() => HandleNewTCPClient(InboundClient));
				TCPClientThread.IsBackground = true;
				TCPClientThread.Start();
			}
			#endregion
		}

		#region Threading
		private static void UDPListenThread()
		{
			while (true)
			{

			}
		}


		/// <summary>
		/// Function for Handling new Clients
		/// </summary>
		/// <param name="Client">Client Object</param>
		private static void HandleNewTCPClient(ObjectTcpClient Client)
		{
			User CurrentUser = new User();

			try
			{
				ObjectNetworkStream ClientNetworkStream = Client.GetStream();

			RestartLogging:
				bool ContinueSignProc = true;
				RequestPacket Received;

				// loops till currect user is set
				while (ContinueSignProc)
				{
					Received = (RequestPacket)_bFormatter.Deserialize(ClientNetworkStream);

					ResponsePacket response = new ResponsePacket();

					switch (Received.RequestedOperationType)
					{
						case NetworkOperationTypes.SignIn:

							response = new ResponsePacket(NetworkReponse.ResponseCodes.NotFound, NetworkOperationTypes.SignIn);

							foreach (User userInList in User.UserArray)
							{
								if (userInList.name == Received.Username)
								{
									if (PasswordHashing.ValidatePassword(Received.Password, userInList.password))
									{
										CurrentUser = userInList;
										CurrentUser.tcpClient = Client;
										response = new ResponsePacket(NetworkReponse.ResponseCodes.successful, NetworkOperationTypes.SignIn, new UserInformationPack(CurrentUser.name));
										Console.WriteLine(ConsoleLog($"{CurrentUser.name} Has logged in"));
										ContinueSignProc = false;
										break;
									}
									else
									{
										response = new ResponsePacket(NetworkReponse.ResponseCodes.WrongPass, NetworkOperationTypes.SignIn);
									}
								}
							}
							break;

						case NetworkOperationTypes.SignUp:

							bool found = false;

							if(String.IsNullOrEmpty(Received.Username) && String.IsNullOrEmpty(Received.Password))
                            {
								response = new ResponsePacket(NetworkReponse.ResponseCodes.NullOrEmpty, NetworkReponse.Field.both, NetworkOperationTypes.SignUp);
								break;
                            }
							else if (String.IsNullOrEmpty(Received.Username))
                            {
								response = new ResponsePacket(NetworkReponse.ResponseCodes.NullOrEmpty, NetworkReponse.Field.username, NetworkOperationTypes.SignUp);
								break;
							}
							else if (String.IsNullOrEmpty(Received.Password))
                            {
								response = new ResponsePacket(NetworkReponse.ResponseCodes.NullOrEmpty, NetworkReponse.Field.password, NetworkOperationTypes.SignUp);
								break;
							}
                            else
                            {
								foreach (User userInList in User.UserArray)
								{
									if (userInList.name == Received.Username)
									{
										response = new ResponsePacket(NetworkReponse.ResponseCodes.UserExists, NetworkOperationTypes.SignUp);
										found = true;
										break;
									}
								}

								if (!found)
								{
									CurrentUser = new User(Client, Received.Username, PasswordHashing.CreateHash(Received.Password));
									User.UserArray.Add(CurrentUser);
									response = new ResponsePacket(NetworkReponse.ResponseCodes.successful, NetworkOperationTypes.SignUp, new UserInformationPack(CurrentUser.name));
									Console.WriteLine(ConsoleLog($"New account Created: {CurrentUser.name}"));
									ContinueSignProc = false;

									#region Update Store
									File.WriteAllText(UserDataStore, JsonConvert.SerializeObject(User.UserArray));
									#endregion
								}
							}
							break;
					}
					ClientNetworkStream.Write(response);
				}


				while (true)
				{
					Received = (RequestPacket)_bFormatter.Deserialize(ClientNetworkStream);

					switch (Received.RequestedOperationType)
					{
						case NetworkOperationTypes.SignIn:
						case NetworkOperationTypes.SignUp:
							ClientNetworkStream.Write(new ResponsePacket(NetworkReponse.ResponseCodes.AlreadyLogged, Received.RequestedOperationType, $"Already Logged in as {CurrentUser.name}"));
							break;
						case NetworkOperationTypes.LogOut:
							Console.WriteLine(ConsoleLog($"{CurrentUser.name} Logged out"));
							goto RestartLogging;
						case NetworkOperationTypes.Message:

							User.SendAll($"\"{CurrentUser.name}\": {Received.Message}");

							Console.WriteLine(ConsoleLog($"\"{CurrentUser.name}\": {Received.Message}"));
							break;

					}
				}
			}
			catch (System.IO.IOException)
			{
				CurrentUser.tcpClient = null;
				Console.WriteLine(ConsoleLog("Connection Ended"));
				User.TotalUsers--;
				UpdateTitle();
				User.SendAll($"\"{CurrentUser.name}\" Left");
				return;
			}
		}
		#endregion

		/// <summary>
		/// Format String for console input
		/// </summary>
		/// <param name="inputMessage">Message to format</param>
		/// <returns>Message with timestamp at the beging</returns>
		public static String ConsoleLog(String inputMessage)
		{
			return $"({DateTime.Now}) || {inputMessage}";
		}

		/// <summary>
		/// Global Function to update the title with user count and ip
		/// </summary>
		public static void UpdateTitle()
		{
			Console.Title = $"Server {ServerTCPListener.LocalEndpoint} || Users: {User.TotalUsers}";
		}

		#region Async
		/// <summary>
		/// create user.json and then use it as the userarray
		/// </summary>
		private static async void CreateAndReadUserFile()
		{
			await WhenFileCreated(UserDataStore);
			User.UserArray = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(UserDataStore)) ?? new List<User>() { }; // if the deserialized file isn't null use it, if not then make a new list
		}

		public static Task WhenFileCreated(string path)
		{
			if (File.Exists(path))
				return Task.FromResult(true);

			var tcs = new TaskCompletionSource<bool>();
			FileSystemWatcher watcher = new FileSystemWatcher(Path.GetDirectoryName(path));

			FileSystemEventHandler createdHandler = null;
			RenamedEventHandler renamedHandler = null;
			createdHandler = (s, e) =>
			{
				if (e.Name == Path.GetFileName(path))
				{
					tcs.TrySetResult(true);
					watcher.Created -= createdHandler;
					watcher.Dispose();
				}
			};

			renamedHandler = (s, e) =>
			{
				if (e.Name == Path.GetFileName(path))
				{
					tcs.TrySetResult(true);
					watcher.Renamed -= renamedHandler;
					watcher.Dispose();
				}
			};

			watcher.Created += createdHandler;
			watcher.Renamed += renamedHandler;

			watcher.EnableRaisingEvents = true;

			return tcs.Task;
		}
		#endregion
	}

	/// <summary>
	/// Password Hashing class
	/// </summary>
	static class PasswordHashing
	{
		public const int SALT_BYTE_SIZE = 40;
		public const int HASH_BYTE_SIZE = 50;
		public const int PBKDF2_ITERATIONS = 1000000;

		public const int ITERATION_INDEX = 0;
		public const int SALT_INDEX = 1;
		public const int PBKDF2_INDEX = 2;

		/// <summary>
		/// Creates a salted PBKDF2 hash of the password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <returns>The hash of the password.</returns>
		public static string CreateHash(string password)
		{
			// Generate a random salt
			RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
			byte[] salt = new byte[SALT_BYTE_SIZE];
			csprng.GetBytes(salt);

			// Hash the password and encode the parameters
			byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
			return PBKDF2_ITERATIONS + ":" +
				Convert.ToBase64String(salt) + ":" +
				Convert.ToBase64String(hash);
		}

		/// <summary>
		/// Validates a password given a hash of the correct one.
		/// </summary>
		/// <param name="password">The password to check.</param>
		/// <param name="correctHash">A hash of the correct password.</param>
		/// <returns>True if the password is correct. False otherwise.</returns>
		public static bool ValidatePassword(string password, string correctHash)
		{
			// Extract the parameters from the hash
			char[] delimiter = { ':' };
			string[] split = correctHash.Split(delimiter);
			int iterations = Int32.Parse(split[ITERATION_INDEX]);
			byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
			byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

			byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
			return SlowEquals(hash, testHash);
		}

		/// <summary>
		/// Compares two byte arrays in length-constant time. This comparison
		/// method is used so that password hashes cannot be extracted from
		/// on-line systems using a timing attack and then attacked off-line.
		/// </summary>
		/// <param name="a">The first byte array.</param>
		/// <param name="b">The second byte array.</param>
		/// <returns>True if both byte arrays are equal. False otherwise.</returns>
		private static bool SlowEquals(byte[] a, byte[] b)
		{
			uint diff = (uint)a.Length ^ (uint)b.Length;
			for (int i = 0; i < a.Length && i < b.Length; i++)
				diff |= (uint)(a[i] ^ b[i]);
			return diff == 0;
		}

		/// <summary>
		/// Computes the PBKDF2-SHA1 hash of a password.
		/// </summary>
		/// <param name="password">The password to hash.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="iterations">The PBKDF2 iteration count.</param>
		/// <param name="outputBytes">The length of the hash to generate, in bytes.</param>
		/// <returns>A hash of the password.</returns>
		private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
		{
			Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
			pbkdf2.IterationCount = iterations;
			return pbkdf2.GetBytes(outputBytes);
		}
	}

	/// <summary>
	/// Local User object, CANNOT BE GLOBAL SO CLIENT DOESN'T GET PASSWORD
	/// </summary>
	[Serializable]
	class User
	{

		/// <summary>
		/// Total count of users
		/// </summary>
		public static int TotalUsers { get; set; }

		/// <summary>
		/// Array of all current Users
		/// </summary>
		public static List<User> UserArray = new List<User>() { };

		/// <summary>
		/// send to all function
		/// </summary>
		/// <param name="message">message to send</param>
		public static void SendAll(String message)
		{
			foreach (User user in UserArray) // go through all users
			{
				if (user.tcpClient != null) // only send if the tcpClient object instance isn't null
				{
					user.tcpClient.GetStream().Write(new ResponsePacket(NetworkReponse.ResponseCodes.MessageSend, message));
				}
			}
		}

		public String name { get; set; }
		public String password { get; set; }

		[JsonIgnore]
		public ObjectTcpClient tcpClient { get; set; }

		/// <summary>
		/// Empty User Object
		/// </summary>
		public User()
		{
		}

		/// <summary>
		/// Creating User with Name and Password
		/// </summary>
		/// <param name="Name">Username</param>
		/// <param name="Password">User Password</param>
		public User(String Name, String Password)
		{
			name = Name;
			password = Password;
		}

		/// <summary>
		/// Create username with Name and Password Plus TCPclient Instance
		/// </summary>
		/// <param name="TCPClient">TCPclient instance</param>
		/// <param name="Name">Username</param>
		/// <param name="Password">User Password</param>
		public User(ObjectTcpClient TCPClient, String Name, String Password)
		{
			tcpClient = TCPClient;
			name = Name;
			password = Password;
		}
	}
}
