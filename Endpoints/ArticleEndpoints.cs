using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using RestauSimplon.GestionArticle;
namespace RestauSimplon.Endpoints
{
    public static class ArticleEndpoints
    {
        // -- Méthode permettant de créer les différents Endpoints pour les articles --
        public static RouteGroupBuilder MapArticleEndpoints(this RouteGroupBuilder group)
        {
            // -- Read la table article --
            group.MapGet("/", RecupererListeArticle)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère tous les articles",
                    description: "Affiche la liste de tous les articles du restaurant"))
                .WithTags("Articles")
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste des articles trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));


            group.MapGet("/categorie/{categorie}", async (string categorie, ArticleDb db) =>
                await db.Articles
                    .Where(a => a.Categorie.ToLower() == categorie.ToLower())
                    .Select(a => new ArticleDTO(a))
                    .ToListAsync())
                    .WithTags("Articles")
                .WithMetadata(new SwaggerOperationAttribute("Articles par catégorie", "Récupère les articles d’une catégorie donnée"));

            // -- Create une nouvelle ligne dans la table article --
            group.MapPost("/", CreerArticle)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Rajouter un article",
                    description: "Rajoute un article dans la base de donnée"))
                .WithTags("Articles")
                .WithMetadata(new SwaggerResponseAttribute(200, "article ajouté"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Erreur de l'ajout article"));

            // -- Update une ligne de la table article --
            group.MapPut("/{id}", MettreAJourArticle)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Mettre à jour un article",
                    description: "Mets à jour les informations d'un article"))
                .WithTags("Articles")
                .WithMetadata(new SwaggerResponseAttribute(200, "Informations modifiées"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de retrouver l'article"));

            // -- Delete une ligne de la table article --
            group.MapDelete("/{id}", SupprimerArticle)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Supprime un article",
                    description: "Permet de retirer un article de notre base de donnée"))
                .WithTags("Articles")
                .WithMetadata(new SwaggerResponseAttribute(200, "article supprimé"))
                .WithMetadata(new SwaggerResponseAttribute(404, "L'article n'existe pas"));

            return group;
        }


    }
}