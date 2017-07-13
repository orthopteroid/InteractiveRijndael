using System;
using System.Text;
using System.IO;
using System.Security.Cryptography;

// Cut and paste into a C# interactive console.
// Read or run Demo(); for instructions on usage.
// orthopteroid@gmail.com
// GPLv3 

public static string Encrypt(string key, string plaintext)
{
    if (key.Length <= 8) throw new Exception("key too short");
    Rfc2898DeriveBytes kb = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes(key));
    using (MemoryStream ms = new MemoryStream())
    using (RijndaelManaged rm = new RijndaelManaged())
    using (ICryptoTransform ct = rm.CreateEncryptor(kb.GetBytes(rm.KeySize / 8), kb.GetBytes(rm.BlockSize / 8)))
    using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
    {
        byte[] utf8PlainBytes = Encoding.UTF8.GetBytes(plaintext);
        cs.Write(utf8PlainBytes, 0, utf8PlainBytes.Length);
        cs.FlushFinalBlock();
        byte[] utf8CryptBytes = ms.ToArray();
        return Convert.ToBase64String(utf8CryptBytes);
    }
}

public static string Decrypt(string key, string b64utf8Cryptext)
{
    if (key.Length <= 8) throw new Exception("key too short");
    Rfc2898DeriveBytes kb = new Rfc2898DeriveBytes(key, Encoding.UTF8.GetBytes(key));
    using (MemoryStream ms = new MemoryStream())
    using (RijndaelManaged rm = new RijndaelManaged())
    using (ICryptoTransform ct = rm.CreateDecryptor(kb.GetBytes(rm.KeySize / 8), kb.GetBytes(rm.BlockSize / 8)))
    using (CryptoStream cs = new CryptoStream(ms, ct, CryptoStreamMode.Write))
    {
        byte[] uft8CryptBytes = Convert.FromBase64String(b64utf8Cryptext);
        cs.Write(uft8CryptBytes, 0, uft8CryptBytes.Length);
        cs.FlushFinalBlock();
        byte[] utf8PlainBytes = ms.ToArray();
        return Encoding.UTF8.GetString(utf8PlainBytes);
    }
}

public static string NewKey(int bitSize)
{
    byte[] randomBytes = new byte[bitSize / 8];
    (new System.Security.Cryptography.RNGCryptoServiceProvider()).GetBytes(randomBytes);
    return Convert.ToBase64String(randomBytes);
}

public static void Demo()
{
    string secretText = "TheRainInSpainFallsMainlyOnThePlain";

    Console.Write("// sending end...\n");
    Console.Write("// you have some secret-text you wish to send:\n\n"+ secretText + "\n\n");

    string sideChannelKey = NewKey(128); // 128 bit key, enlarge as necessary
    Console.Write("// make a random 128 bit secret-key:\n");
    Console.Write("// (send this secret-key by some secure side-channel means)\n\n");
    Console.Write("Console.WriteLine(NewKey(128));\n\n" + sideChannelKey + "\n\n");

    string cipherText = Encrypt(sideChannelKey, secretText);
    Console.Write("// use the secret-key to generate the encrypted-text:\n");
    Console.Write("// (send encrypted-text over an unsecured open-channel)\n\n");
    Console.Write("Console.WriteLine(Encrypt(\"" + sideChannelKey + "\", \"" + secretText + "\"));\n\n");
    Console.Write(cipherText + "\n\n");

    Console.Write("// receiving end...\n");
    Console.Write("// use the key and encrypted-text to recreate the secret-text\n\n");
    Console.Write("Console.WriteLine(Decrypt(\""+sideChannelKey+ "\", \"" + cipherText + "\"));\n\n");
    Console.Write(Decrypt(sideChannelKey, cipherText) + "\n\n");
}