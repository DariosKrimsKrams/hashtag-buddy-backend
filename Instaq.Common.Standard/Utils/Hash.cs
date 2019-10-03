namespace Instaq.Common.Utils
{
    using System.Security.Cryptography;
    using System.Text;

    public class Hash
    {
        public static string GetMd5(string input)
        {
            var sb        = new StringBuilder();
            var algorithm = MD5.Create();
            var hash      = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (var b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString().Substring(0, 10).ToLower();
        }

        public static string GetSha256(string input)
        {
            var sb        = new StringBuilder();
            var algorithm = new SHA256CryptoServiceProvider();
            var hash      = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (var b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString().ToLower();
        }
    }
}
