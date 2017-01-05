namespace MachineLearning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using MathNet.Numerics.Distributions;

    public class MatchPredictor
    {
        private readonly IEnumerable<TeamAdvantage> teams;

        private readonly double homeAdvantage;

        public MatchPredictor(IEnumerable<TeamAdvantage> teams, double homeAdvantage)
        {
            this.teams = teams;
            this.homeAdvantage = homeAdvantage;
        }

        public double[,] ProbabilityTable(string homeTeamName, string awayTeamName)
        {
            var homeTeam = teams.FirstOrDefault(t => t.Team == homeTeamName);
            var awayTeam = teams.FirstOrDefault(t => t.Team == awayTeamName);
            var lambdaa = Math.Exp(homeTeam.Attack - awayTeam.Defence + homeAdvantage);
            var lambdab = Math.Exp(awayTeam.Attack - homeTeam.Defence);
            var arraya = new List<double>();
            var arrayb = new List<double>();

            for (var i = 0; i < 7; i++)
            {
                arraya.Add(Poisson.PMF(lambdaa, i));
                arrayb.Add(Poisson.PMF(lambdab,i));
            }
            arraya.Add(1-arraya.Sum());
            arrayb.Add(1-arrayb.Sum());
            var dblArray = new double[8,8];
            for (var j = 0; j < 8; j++)
            {
                for (var k = 0; k < 8; k++)
                {
                    dblArray[j, k] = (arraya[k]*arrayb[j]) * 100;
                }
            }
            return dblArray;
        }

 

        public Probabilities ResultProbability(double[,] probs)
        {
            var home = 0.0;
            var away = 0.0;
            var draw = 0.0;
            
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (i > j)
                    {
                        away = away + probs[i, j];
                    }
                    else
                    {
                        if (i == j)
                        {
                            draw = draw + probs[i, j];
                        }
                        else
                        {
                            home = home + probs[i, j];
                        }
                    }
                   
                }
            }

            var p = new Probabilities
            {
                Home = home,
                Away = away,
                Draw = draw
            };

            return p;
        }

        public string FindHighestAsString(double[,]dblArray)
        {
            var trackHighest = 0.0;
            var x = 0;
            var y = 0;
            for (var j = 0; j < 8; j++)
            {
                for (var k = 0; k < 8; k++)
                {
                    if (dblArray[j, k] > trackHighest)
                    {
                        y = j;
                        x = k;
                        trackHighest = dblArray[j, k];
                    }
                }
            }
            return string.Format("Result: {0}:{1} - {2}%", x,y,trackHighest);
        }

        //public ScoreResult GetScore(string homeTeamName, string awayTeamName)
        //{
        //    var homeTeam = this.teams.FirstOrDefault(t => t.Team == homeTeamName);
        //    var awayTeam = this.teams.FirstOrDefault(t => t.Team == awayTeamName);

        //    // Need to wrap rpois
        //    var homeResult = Poisson.Sample(Math.Exp(homeTeam.Attack - awayTeam.Defence + this.homeAdvantage));
        //    var awayResult = Poisson.Sample(Math.Exp(awayTeam.Attack - homeTeam.Defence));

        //    // Need to return an object here with Home Result, Away Result, End Result
        //    return new ScoreResult
        //    {
        //        HomeTeamScore = new Score(homeTeam, homeResult),
        //        AwayTeamScore = new Score(awayTeam, awayResult)
        //    };
        //}
    }
}