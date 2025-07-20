using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RestauSimplon.GestionClient
{
    public class ClientsDTO
    {
        [SwaggerSchema("Id du client")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Un nom de famille est requis.")]
        [MinLength(2, ErrorMessage = "Le nom doit contenir au moins 2 caractères.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le nom de famille doit contenir uniquement des lettres")]
        [SwaggerSchema("Nom de Famille du client")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "Un prénom est requis.")]
        [MinLength(2, ErrorMessage = "Le prénom doit contenir au moins 2 caractères.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Le prénom doit contenir uniquement des lettres")]
        [SwaggerSchema("Prenom du client")]
        public string Prenom { get; set; }

        [SwaggerSchema("Adresse du client")]
        [Required(ErrorMessage = "Une adresse est requise.")]
        [RegularExpression(@"^\d{1,4}\s?[a-zA-ZÀ-ÿ\s\.'\-]+$", ErrorMessage = "L'adresse doit commencer par un numéro suivi d'une rue valide.")]
        public string? Adresse { get; set; }

        [SwaggerSchema("Numéro de téléphone du client")]
        [Required(ErrorMessage = "Un numéro de telephone est requis.")]
        [RegularExpression(@"^0[1-9]([ .-]?\d{2}){4}$", ErrorMessage = "Le numéro doit être au format français (ex : 06 12 34 56 78).")]
        public string? Phone { get; set; }

        public ClientsDTO() { }
        public ClientsDTO(Clients clients) =>
            (Id, Nom, Prenom, Adresse, Phone) = (clients.Id, clients.Nom, clients.Prenom, clients.Adresse, clients.Phone);
    }
}
