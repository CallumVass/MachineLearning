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

        public ScoreResult GetScore(string homeTeamName, string awayTeamName)
        {
            var homeTeam = this.teams.FirstOrDefault(t => t.Team == homeTeamName);
            var awayTeam = this.teams.FirstOrDefault(t => t.Team == awayTeamName);

            // Need to wrap rpois
            var homeResult = Poisson.Sample(Math.Exp(homeTeam.Attack - awayTeam.Defence + this.homeAdvantage));
            var awayResult = Poisson.Sample(Math.Exp(awayTeam.Attack - homeTeam.Defence));

            // Need to return an object here with Home Result, Away Result, End Result
            return new ScoreResult
            {
                HomeTeamScore = new Score(homeTeam, homeResult),
                AwayTeamScore = new Score(awayTeam, awayResult)
            };
        }
    }
}