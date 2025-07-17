using Microsoft.EntityFrameworkCore;

namespace RestauSimplon.GestionClient
{
    public class ClientsDb : DbContext
    {
        public ClientsDb(DbContextOptions<ClientsDb> options) : base (options) { }

        public DbSet<Clients> Clients => Set<Clients>();
    }
}
