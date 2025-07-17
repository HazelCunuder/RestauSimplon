using Microsoft.EntityFrameworkCore;

namespace RestauSimplon.GestionClient
{
    public class GroupCommandeDb : DbContext
    {
        public GroupCommandeDb(DbContextOptions<GroupCommandeDb> options) : base (options) { }

        public DbSet<GroupCommande> GroupCommande => Set<GroupCommande>();
    }
}
