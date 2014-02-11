namespace SnakeBattleNet.Web.Models
{
    public class SnakeStatsViewModel : SnakeViewModelBase
    {
        public int Wins { get; set; }
        public int Loses { get; set; }
        public int Matches { get; set; }
        public int Score { get; set; }

        public SnakeStatsViewModel(string id, string name, int wins, int loses, int matches, int score)
            : base(id, name)
        {
            this.Wins = wins;
            this.Loses = loses;
            this.Matches = matches;
            this.Score = score;
        }
    }
}