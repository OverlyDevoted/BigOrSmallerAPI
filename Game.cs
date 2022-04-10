namespace Catalog
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Cover_url { get; set; }
        public DateTime Created { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } 
        
    }
}
