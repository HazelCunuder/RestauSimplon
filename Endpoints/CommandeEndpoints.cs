using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionArticle;
using RestauSimplon.GestionClient;
using RestauSimplon.GestionCommande;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Sockets;

/*3.Gestion des commandes :
Une commande doit être associée à un client et contenir un ou plusieurs articles.
Les fonctionnalités attendues :
- Créer une commande en sélectionnant un client et des articles.
- Calculer automatiquement le montant total d'une commande.
- Consulter les commandes :
Par client(voir l'historique des commandes d'un client).
Par date(voir les commandes passées dans une journée).*/

namespace RestauSimplon.Endpoints
{
    public static class CommandeEndpoints
    {
        public static RouteGroupBuilder MapCommandeEndpoints(this RouteGroupBuilder group)
        {
            // Création d'une commande 
            group.MapPost("/", CreationCommande)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Créer une commande",
                    description:
                    "Crée une nouvelle commande à partir des données fournies dans le corps de la requête. Le prix total est calculé automatiquement."))
                .WithMetadata(new SwaggerResponseAttribute(201, "Commande créée avec succès"))
                .WithMetadata(new SwaggerResponseAttribute(400, "Requête invalide"));

            // 	Liste toutes les commandes
            group.MapGet("/", RecupererListeCommandes)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère toutes les commandes",
                    description: "Renvoie la liste complète de toutes les commandes enregistrées."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste des commandes trouvées"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));

