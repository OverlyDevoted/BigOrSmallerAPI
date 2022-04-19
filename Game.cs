using System.Globalization;

namespace Catalog
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Cover_url { get; set; } = String.Empty;
        public bool IsSmallerMode { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public User User { get; set; } 
        public static List<Game> SortByDate(List<Game> list)
        {
            list.Sort((delegate (Game a, Game b) { return DateTime.Compare(b.Created, a.Created); }));
            return list;
        }
    }
}
