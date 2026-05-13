using Flowframes.IO;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Flowframes.Data
{
    internal class PseudoHash
    {
        public static string GetHash(string path, bool b64 = true)
        {
            bool isDir = Directory.Exists(path);
            string hash = "";

            if (isDir)
            {
                var dir = new DirectoryInfo(path);
                var files = IoUtils.GetFileInfosSorted(path);
                hash = $"{dir.Name}{files.Sum(f => f.Length)}{dir.LastWriteTime.ToString("yyyyMMddHHmmss")}";
            }
            else
            {
                var file = new FileInfo(path);
                hash = $"{file.Name}{file.Length}{file.LastWriteTime.ToString("yyyyMMddHHmmss")}";
            }

            if (!b64)
                return hash;

            string hashB64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(hash));
            hashB64 = new string(hashB64.TrimEnd('=').Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray()); // Ensure no invalid chars in b64 hash
            return hashB64;
        }
    }
}
