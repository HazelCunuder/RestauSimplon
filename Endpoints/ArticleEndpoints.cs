using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
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

        static async Task<IResult> RecupererListeArticle(ArticleDb db)
        {
            return TypedResults.Ok(await db.Articles.Select(x => new ArticleDTO(x)).ToArrayAsync());
        }

        static async Task<IResult> CreerArticle(ArticleDTO articlesDTO, ArticleDb db)
        {
            var validationResults = new List<ValidationResult>();
            var contextValidation = new ValidationContext(articlesDTO, null, null);

            if (!Validator.TryValidateObject(articlesDTO, contextValidation, validationResults, true))
            {
                var errors = validationResults.Select(v => v.ErrorMessage);
                return TypedResults.BadRequest(new { Errors = errors });
            }
            var article = new Article
            {
                Nom = articlesDTO.Nom,
                Categorie = articlesDTO.Categorie,
                Prix = articlesDTO.Prix,
            };

            db.Articles.Add(article);
            await db.SaveChangesAsync();

            articlesDTO = new ArticleDTO(article);

            return TypedResults.Created($"/articles/{article.Id}", articlesDTO);
        }

        static async Task<IResult> MettreAJourArticle(int id, ArticleDTO articleDTO, ArticleDb db)
        {

            var validationResults = new List<ValidationResult>();
            var contextValidation = new ValidationContext(articleDTO, null, null);

            if (!Validator.TryValidateObject(articleDTO, contextValidation, validationResults, true)) 
            {
                var errors = validationResults.Select(v => v.ErrorMessage);
                return TypedResults.BadRequest(new { Errors = errors });
            }

            var article = await db.Articles.FindAsync(id);

            if (article is null) return TypedResults.NotFound();

            article.Nom = articleDTO.Nom;
            article.Categorie = articleDTO.Categorie;
            article.Prix = articleDTO.Prix;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> SupprimerArticle(int id, ArticleDb db)
        {
            if (await db.Articles.FindAsync(id) is Article article)
            {
                db.Articles.Remove(article);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}