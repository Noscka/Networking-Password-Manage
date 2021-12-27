using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SymmetricalEncryption
{
	class Program
	{
		static void Main(string[] args)
		{
			var key = "b14ca5898a4e4133bbce2ea2315a1916";

			//Console.WriteLine("Please enter a secret key for the symmetric algorithm.");  
			//var key = Console.ReadLine();  

			Console.WriteLine("Please enter a string for encryption");
			var str = Console.ReadLine();
			var encryptedString = AesOperation.EncryptString(key, str);
			Console.WriteLine($"encrypted string = {encryptedString}");

			var decryptedString = AesOperation.DecryptString(key, encryptedString);
			Console.WriteLine($"decrypted string = {decryptedString}");

			Console.ReadKey();
		}
	}

	public class AesOperation
	{
		public static string EncryptString(string key, string plainText)
		{
			byte[] iv = new byte[16];
			byte[] array;

			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(key);
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
						{
							streamWriter.Write(plainText);
						}

						array = memoryStream.ToArray();
					}
				}
			}

			return Convert.ToBase64String(array);
		}

		public static string DecryptString(string key, string cipherText)
		{
			byte[] iv = new byte[16];
			byte[] buffer = Convert.FromBase64String(cipherText);

			using (Aes aes = Aes.Create())
			{
				aes.Key = Encoding.UTF8.GetBytes(key);
				aes.IV = iv;
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
		}
	}
}
