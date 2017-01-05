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
                //var result = data.GetScore(home,away);
                var probs = data.ProbabilityTable(home, away);
                var pct = data.ResultProbability(probs);

                var highest = data.FindHighestAsString(probs);

                Console.WriteLine(" ");
                Console.WriteLine("------------------------");
                Console.WriteLine(string.Format("{0}: {1}%, {2}: {3}%, {4}: {5}%", home, pct.Home, away, pct.Away, "draw", pct.Draw));
                Console.WriteLine(string.Format("Likeliest Result: {0}", highest));
                Console.WriteLine("------------------------");
                Console.WriteLine(" ");

                Console.ReadLine();
            }
    }
}