using FileNamesCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNamesEncryptor
{
    public class Encryptor
    {
        private readonly Crypto crypto;

        public Encryptor(Crypto crypto)
        {
            this.crypto = crypto;
        }

        private string EncryptName(string path)
        {
            string encryptedName = crypto.Encrypt(Path.GetFileName(path));
            string encryptedNameWithExtension = Path.ChangeExtension(encryptedName, "hid");
            string newName = Path.Combine(Path.GetDirectoryName(path), encryptedNameWithExtension);

            return newName;
        }

        public void RenameEntries(string dir)
        {
            if (Path.GetExtension(dir) == ".hid")
                return;

            string[] entries = Directory.GetFiles(dir);

            foreach (var entry in entries)
            {
                if (Path.GetExtension(entry) != ".hid")
                {
                    string newName = EncryptName(entry);
                    Console.WriteLine(entry);
                    File.Move(entry, newName);
                }
            }

            string[] dirs = Directory.GetDirectories(dir);
            Task[] subTasks = new Task[dirs.Length];

            for (int i = 0; i < dirs.Length; i++)
            {
                string subDir = dirs[i];
                subTasks[i] = Task.Run(() => RenameEntries(subDir));
            }

            Task.WaitAll(subTasks);

            string newDirName = EncryptName(dir);
            Console.WriteLine(dir);
            Directory.Move(dir, newDirName);
        }
    }
}
