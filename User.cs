using System.Text.Json.Serialization;

namespace Catalog
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [JsonIgnore]
        public List<Game> Games { get; set; }
    }
}
