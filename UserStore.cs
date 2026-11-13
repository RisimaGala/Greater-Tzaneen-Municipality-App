using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace Greater_Tzaneen_Municipality_App
{
    /// <summary>
    /// Local account store. Passwords are never stored in plain text: each user has a random
    /// salt and a PBKDF2 hash. Data lives in %AppData%\GreaterTzaneenMunicipalityApp\users.txt
    /// as lines of "username|saltBase64|hashBase64".
    /// </summary>
    public static class UserStore
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100000;

        public static string CurrentUser { get; private set; }

        private static string FilePath
        {
            get
            {
                string dir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "GreaterTzaneenMunicipalityApp");
                Directory.CreateDirectory(dir);
                return Path.Combine(dir, "users.txt");
            }
        }

        public static bool Register(string username, string password, out string error)
        {
            username = (username ?? "").Trim();
            if (username.Length < 3 || username.IndexOf('|') >= 0)
            {
                error = "Username must be at least 3 characters and cannot contain '|'.";
                return false;
            }
            if ((password ?? "").Length < 6)
            {
                error = "Password must be at least 6 characters.";
                return false;
            }

            var users = Load();
            if (users.ContainsKey(username))
            {
                error = "That username is already taken.";
                return false;
            }

            byte[] salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            users[username] = new[] { Convert.ToBase64String(salt), Convert.ToBase64String(Hash(password, salt)) };
            Save(users);

            CurrentUser = username;
            error = null;
            return true;
        }

        public static bool Login(string username, string password)
        {
            username = (username ?? "").Trim();
            var users = Load();
            string[] entry;
            if (!users.TryGetValue(username, out entry))
            {
                return false;
            }

            byte[] expected = Convert.FromBase64String(entry[1]);
            byte[] actual = Hash(password ?? "", Convert.FromBase64String(entry[0]));

            // Constant-time comparison.
            int diff = expected.Length ^ actual.Length;
            for (int i = 0; i < expected.Length && i < actual.Length; i++)
            {
                diff |= expected[i] ^ actual[i];
            }
            if (diff != 0)
            {
                return false;
            }

            CurrentUser = username;
            return true;
        }

        private static byte[] Hash(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                return pbkdf2.GetBytes(HashSize);
            }
        }

        private static Dictionary<string, string[]> Load()
        {
            var users = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);
            if (!File.Exists(FilePath))
            {
                return users;
            }
            foreach (string line in File.ReadAllLines(FilePath))
            {
                string[] parts = line.Split('|');
                if (parts.Length == 3)
                {
                    users[parts[0]] = new[] { parts[1], parts[2] };
                }
            }
            return users;
        }

        private static void Save(Dictionary<string, string[]> users)
        {
            var lines = new List<string>();
            foreach (var kv in users)
            {
                lines.Add(kv.Key + "|" + kv.Value[0] + "|" + kv.Value[1]);
            }
            File.WriteAllLines(FilePath, lines);
        }
    }
}
