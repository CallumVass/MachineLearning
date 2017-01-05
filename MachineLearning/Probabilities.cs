namespace MachineLearning
{
    public class Probabilities
    {
        public double Home { get; set; }

        public double Away { get; set; }

        public double Draw { get; set; }

        public override string ToString()
        {
            return string.Format("Home Win: {0}%, Away Win: {1}%, Draw: {2}%", this.Home, this.Away, this.Draw);
        }
    }
}
