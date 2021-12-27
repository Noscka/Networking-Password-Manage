using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Public_Private_Keys
{
	internal class Program
	{
		static void Main(string[] args)
		{
			// Create an instance of the RSA algorithm class  
			RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
			SymmetricAlgorithm aes = new AesManaged();
			byte[] key = aes.Key;

			Console.WriteLine(key);

			// Get the public keyy   
			string publicKey = rsa.ToXmlString(false); // false to get the public key   
			string privateKey = rsa.ToXmlString(true); // true to get the private key   

			Console.WriteLine($"{publicKey}\n{privateKey}");

			// Call the encryptText method   
			EncryptText(publicKey, "Hello from C# Corner", "encryptedData.dat");

			// Call the decryptData method and print the result on the screen   
			Console.WriteLine("Decrypted message: {0}", DecryptData(privateKey, "encryptedData.dat"));

		}

		// Create a method to encrypt a text and save it to a specific file using a RSA algorithm public key   
		static void EncryptText(string publicKey, string text, string fileName)
		{
			// Convert the text to an array of bytes   
			UnicodeEncoding byteConverter = new UnicodeEncoding();
			byte[] dataToEncrypt = byteConverter.GetBytes(text);

			// Create a byte array to store the encrypted data in it   
			byte[] encryptedData;
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
			{
				// Set the rsa pulic key   
				rsa.FromXmlString(publicKey);

				// Encrypt the data and store it in the encyptedData Array   
				encryptedData = rsa.Encrypt(dataToEncrypt, false);
			}
			// Save the encypted data array into a file   
			File.WriteAllBytes(fileName, encryptedData);

			Console.WriteLine("Data has been encrypted");
		}

		// Method to decrypt the data withing a specific file using a RSA algorithm private key   
		static string DecryptData(string privateKey, string fileName)
		{
			// read the encrypted bytes from the file   
			byte[] dataToDecrypt = File.ReadAllBytes(fileName);

			// Create an array to store the decrypted data in it   
			byte[] decryptedData;
			using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
			{
				// Set the private key of the algorithm   
				rsa.FromXmlString(privateKey);
				decryptedData = rsa.Decrypt(dataToDecrypt, false);
			}

			// Get the string value from the decryptedData byte array   
			UnicodeEncoding byteConverter = new UnicodeEncoding();
			return byteConverter.GetString(decryptedData);
		}
	}
}
