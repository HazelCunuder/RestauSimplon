using Microsoft.EntityFrameworkCore;

public class CommandeDb : DbContext
{
    public CommandeDb(DbContextOptions<CommandeDb> options)
        : base(options) { }

    public DbSet<Commande> Commandes => Set<Commande>();
}