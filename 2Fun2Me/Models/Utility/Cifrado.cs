using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TwoFunTwoMe.Models.Utility
{
	public class Cifrado
	{
		public byte[] Clave = Encoding.ASCII.GetBytes("TwoFunTwoMeEncryp");
		public byte[] IV = Encoding.ASCII.GetBytes("Two.Fun.Two.Me.Encryp");

		public string cifrar(string textoCifrar)
		{
			byte[] inputBytes = Encoding.ASCII.GetBytes(textoCifrar);
			byte[] encripted;
			RijndaelManaged cripto = new RijndaelManaged();
			using (MemoryStream ms = new MemoryStream(inputBytes.Length))
			{
				using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateEncryptor(Clave, IV), CryptoStreamMode.Write))
				{
					objCryptoStream.Write(inputBytes, 0, inputBytes.Length);
					objCryptoStream.FlushFinalBlock();
					objCryptoStream.Close();
				}
				encripted = ms.ToArray();
			}
			return Convert.ToBase64String(encripted);
		}


		public string descifrar(string textoCifrado)
		{
			byte[] inputBytes = Convert.FromBase64String(textoCifrado);
			byte[] resultBytes = new byte[inputBytes.Length];
			string textoLimpio = String.Empty;
			RijndaelManaged cripto = new RijndaelManaged();
			using (MemoryStream ms = new MemoryStream(inputBytes))
			{
				using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(Clave, IV), CryptoStreamMode.Read))
				{
					using (StreamReader sr = new StreamReader(objCryptoStream, true))
					{
						textoLimpio = sr.ReadToEnd();
					}
				}
			}
			return textoLimpio;
		}
	}
}