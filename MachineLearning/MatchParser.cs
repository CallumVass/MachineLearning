namespace MachineLearning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using RDotNet;

    public class MatchParser
    {
        private readonly IEnumerable<Match> matches;

        public MatchParser(IEnumerable<Match> matches)
        {
            this.matches = matches;
        }

        public string[] Teams
        {
            get
            {
                var teams = new List<string>();
                teams.AddRange(this.matches.Select(e => e.HomeTeam));
                teams.AddRange(this.matches.Select(e => e.AwayTeam));
                return teams.Distinct().OrderBy(t => t).ToArray();
            }
        }

        public int NumberOfTeams
        {
            get
            {
                return this.Teams.Length;
            }
        }

        public int NumberOfGames
        {
            get
            {
                return this.matches.Count();
            }
        }

        public List<double> Y
        {
            get
            {
                var results = new List<double>();
                foreach (var match in this.matches)
                {
                    results.Add(match.HomeScore);
                    results.Add(match.AwayScore);
                }

                return results;
            }
        }

        public double[,] X
        {
            get
            {
                var meh = new double[(2 * this.NumberOfGames), ((2 * this.NumberOfTeams) + 1)];
                for (var i = 0; i < this.NumberOfGames; i++)
                {
                    var match = this.matches.ElementAt(i);
                    var m = Array.IndexOf(this.Teams, match.HomeTeam);
                    var n = Array.IndexOf(this.Teams, match.AwayTeam);
                    meh[2 * i, m] = 1;
                    meh[2 * i, n + this.NumberOfTeams] = -1;
                    meh[(2 * i) + 1, n] = 1;
                    meh[(2 * i) + 1, m + this.NumberOfTeams] = -1;
                    meh[2 * i, 2 * this.NumberOfTeams] = 1;
                }

                return meh;
            }
        }

        public double[,] XX
        {
            get
            {
                int[] indices = { 0 };
                var meh = this.ResizeArray(this.X, indices);
                return meh;
            }
        }

        public MatchPredictor Build()
        {
            var engine = REngine.GetInstance();
            var y = engine.CreateNumericVector(this.Y);
            engine.SetSymbol("y", y);
            var xx = engine.CreateNumericMatrix(this.XX);
            engine.SetSymbol("xx", xx);
            engine.Evaluate("parameters <- glm(formula = y ~ 0 + xx, family = poisson)").AsVector().ToArray();
            var z = engine.Evaluate("z <- c(0, coefficients(parameters))");
            var p = z.AsNumericMatrix();

            var teams = this.Teams.Select(
                (t, i) =>
                new TeamAdvantage
                    {
                        Team = t,
                        Attack = Convert.ToDouble(p[i, 0]),
                        Defence = Convert.ToDouble(p[i + 20, 0])
                    });

            return new MatchPredictor(teams, Convert.ToDouble(p[2 * this.NumberOfTeams, 0]));
        }

        public T[,] ResizeArray<T>(T[,] original, int[] columnsToRemove)
        {
            var newArray = new T[original.GetLength(0), original.GetLength(1) - columnsToRemove.Length];
            int minRows = original.GetLength(0);
            for (int i = 0; i < minRows; i++)
            {
                int currentColumn = 0;
                for (int j = 0; j < original.GetLength(1); j++)
                {
                    if (columnsToRemove.Contains(j) == false)
                    {
                        newArray[i, currentColumn] = original[i, j];
                        currentColumn++;
                    }
                }
            }

            return newArray;
        }
    }
}