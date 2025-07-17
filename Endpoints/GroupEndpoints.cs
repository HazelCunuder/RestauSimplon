using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionClient;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Endpoints
{
    public static class GroupEndpoints
    {
        // -- Méthode permettant de créer les différents Endpoints pour l'association commande/menu --
        public static RouteGroupBuilder MapGroupEndpoints(this RouteGroupBuilder group)
        {
            // -- Read la table client --
            group.MapGet("/", RecupererListeGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère tous les GroupCommande",
                    description: "Affiche la liste de tous les GroupCommande du restaurant"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste des GroupCommande trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));

            // -- Create une nouvelle ligne dans la table client --
            group.MapPost("/", CreerGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Rajouter un client",
                    description: "Rajoute un client dans la base de donnée"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Client ajouté"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Erreur de l'ajout client"));

            // -- Update une ligne de la table client --
            group.MapPut("/{id}", MettreAJourGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Mettre à jour un client",
                    description: "Mets à jour les informations d'un client"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Informations modifiées"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de retrouver le client"));

            // -- Delete une ligne de la table client --
            group.MapDelete("/{id}", SupprimerGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Supprime un client",
                    description: "Permet de retirer un client de notre base de donnée"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Client supprimé"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Le Client n'existe pas"));

            return group;
        }

        static async Task<IResult> RecupererListeGroupe(GroupCommandeDb db)
        {
            return TypedResults.Ok(await db.GroupCommande.Select(x => new GroupCommandeDTO(x)).ToArrayAsync());
        }

        static async Task<IResult> CreerGroupe(GroupCommandeDTO GroupCommandeDTO, GroupCommandeDb db)
        {
            var commandeGroupe = new GroupCommande
            {
                Id = GroupCommandeDTO.Id,
                IdCommande = GroupCommandeDTO.IdCommande,
                IdPlat = GroupCommandeDTO.IdPlat,
                Quantite = GroupCommandeDTO.Quantite
            };

            db.GroupCommande.Add(commandeGroupe);
            await db.SaveChangesAsync();

            GroupCommandeDTO = new GroupCommandeDTO(commandeGroupe);

            return TypedResults.Created($"/GroupCommande/{commandeGroupe.Id}", GroupCommandeDTO);
        }

        static async Task<IResult> MettreAJourGroupe(int id, GroupCommandeDTO groupCommandeDTO, GroupCommandeDb db)
        {
            var commandeGroupe = await db.GroupCommande.FindAsync(id);

            if (commandeGroupe is null) return TypedResults.NotFound();

            commandeGroupe.Id = groupCommandeDTO.Id;
            commandeGroupe.IdCommande = groupCommandeDTO.IdCommande;
            commandeGroupe.IdPlat = groupCommandeDTO.IdPlat;
            commandeGroupe.Quantite = groupCommandeDTO.Quantite;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> SupprimerGroupe(int id, GroupCommandeDb db)
        {
            if (await db.GroupCommande.FindAsync(id) is GroupCommande client)
            {
                db.GroupCommande.Remove(client);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }   
}