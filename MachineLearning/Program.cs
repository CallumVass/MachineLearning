using System;

namespace MachineLearning
{
    using System.IO;
    using System.Linq;
    using System.Net;

    using CsvHelper;

    public class Program
    {
        public static void Main(string[] args)
        {
            var webClient = new WebClient();
            var fileData = webClient.DownloadString("http://elysiantech.co.uk:3000/Results1516header.csv");
            using (var sr = new StringReader(fileData))
            {
                var reader = new CsvReader(sr);
                var records = reader.GetRecords<Match>().ToList();
                var parser = new MatchParser(records);
                var data = parser.Build();
                Console.WriteLine("Please enter a home team: ");
                var home = Console.ReadLine();
                Console.WriteLine("Please enter an away team: ");
                var away = Console.ReadLine();
                var result = data.GetScore(home,away);
                Console.WriteLine("Home: {0}, Away: {1}", result.HomeTeamScore.Result, result.AwayTeamScore.Result);
                Console.ReadLine();
            }
        }
    }
}