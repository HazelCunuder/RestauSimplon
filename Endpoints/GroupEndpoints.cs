using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionClient;
using Swashbuckle.AspNetCore.Annotations;
using RestauSimplon.GestionCommande;
using System.ComponentModel.DataAnnotations;

namespace RestauSimplon.Endpoints
{
    public static class GroupEndpoints
    {
        // -- Méthode permettant de créer les différents Endpoints pour l'association commande/menu --
        public static RouteGroupBuilder MapGroupEndpoints(this RouteGroupBuilder group)
        {
            // -- Récupère la liste des GroupCommande --
            group.MapGet("/", RecupererListeGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère tous les GroupCommande",
                    description: "Affiche la liste de tous les GroupCommande du restaurant"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste des GroupCommande trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));

            // -- Création d'un GroupCommande --
            group.MapPost("/", CreerGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Créer un GroupCommande ",
                    description: "Permet de créer un GroupCommande du restaurant"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Création d'un GroupCommande"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de créer un GroupCommande"));

            // -- Mise a jour d'un GroupCommande --
            group.MapPut("/{id}", MettreAJourGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Mettre à jour un GroupCommand",
                    description: "Mets à jour un GroupCommand"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Le GroupCommand a été modifié"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de modifier le GroupCommand"));

            // -- Supprimer un GroupCommand --
            group.MapDelete("/{id}", SupprimerGroupe)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Supprime un GroupCommand",
                    description: "Permet de supprimer un GroupCommand"))
                .WithMetadata(new SwaggerResponseAttribute(200, "GroupCOmmand supprimé"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Le GroupCommand n'existe pas"));

            return group;
        }

        static async Task<IResult> RecupererListeGroupe(GroupCommandeDb db)
        {
            return TypedResults.Ok(await db.GroupCommande.Select(x => new GroupCommandeDTO(x)).ToArrayAsync());
        }

        static async Task<IResult> CreerGroupe(GroupCommandeDTO GroupCommandeDTO, GroupCommandeDb db)
        {
            var validationResults = new List<ValidationResult>();
            var contextValidation = new ValidationContext(GroupCommandeDTO, null, null);

            if (!Validator.TryValidateObject(GroupCommandeDTO, contextValidation, validationResults, true))
            {
                var errors = validationResults.Select(v => v.ErrorMessage);
                return TypedResults.BadRequest(new { Errors = errors });
            }

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

            var validationResults = new List<ValidationResult>();
            var contextValidation = new ValidationContext(groupCommandeDTO, null, null);

            if (!Validator.TryValidateObject(groupCommandeDTO, contextValidation, validationResults, true))
            {
                var errors = validationResults.Select(v => v.ErrorMessage);
                return TypedResults.BadRequest(new { Errors = errors });
            }

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