
using FileNamesCrypto;
using FileNamesDecryptor;

string hashedPassword = Helper.GetStoredPassword();

string password = Helper.ReadPassword(hashedPassword);
Console.WriteLine();

Crypto crypto = new Crypto(password);
Decryptor decryptor = new Decryptor(crypto);

string curDir = Directory.GetCurrentDirectory();
string[] dirs = Directory.GetDirectories(curDir);

foreach (var dir in dirs)
{
    decryptor.RenameEntries(dir);
}

Console.WriteLine("\nPress any key to exit . . .");
Console.ReadKey();
