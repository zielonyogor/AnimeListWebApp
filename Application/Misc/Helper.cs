using Application.Models;
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
            Console.WriteLine(response.Content);
            if (!response.IsSuccessStatusCode)
            {
                return -1;
            }

            var imageBytes = await httpClient.GetByteArrayAsync(imagePath);
            await File.WriteAllBytesAsync(filenamePath, imageBytes);

            return 0;
        }

        public static void DeleteImage(string? path)
        {
            // delete previous image
            if (!String.IsNullOrWhiteSpace(path))
            {
                var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var imageFileName = Path.GetFileName(path);
                var imagePath = Path.Combine(wwwrootPath, imageFileName);
                Console.WriteLine($"{imagePath}");

                System.IO.File.Delete(imagePath);
            }
        }
    }
}
