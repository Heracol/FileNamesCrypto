
using FileNamesCrypto;
using FileNamesEncryptor;

string password = Helper.ReadPassword();
Console.WriteLine();

Helper.StorePassword(password);

Crypto crypto = new Crypto(password);
Encryptor encryptor = new Encryptor(crypto);

string curDir = Directory.GetCurrentDirectory();
string[] dirs = Directory.GetDirectories(curDir);

foreach (var dir in dirs)
{
    encryptor.RenameEntries(dir);
}

Console.WriteLine("\nPress any key to exit . . .");
Console.ReadKey();
