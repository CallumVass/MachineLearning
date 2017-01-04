namespace MachineLearning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SeasonData
    {
        private readonly IEnumerable<TeamAdvantage> teams;

        private readonly double homeAdvantage;

        public SeasonData(IEnumerable<TeamAdvantage> teams, double homeAdvantage)
        {
            this.teams = teams;
            this.homeAdvantage = homeAdvantage;
        }

        public void GetScore(string homeTeamName, string awayTeamName)
        {
            var homeTeam = this.teams.FirstOrDefault(t => t.Team == homeTeamName);
            var awayTeam = this.teams.FirstOrDefault(t => t.Team == awayTeamName);

            // Need to wrap rpois
            var homeResult = Math.Exp(homeTeam.Attack - awayTeam.Defence + this.homeAdvantage);
            var awayResult = Math.Exp(awayTeam.Attack - homeTeam.Defence);

            // Need to return an object here with Home Result, Away Result, End Result
        }
    }
}