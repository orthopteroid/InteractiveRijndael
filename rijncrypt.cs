#!/usr/bin/csharp -s

using System.Text;
using System.IO;
using System.Security.Cryptography;

public static class ArgHelper
{
	public static bool Get(string[] args, string arg, string defaultValue, out string returnValue)
	{
		int i = Array.IndexOf(args, arg);
		returnValue = i < 0 ? defaultValue : args[ i + 1 ];
		return i >= 0;
	}
}

if(Array.IndexOf(Args, "-h") >= 0)
{
	Console.WriteLine(@"rijncrypt.cs (requires mono) <orthopteroid@gmail.com>
-h This Help
-k <base64encodedkey> Your own 128bit base64 key. (optional. If not specified a random key will be used.)
-e <plaintext> Encrypt some plaintext. Prints the key and the ciphertext.
-d <ciphertext> Decrypt then prints some ciphertext. Requires -k option.
	");
	return;
}

int keySize = 128;
byte[] randomBytes = new byte[keySize / 8];
(new System.Security.Cryptography.RNGCryptoServiceProvider()).GetBytes(randomBytes);
string keyB64 = Convert.ToBase64String(randomBytes);
bool rc = ArgHelper.Get(Args, "-k", keyB64, out keyB64);

string plaintext = "";
if(ArgHelper.Get(Args, "-e", plaintext, out plaintext))
{
	if (keyB64.Length <= 8) throw new Exception("key too short");
	Rfc2898DeriveBytes kb = new Rfc2898DeriveBytes(keyB64, Encoding.UTF8.GetBytes(keyB64));
	using (MemoryStream ms = new MemoryStream())
	using (RijndaelManaged rm = new RijndaelManaged())
	using (ICryptoTransform ct = rm.CreateEncryptor(kb.GetBytes(rm.KeySize / 8), kb.GetBytes(rm.BlockSize / 8)))
	using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
	{
		byte[] utf8PlainBytes = Encoding.UTF8.GetBytes(plaintext);
		cs.Write(utf8PlainBytes, 0, utf8PlainBytes.Length);
		cs.FlushFinalBlock();
		byte[] utf8CryptBytes = ms.ToArray();
		Console.WriteLine(keyB64);
		Console.WriteLine(Convert.ToBase64String(utf8CryptBytes));
	}
}

string b64utf8Cryptext = "";
if(ArgHelper.Get(Args, "-d", b64utf8Cryptext, out b64utf8Cryptext))
{
	if (keyB64.Length <= 8) throw new Exception("key too short");
	Rfc2898DeriveBytes kb = new Rfc2898DeriveBytes(keyB64, Encoding.UTF8.GetBytes(keyB64));
	using (MemoryStream ms = new MemoryStream())
	using (RijndaelManaged rm = new RijndaelManaged())
	using (ICryptoTransform ct = rm.CreateDecryptor(kb.GetBytes(rm.KeySize / 8), kb.GetBytes(rm.BlockSize / 8)))
	using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
	{
		byte[] uft8CryptBytes = Convert.FromBase64String(b64utf8Cryptext);
		cs.Write(uft8CryptBytes, 0, uft8CryptBytes.Length);
		cs.FlushFinalBlock();
		byte[] utf8PlainBytes = ms.ToArray();
		Console.WriteLine(Encoding.UTF8.GetString(utf8PlainBytes));
	}
	return;
}
