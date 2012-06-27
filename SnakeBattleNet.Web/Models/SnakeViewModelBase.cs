namespace SnakeBattleNet.Web.Models
{
    public class SnakeViewModelBase
    {
        public string Id { get; set; }

        public SnakeViewModelBase() { }
        public SnakeViewModelBase(string id)
        {
            Id = id;
        }
    }
}