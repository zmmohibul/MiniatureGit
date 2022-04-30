using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace MiniatureGit
{
    public class Utils
    {
        public static async Task<string> WriteObjectAndGetObjectHashAsync<T>(string path, T obj)
        {
            var objectSerialized = JsonSerializer.Serialize<T>(obj);
            var objectHash = GetSha1(objectSerialized);
            await File.WriteAllTextAsync(Path.Join(path, objectHash), objectSerialized);
            return objectHash;
        }

        public static async Task<string> WriteObjectAndGetJson<T>(string path, T obj)
        {
            var objectSerialized = JsonSerializer.Serialize<T>(obj);
            await File.WriteAllTextAsync(path, objectSerialized);
            return objectSerialized;
        }

        public static string GetSha1(string intput)
        {
            using var sha1 = SHA1.Create();
            return Convert.ToHexString(sha1.ComputeHash(UnicodeEncoding.UTF8.GetBytes(intput)));
        }
    }
}