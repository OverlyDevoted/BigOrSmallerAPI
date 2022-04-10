namespace Catalog
{
    public class GameItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cover_Url { get; set; }
        public int Score { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}
