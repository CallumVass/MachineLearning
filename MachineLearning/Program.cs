using System;
using System.Collections.Generic;

namespace MachineLearning
{
    using System.IO;
    using System.Linq;
    using System.Net;

    using CsvHelper;

    public class Program
    {
        private static MatchPredictor data;
        private static List<Match> records;

        public static void Main(string[] args)
        {
            var webClient = new WebClient();
            var fileData = webClient.DownloadString("http://elysiantech.co.uk:3000/Results1516header.csv");
            using (var sr = new StringReader(fileData))
            {
                var reader = new CsvReader(sr);
                records = reader.GetRecords<Match>().ToList();
                var parser = new MatchParser(records);
                data = parser.Build();
                Options();
            }
        }

        public static void Options()
        {
            while (true)
            {
                Console.WriteLine("Please select an option: ");
                Console.WriteLine("1: Score and Win Prediction");
                Console.WriteLine("2: ");
                Console.WriteLine("3: ");
                Console.WriteLine("4: ");
                Console.WriteLine("5: Compare Results");
                var decision = Console.ReadLine();
                switch (decision)
                {
                    case "1":
                        ScoreAndWinPrediction();
                        break;
                    case "5":
                        Console.WriteLine("set threshold value:");
                        var threshold = Convert.ToDouble(Console.ReadLine());
                        CompareResults(threshold);
                        break;
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public static void ScoreAndWinPrediction()
        {
            Console.WriteLine("Please enter a home team: ");
            var home = Console.ReadLine();
            Console.WriteLine("Please enter an away team: ");
            var away = Console.ReadLine();

            var probs = data.ProbabilityTable(home, away);
            var pct = data.ResultProbability(probs);

            var highest = data.FindHighestAsString(probs);

            Console.WriteLine(" ");
            Console.WriteLine("------------------------");
            Console.WriteLine("{0}: {1}%, {2}: {3}%, {4}: {5}%", home, pct.Home, away, pct.Away, "draw", pct.Draw);
            Console.WriteLine("Likeliest Result: {0}", highest);
            Console.WriteLine("------------------------");
            Console.WriteLine(" ");
        }

        public static void CompareResults(double threshold)
        {
            var correct = 0;
            foreach (var match in records)
            {
                var home = match.HomeTeam;
                var away = match.AwayTeam;

                var probs = data.ProbabilityTable(home, away);
                var pct = data.ResultProbability(probs);

                var highest = data.FindHighestAsString(probs);

                Console.WriteLine(" ");
                Console.WriteLine("------------------------");
                Console.WriteLine("{0}: {1}%, {2}: {3}%, {4}: {5}%", home, pct.Home, away, pct.Away, "draw", pct.Draw);
                Console.WriteLine("Likeliest Result: {0}", highest);

                var homescore = match.HomeScore;
                var awayscore = match.AwayScore;

                if (pct.Home > (pct.Away + threshold) && pct.Home > (pct.Draw + threshold))
                {
                    Console.WriteLine(home + " win");

                    if (homescore > awayscore)
                    {
                        Console.WriteLine("correct");
                        correct++;
                    }
                    else
                    {
                        Console.WriteLine("wrong");
                    }
                }
                else if (pct.Away > (pct.Home + threshold) && pct.Away > (pct.Draw + threshold))
                {
                    Console.WriteLine(away + " win");

                    if (awayscore > homescore)
                    {
                        Console.WriteLine("correct");
                        correct++;
                    }
                    else
                    {
                        Console.WriteLine("wrong");
                    }
                }
                else
                {
                    Console.WriteLine("draw");

                    if (homescore == awayscore)
                    {
                        Console.WriteLine("correct");
                        correct++;
                    }
                    else
                    {
                        Console.WriteLine("wrong");
                    }
                }

                Console.WriteLine("------------------------");
                Console.WriteLine(" ");
            }

            var pctage = correct / (double)records.Count * 100;

            Console.WriteLine("{0} out of {1} ({2}%)", correct, records.Count, pctage);
        }
    }
}