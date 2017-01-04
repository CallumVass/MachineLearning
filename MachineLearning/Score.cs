namespace MachineLearning
{
    public class Score
    {
        private readonly TeamAdvantage team;

        private readonly int result;

        public Score(TeamAdvantage team, int result)
        {
            this.team = team;
            this.result = result;
        }

        public TeamAdvantage Team
        {
            get
            {
                return this.team;
            }
        }

        public int Result
        {
            get
            {
                return this.result;
            }
        }
    }
}