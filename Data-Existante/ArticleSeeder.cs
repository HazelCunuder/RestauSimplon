using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionArticle;

namespace RestauSimplon.Data_Existante
{
    public class ArticleSeeder
    {
        // -- Permet d'insérer l'article.json dans notre base de donnée sqlite --
        public static async Task SeedAsync(ArticleDb context)
        {
            // -- Si la base de donnée contient déjà les éléments du json, alors quitter la méthode --
            if (await context.Articles.AnyAsync())
                return;

            // -- Lit le ficihier Json et le transforme en liste de articles --
            var json = await File.ReadAllTextAsync("Data-Existante\\plats.json");
            var articles = JsonSerializer.Deserialize<List<Article>>(json);

            // -- Si la liste de articles n'est pas null, alors, ajouter les données dans la base de donnée --
            if (articles is not null)
            {
                await context.Articles.AddRangeAsync(articles);
                await context.SaveChangesAsync();
            }
        }
    }
}