using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Dapper.Net.Extensions {

    public static class StringExtensions {
        public static string Squeeze(this string input) => Regex.Replace(input, @"\s+", " ");
        public static string TrimSqueeze(this string input) => input.Trim().Squeeze();

        public static string ReadFileContents(this string path) {
            using (var sr = File.OpenText(path)) {
                return sr.ReadToEnd().SanitizeSql();
            }
        }

        public static async Task<string> ReadFileContentsAsync(this string path) {
            using (var sr = File.OpenText(path)) {
                return (await sr.ReadToEndAsync()).SanitizeSql();
            }
        }

        public static string SanitizeSql(this string input) => input.Replace("\r\n", ";").Replace('\r', ';').Replace('\n', ';').Replace("\t", "    ");
    }

}