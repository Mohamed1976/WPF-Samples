using System;
using System.Net;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //KVK has a webservice which can be used to retrieve company data
            //https://www.kvk.nl/zoeken/handelsregister/?handelsnaam=disq&kvknummer=&straat=&postcode=&huisnummer=&plaats=rotterdam&hoofdvestiging=1&rechtspersoon=1&nevenvestiging=1&zoekvervallen=0&zoekuitgeschreven=1&start=0
            Console.WriteLine("Retrieving webpage ...");
            string url = @"https://www.kvk.nl/zoeken/handelsregister/?handelsnaam=Kadaster&plaats=Arnhem"; 

            string content = await GetWebpageContent(url);
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