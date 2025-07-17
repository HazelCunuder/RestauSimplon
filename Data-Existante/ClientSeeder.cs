using RestauSimplon.GestionClient;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace RestauSimplon.Data_Existante
{
    public class ClientSeeder
    {
        // -- Permet d'insérer le client.json dans notre base de donnée sqlite --
        public static async Task SeedAsync(ClientsDb context)
        {
            // -- Si la base de donnée contient déjà les éléments du json, alors quitter la méthode --
            if (await context.Clients.AnyAsync())
                return;

            // -- Lit le ficihier Json et le transforme en liste de Clients --
            var json = await File.ReadAllTextAsync("Data-Existante\\clients.json");
            var clients = JsonSerializer.Deserialize<List<Clients>>(json);

            // -- Si la liste de clients n'est pas null, alors, ajouter les données dans la base de donnée --
            if (clients is not null)
            {
                await context.Clients.AddRangeAsync(clients);
                await context.SaveChangesAsync();
            }
        }
    }
}
