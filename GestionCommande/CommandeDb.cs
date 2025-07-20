using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionClient;

public class CommandeDb : DbContext
{
    public CommandeDb(DbContextOptions<CommandeDb> options)
        : base(options) { }

    public DbSet<Commande> Commandes => Set<Commande>();
    public DbSet<GroupCommande> GroupCommande => Set<GroupCommande>();
}