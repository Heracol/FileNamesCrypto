using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileNamesCrypto
{
    public static class Helper
    {
        public static void StorePassword(string password)
        {
            try
            {
                string hashedPassword = CalculatePasswordHash(password);

                using (StreamWriter writer = new StreamWriter("password.txt"))
                {
                    writer.WriteLine(hashedPassword);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }

        public static string CalculatePasswordHash(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            Rfc2898DeriveBytes derivedBytes = new Rfc2898DeriveBytes(passwordBytes, passwordBytes, 1000, HashAlgorithmName.SHA256);
            byte[] hashedBytes = derivedBytes.GetBytes(32);
            string hashedPassword = Convert.ToBase64String(hashedBytes);

            return hashedPassword;
        }

        public static string? GetStoredPassword()
        {
            if (!File.Exists("password.txt"))
                return null;

            string? hashedPassword = null;
            using (StreamReader reader = new StreamReader("password.txt"))
            {
                hashedPassword = reader.ReadLine();
            }

            return hashedPassword;
        }

        public static string ReadPassword()
        {
            Console.Write("Enter Password: ");

            string password = string.Empty;
            ConsoleKey key;

            do
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }

        public static string ReadPassword(string? hashedPassword)
        {
            string password = ReadPassword();

            if (hashedPassword == null)
                return password;

            while (hashedPassword != CalculatePasswordHash(password))
            {
                Console.WriteLine("Invalid Password!");
                password = ReadPassword();
            }

            return password;
        }
    }
}
