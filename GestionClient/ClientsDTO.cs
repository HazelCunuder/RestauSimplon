using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.GestionClient
{
    public class ClientsDTO
    {
        [SwaggerSchema("Id du client")]
        public int Id { get; set; }

        [SwaggerSchema("Nom de Famille du client")]
        public string Nom { get; set; }

        [SwaggerSchema("Prenom du client")]
        public string Prenom { get; set; }

        [SwaggerSchema("Adresse du client")]
        public string? Adresse { get; set; }

        [SwaggerSchema("Numéro de téléphone du client")]
        public string? Phone { get; set; }

        public ClientsDTO() { }
        public ClientsDTO(Clients clients) =>
            (Id, Nom, Prenom, Adresse, Phone) = (clients.Id, clients.Nom, clients.Prenom, clients.Adresse, clients.Phone);
    }
}
