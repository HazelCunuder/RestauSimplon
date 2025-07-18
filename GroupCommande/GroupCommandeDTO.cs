using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RestauSimplon.GestionClient
{
    public class GroupCommandeDTO
    {
        [SwaggerSchema("Id du regroupement")]
        public int Id { get; set; }

        [SwaggerSchema("Identifiant de la commande")]
        [Required(ErrorMessage = "L'identifiant de la commande est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "L'identifiant de la commande doit être un entier positif.")]
        public int IdCommande { get; set; }

        [SwaggerSchema("Identifiant du plat")]
        [Required(ErrorMessage = "L'identifiant du plat est requis.")]
        [Range(1, int.MaxValue, ErrorMessage = "L'identifiant du plat doit être un entier positif.")]
        public int IdPlat { get; set; }

        [SwaggerSchema("Quantité commandée")]
        [Required(ErrorMessage = "La quantité est requise.")]
        [Range(1, 100, ErrorMessage = "La quantité doit être comprise entre 1 et 100.")]
        public int Quantite { get; set; }

        public GroupCommandeDTO() { }
        public GroupCommandeDTO(GroupCommande groupe) =>
            (Id, IdCommande, IdPlat, Quantite) = (groupe.Id, groupe.IdCommande, groupe.IdPlat, groupe.Quantite);
    }
}
