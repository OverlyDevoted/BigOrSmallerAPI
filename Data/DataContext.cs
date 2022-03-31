using Microsoft.EntityFrameworkCore;

namespace Catalog.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }

    }
}
