using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionClient;
using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.Endpoints
{
    public static class ClientsEndpoints
    {
        // -- Méthode permettant de créer les différents Endpoints pour les clients --
        public static RouteGroupBuilder MapClientsEndpoints(this RouteGroupBuilder group)
        {
            // -- Read la table client --
            group.MapGet("/", RecupererListeClient)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Récupère tous les clients",
                    description: "Affiche la liste de tous les clients du restaurant"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Liste des clients trouvée"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de trouver la liste"));

            // -- Create une nouvelle ligne dans la table client --
            group.MapPost("/", CreerClient)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Rajouter un client",
                    description: "Rajoute un client dans la base de donnée"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Client ajouté"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Erreur de l'ajout client"));

            // -- Update une ligne de la table client --
            group.MapPut("/{id}", MettreAJourClient)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Mettre à jour un client",
                    description: "Mets à jour les informations d'un client"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Informations modifiées"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Impossible de retrouver le client"));

            // -- Delete une ligne de la table client --
            group.MapDelete("/{id}", SupprimerClient)
                .WithMetadata(new SwaggerOperationAttribute(
                    summary: "Supprime un client",
                    description: "Permet de retirer un client de notre base de donnée"))
                .WithMetadata(new SwaggerResponseAttribute(200, "Client supprimé"))
                .WithMetadata(new SwaggerResponseAttribute(404, "Le Client n'existe pas"));

            return group;
        }

        static async Task<IResult> RecupererListeClient(ClientsDb db)
        {
            return TypedResults.Ok(await db.Clients.Select(x => new ClientsDTO(x)).ToArrayAsync());
        }

        static async Task<IResult> CreerClient(ClientsDTO clientsDTO, ClientsDb db)
        {
            var client = new Clients
            {
                Nom = clientsDTO.Nom,
                Prenom = clientsDTO.Prenom,
                Adresse = clientsDTO.Adresse,
                Phone = clientsDTO.Phone
            };

            db.Clients.Add(client);
            await db.SaveChangesAsync();

            clientsDTO = new ClientsDTO(client);

            return TypedResults.Created($"/clients/{client.Id}", clientsDTO);
        }

        static async Task<IResult> MettreAJourClient(int id, ClientsDTO clientDTO, ClientsDb db)
        {
            var client = await db.Clients.FindAsync(id);

            if (client is null) return TypedResults.NotFound();

            client.Nom = clientDTO.Nom;
            client.Prenom = clientDTO.Prenom;
            client.Adresse = clientDTO.Adresse;
            client.Phone = clientDTO.Phone;

            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        static async Task<IResult> SupprimerClient(int id, ClientsDb db)
        {
            if (await db.Clients.FindAsync(id) is Clients client)
            {
                db.Clients.Remove(client);
                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }   
}