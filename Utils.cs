using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MiniatureGit
{
    public class Utils
    {
        public static async Task<string> WriteObjectAndGetObjectHashAsync<T>(T obj, string path)
        {
            var objectSerialized = JsonSerializer.Serialize<T>(obj);
            var objectHash = GetSha1(objectSerialized);
            await File.WriteAllTextAsync(Path.Join(path, objectHash), objectSerialized);
            return objectHash;
        }

        public static string GetSha1(string intput)
        {
            using var sha1 = SHA1.Create();
            return Convert.ToHexString(sha1.ComputeHash(UnicodeEncoding.UTF8.GetBytes(intput)));
        }
    }
}