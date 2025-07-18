using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionArticle;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;


/*3.Gestion des commandes :
Une commande doit être associée à un client et contenir un ou plusieurs articles.
Les fonctionnalités attendues :
- Créer une commande en sélectionnant un client et des articles.
- Calculer automatiquement le montant total d'une commande.
- Consulter les commandes :
Par client(voir l'historique des commandes d'un client).
Par date(voir les commandes passées dans une journée).*/

public static class GestionCommande
{
    public static void MapEndpoints(WebApplication app)
    {
        /* code de l'API */
        var commandeItems = app.MapGroup("/commandeitems");


        // 	Liste toutes les commandes
        commandeItems.MapGet("/", RecupererListeCommandes)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Récupère toutes les commandes",
                description: "Renvoie la liste complète de toutes les commandes enregistrées."))
            .WithMetadata(new SwaggerResponseAttribute(200, "Liste des commandes trouvées"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));
        
        // 	Liste uniquement les commandes terminées
        commandeItems.MapGet("/complete", RecupererCompleteCommandes)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Récupère toutes les commandes terminées",
                description: "Renvoie la liste complète de toutes les commandes terminées."))
            .WithMetadata(new SwaggerResponseAttribute(200, "Liste des commandes trouvées"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));

        // Récupère une commande par ID
        commandeItems.MapGet("/{id}", ObtenirCommande)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Récupère une commande par ID",
                description: "Renvoie une commande spécifique à partir de son identifiant."))
            .WithMetadata(new SwaggerResponseAttribute(200, "Commande trouvée"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

        // Création d'une commande
        commandeItems.MapPost("/", CreationCommande)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Créer une commande",
                description: "Crée une nouvelle commande à partir des données fournies dans le corps de la requête. Le prix total est calculé automatiquement."))
            .WithMetadata(new SwaggerResponseAttribute(201, "Commande créée avec succès"))
            .WithMetadata(new SwaggerResponseAttribute(400, "Requête invalide"));

        // Mise à jour d'une commande existante
        commandeItems.MapPut("/{id}", MajCommande)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Mettre à jour une commande",
                description: "Met à jour les informations d’une commande existante (statut ou prix)."))
            .WithMetadata(new SwaggerResponseAttribute(204, "Commande mise à jour avec succès"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

        // Suppression d'une commande
        commandeItems.MapDelete("/{id}", SupprimerCommande)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: "Supprimer une commande",
                description: "Supprime définitivement une commande à partir de son identifiant."))
            .WithMetadata(new SwaggerResponseAttribute(204, "Commande supprimée avec succès"))
            .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));
    }

    // 	Liste toutes les commandes
    public static async Task<IResult> RecupererListeCommandes(CommandeDb db)
    {
        return TypedResults.Ok(await db.Commandes.Select(x => new CommandeItemDTO(x)).ToArrayAsync());
    }

    // 	Liste uniquement les commandes terminées
    public static async Task<IResult> RecupererCompleteCommandes(CommandeDb db)
    {
        return TypedResults.Ok(await db.Commandes.Where(c => c.StatutCommande).Select(x => new CommandeItemDTO(x)).ToListAsync());
    }

    // Récupère une commande par ID
    public static async Task<IResult> ObtenirCommande(int id, CommandeDb db)
    {
        return await db.Commandes.FindAsync(id)
            is Commande commande
            ? TypedResults.Ok(new CommandeItemDTO(commande))
            : TypedResults.NotFound();
    }

    // Création d'une commande
    public static async Task<IResult> CreationCommande(CommandeItemDTO dto, CommandeDb db)
    {

        var validationResults = new List<ValidationResult>();
        var contextValidation = new ValidationContext(dto, null, null);

        if (!Validator.TryValidateObject(dto, contextValidation, validationResults, true))
        {
            var errors = validationResults.Select(v => v.ErrorMessage);
            return TypedResults.BadRequest(new { Errors = errors });
        }

        decimal total = 0;

        // Calcul automatique
        if (dto.PrixArticles != null && dto.PrixArticles.Count > 0)
        {
            total = dto.PrixArticles.Sum();
        }

        var commande = new Commande
        {
            StatutCommande = dto.StatutCommande,
            PrixTotal = total
        };

        db.Commandes.Add(commande);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/commandeitems/{commande.Id}", new CommandeItemDTO(commande));
    }

    // Mise à jour (statut, total)
    public static async Task<IResult> MajCommande(int id, CommandeItemDTO commandeItemDTO, CommandeDb db)
    {

        var validationResults = new List<ValidationResult>();
        var contextValidation = new ValidationContext(commandeItemDTO, null, null);

        if (!Validator.TryValidateObject(commandeItemDTO, contextValidation, validationResults, true))
        {
            var errors = validationResults.Select(v => v.ErrorMessage);
            return TypedResults.BadRequest(new { Errors = errors });
        }

        var commande = await db.Commandes.FindAsync(id);
        if (commande is null) return TypedResults.NotFound();

        commande.PrixTotal = commandeItemDTO.PrixTotal;
        commande.StatutCommande = commandeItemDTO.StatutCommande;

        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    // 	Suppression
    public static async Task<IResult> SupprimerCommande(int id, CommandeDb db)
    {
        if (await db.Commandes.FindAsync(id) is Commande commande)
        {
            db.Commandes.Remove(commande);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        return TypedResults.NotFound();
    }
}