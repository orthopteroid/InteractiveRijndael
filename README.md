# InteractiveRijndael
Encrypt/Decrypt text using Rijndael in a C# interactive console.

The NewKey function generates a base-64 string containing a psuedorandom key of the specified bitlength - it's not necessary to use this function, you can use your own key. The encrypt/decrypt implmentation uses the supplied key (either your own, or one made by NewKey) to create a psuedorandom key, psuedorandom salt and a psuedorandom initialization vector for the Rijndael algorithim. There are likely problems with running a single keystring through Rfc2898DeriveBytes (based upon SHA1) to make a key, salt and an IV, but there you have it.

Script has been tested with csi.exe (on my machine, at C:\Program Files (x86)\MSBuild\14.0\Bin\csi.exe) or the Visual Studio C# Interactive window, as described in

https://msdn.microsoft.com/en-us/magazine/mt614271.aspx

and on linux using scriptcs.exe with mono - built from source, as described in

https://github.com/scriptcs/scriptcs/wiki/Building-on-Mac-and-Linux

Example C# interactive console session...

```// sending end...
// you have some secret-text you wish to send:

TheRainInSpainFallsMainlyOnThePlain

// make a random 128 bit secret-key:
// (send this secret-key by some secure side-channel means)

Console.WriteLine(NewKey(128));

LtUNnohbcrxtRm/QTcCIDg==

// use the secret-key to generate the encrypted-text:
// (send encrypted-text over an unsecured open-channel)

Console.WriteLine(Encrypt("LtUNnohbcrxtRm/QTcCIDg==", "TheRainInSpainFallsMainlyOnThePlain"));

RKBQMj/QylUmScFwLRflJAcO0Twe7lhU6e5BS6PHVWAcumM1KVr9n6qfUrXMGCQK

// receiving end...
// use the key and encrypted-text to recreate the secret-text

Console.WriteLine(Decrypt("LtUNnohbcrxtRm/QTcCIDg==", "RKBQMj/QylUmScFwLRflJAcO0Twe7lhU6e5BS6PHVWAcumM1KVr9n6qfUrXMGCQK"));

TheRainInSpainFallsMainlyOnThePlain
```
