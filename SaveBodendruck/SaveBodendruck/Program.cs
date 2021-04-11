namespace SaveBodendruck
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using Microsoft.Extensions.Configuration;

    internal class Program
    {
        public static IConfiguration Configuration { get; set; }

        private static void Main(string[] args)
        {
            LoadingConfiguration();

            SaveBodendruck();

            SaveSunpots();

            Console.WriteLine($" outputFilePath = {Configuration["outputFilePath"]} ");
            Console.WriteLine($" outputFilePathSun = {Configuration["outputFilePathSun"]} ");
            Console.WriteLine($" bodendruckUrl = {Configuration["bodendruckUrl"]} ");
            Console.WriteLine($" sunUrl = {Configuration["sunUrl"]} ");
        }

        private static void LoadingConfiguration()
        {
            var builder = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        private static void SaveBodendruck()
        {
            var date = DateTime.Now.AddDays(1);

            string dateString = date.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            string outPutFilePath = Configuration["outputFilePath"];

            string fileName = $"{outPutFilePath}{dateString}.gif";

            string url = Configuration["bodendruckUrl"];

            DownLoadImage(url, fileName);
        }

        private static void SaveSunpots()
        {
            var date = DateTime.Now;

            string dateString = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            string outPutFilePath = Configuration["outputFilePathSun"];

            string fileName = $"{outPutFilePath}{dateString}.jpg";

            string url = Configuration["sunUrl"];

            DownLoadImage(url, fileName);
        }

        private static void DownLoadImage(string url, string fileName)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream output = System.IO.File.OpenWrite(fileName))
                {
                    using (Stream input = response.GetResponseStream())
                    {
                        byte[] buffer = new byte[8192];
                        int bytesRead;
                        while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            output.Write(buffer, 0, bytesRead);
                        }
                    }
                }
            }
        }
    }
}