            // 	Liste toutes les commandes en attente
            group.MapGet("/attente", RecupererAttenteCommandes)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère les commandes en attente",
                    description: "Affiche toutes les commandes dont le statut est en attente (StatutCommande = false)."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Commandes en attente"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Aucune commande trouvée"));

            // 	Liste uniquement les commandes terminées
            group.MapGet("/complete", RecupererCompleteCommandes)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère toutes les commandes terminées",
                    description: "Renvoie la liste complète de toutes les commandes terminées."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste des commandes trouvées"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));

            // Récupère une commande par ID commande
            group.MapGet("/{id}", ObtenirCommande)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère une commande par ID",
                    description: "Renvoie une commande spécifique à partir de son identifiant."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Commande trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

            // Récupère une commande par l'ID client
            group.MapGet("/par-client/{idClient}", ObtenirCommandeClient)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère les commandes d’un client",
                    description: "Permet de consulter l’historique des commandes d’un client donné."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Commande trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

            // Récupère les commandes passées à une date donnée (au format aaaa-MM-dd)
            group.MapGet("/par-date/{date}", ObtenirCommandesParDate)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère les commandes d'une date donnée",
                    description: "Renvoie toutes les commandes passées à la date indiquée (au format aaaa-MM-dd)."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Commandes trouvées"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Aucune commande pour cette date"));

            // Mise à jour d'une commande existante
            group.MapPut("/{id}", MajCommande)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Mettre à jour une commande",
                    description: "Met à jour les informations d’une commande existante (statut ou prix)."))
                .WithMetadata(new SwaggerResponseAttribute(204, "Commande mise à jour avec succès"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

            // Mise à jour du statut d'un commande existante
            group.MapPut("/{id}/statut", MajStatutCommande)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Modifier le statut d’une commande",
                    description: "Met à jour le statut de livraison d’une commande."))
                .WithMetadata(new SwaggerResponseAttribute(200, "Statut mis à jour"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

            // Suppression d'une commande
            group.MapDelete("/{id}", SupprimerCommande)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Supprimer une commande",
                    description: "Supprime définitivement une commande à partir de son identifiant."))
                .WithMetadata(new SwaggerResponseAttribute(204, "Commande supprimée avec succès"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Commande non trouvée"));

            return group;
        }


        // -- Création d'une commande
        public static async Task<IResult> CreationCommande(
            CommandCreateDTO dto,
            CommandeDb commandeDb,
            ClientsDb clientsDb,
            ArticleDb articleDb)
        {
            // Verifier que le client existe dans ClientsDb
            var client = await clientsDb.Clients.FindAsync(dto.IdClient);

            if (client == null)
            {
                return TypedResults.NotFound($"Client {dto.IdClient} introuvable.");
            }

            // Verifier que l'article existe dans ArticleDb
            if (dto.Articles == null || !dto.Articles.Any())
            {
                return TypedResults.BadRequest("La commande doit contenir au moins un article.");
            }

            // Créer une nouvelle commande
            var commande = new Commande
            {
                DateCommande = DateTime.Now,
                StatutCommande = false,
                IdClient = dto.IdClient,
                PrixTotal = 0,
                lignes = new List<GroupCommande>()
            };

            decimal total = 0;

            // Verifier et traiter l'article
            foreach (var ligne in dto.Articles)
            {
                var article = await articleDb.Articles.FindAsync(ligne.IdPlat);
                if (article == null)
                    return TypedResults.NotFound($"Article {ligne.IdPlat} introuvable.");

                var sousTotal = (decimal)article.Prix * ligne.Quantite;
                total += sousTotal;

                commande.lignes.Add(new GroupCommande
                {
                    IdPlat = ligne.IdPlat,
                    Quantite = ligne.Quantite
                });
            }

            // Sauvegarde dans la base de données
            commande.PrixTotal = total;
            commandeDb.Commandes.Add(commande);
            await commandeDb.SaveChangesAsync();

            return TypedResults.Created($"/commande/{commande.Id}", new CommandeItemDTO(commande));
        }

        // -- Liste toutes les commandes
        public static async Task<IResult> RecupererListeCommandes(CommandeDb db)
        {
            return TypedResults.Ok(await db.Commandes.Select(x => new CommandeItemDTO(x)).ToArrayAsync());
        }


        // -- Liste uniquement les commandes en attente
        public static async Task<IResult> RecupererAttenteCommandes(CommandeDb db)

        {
            var enAttente = await db.Commandes
                .Where(c => !c.StatutCommande)
                .Select(c => new CommandeItemDTO(c))
                .ToListAsync();

            return enAttente.Count == 0
                ? TypedResults.NotFound("Aucune commande en attente de livraison.")
                : TypedResults.Ok(enAttente);
        }

        // -- Liste uniquement les commandes terminées
        public static async Task<IResult> RecupererCompleteCommandes(CommandeDb db)
        {
            return TypedResults.Ok(await db.Commandes.Where(c => c.StatutCommande).Select(x => new CommandeItemDTO(x))
                .ToListAsync());
        }

        // -- Récupère une commande par ID commande
        public static async Task<IResult> ObtenirCommande(int id, CommandeDb db)
        {
            return await db.Commandes.FindAsync(id)
                is Commande commande
                ? TypedResults.Ok(new CommandeItemDTO(commande))
                : TypedResults.NotFound();
        }

        // -- Récupère une commande par ID client
        public static async Task<IResult> ObtenirCommandeClient(int idClient, CommandeDb db)
        {
            var commandes = await db.Commandes
                .Where(c => c.IdClient == idClient)
                .Select(c => new CommandeItemDTO(c))
                .ToListAsync();

            return commandes.Any()
                ? TypedResults.Ok(commandes)
                : TypedResults.NotFound($"Aucune commande trouvée pour le client {idClient}.");
        }

        // -- Récupère une commande par date
        public static async Task<IResult> ObtenirCommandesParDate(string date, CommandeDb db)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
            {
                return TypedResults.BadRequest("Format de date invalide. Utilisez le format yyyy-MM-dd.");
            }

            var commandes = await db.Commandes
                .Where(c => c.DateCommande.Date == parsedDate.Date)
                .Select(c => new CommandeItemDTO(c))
                .ToListAsync();

            if (commandes.Count == 0)
            {
                return TypedResults.NotFound($"Aucune commande trouvée pour la date {parsedDate:yyyy-MM-dd}");
            }

            return TypedResults.Ok(commandes);
        }

        // -- Mise à jour (statut, total)
        public static async Task<IResult> MajCommande(int id, CommandeItemDTO commandeItemDTO, CommandeDb db)
        {
            var commande = await db.Commandes.FindAsync(id);
            if (commande is null) return TypedResults.NotFound();

            commande.PrixTotal = commandeItemDTO.PrixTotal;
            commande.StatutCommande = commandeItemDTO.StatutCommande;

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        // -- Mise à jour du statut
        public static async Task<IResult> MajStatutCommande(int id, bool statut, CommandeDb db)
        {
            var commande = await db.Commandes.FindAsync(id);

            if (commande == null)
                return TypedResults.NotFound($"Commande {id} non trouvée.");

            commande.StatutCommande = statut;
            await db.SaveChangesAsync();

            return TypedResults.Ok($"Statut de la commande {id} mis à jour : " + (statut ? "Livrée" : "En attente"));
        }

        // 	-- Suppression
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
}
