using System.Text.RegularExpressions;

namespace Application.Misc
{
    class Helper
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        private static readonly HttpClient httpClient = new HttpClient();
        public static string RemoveWhitespace(string input)
        {
            return sWhitespace.Replace(input, string.Empty);
        }

        public static async Task<int> SaveImage(string imagePath, string filenamePath)
        {
            var response = await httpClient.GetAsync(imagePath);
            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }
            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            await File.WriteAllBytesAsync(filenamePath, imageBytes);

            return 0;
        }
    }
}
