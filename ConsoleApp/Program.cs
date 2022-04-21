using System;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Retrieving webpage ...");

            string content = await GetWebpageContent(@"https://www.microsoft.com");
            Console.WriteLine($"Content size: {content.Length}");
            Console.WriteLine($"Content: {content}");

            Console.ReadKey();
        }

        private static Task<string> GetWebpageContent(string Url)
        {
            WebClient client = new WebClient();
            return client.DownloadStringTaskAsync(Url);
        }

    }
}