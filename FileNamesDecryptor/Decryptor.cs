using FileNamesCrypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileNamesDecryptor
{
    public class Decryptor
    {
        private readonly Crypto crypto;

        public Decryptor(Crypto crypto)
        {
            this.crypto = crypto;
        }

        private string DecryptName(string path)
        {
            string encryptedName = crypto.Decrypt(Path.GetFileNameWithoutExtension(path));
            string newName = Path.Combine(Path.GetDirectoryName(path), encryptedName);

            return newName;
        }

        public void RenameEntries(string dir)
        {
            if (Path.GetExtension(dir) != ".hid")
                return;

            string newDirName = DecryptName(dir);
            Console.WriteLine(newDirName);
            Directory.Move(dir, newDirName);

            string[] entries = Directory.GetFiles(newDirName);

            foreach (var entry in entries)
            {
                if (Path.GetExtension(entry) == ".hid")
                {
                    string newName = DecryptName(entry);
                    Console.WriteLine(newName);
                    File.Move(entry, newName);
                }
            }

            string[] dirs = Directory.GetDirectories(newDirName);
            Task[] subTasks = new Task[dirs.Length];

            for (int i = 0; i < dirs.Length; i++)
            {
                string subDir = dirs[i];
                subTasks[i] = Task.Run(() => RenameEntries(subDir));
            }

            Task.WaitAll(subTasks);
        }
    }
}
