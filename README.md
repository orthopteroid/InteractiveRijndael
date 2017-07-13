# InteractiveRijndael
Encrypt/Decrypt text using Rijndael in a C# interactive console

For use with csi.exe (on my machine, at C:\Program Files (x86)\MSBuild\14.0\Bin\csi.exe) or the Visual Studio C# Interactive window, described in

https://msdn.microsoft.com/en-us/magazine/mt614271.aspx

or possibly the mono REPL, described in

http://www.mono-project.com/docs/tools+libraries/tools/repl/

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
