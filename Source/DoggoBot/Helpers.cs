using System;
using System.IO;
using System.Net;
using System.Drawing;

namespace DoggoBot
{
    public static class Helpers
    {
        public static byte[] GetImageAsByteArray(string imageUrl)
        {
            var webClient = new WebClient();
            var data = webClient.DownloadData(new Uri(imageUrl));

            return IsValidImage(data) ? data : null;
        }

        public static bool IsValidImage(byte[] bytes)
        {
            try
            {
                using (var ms = new MemoryStream(bytes))
                {                   
                    Image.FromStream(ms);
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }
    }
}