using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.GestionClient
{
    public class GroupCommandeDTO
    {
        [SwaggerSchema("Id du regroupement")]
        public int Id { get; set; }

        [SwaggerSchema("Identifiant du plat")]
        public int IdCommande { get; set; }

        [SwaggerSchema("Identifiant du plat")]
        public int IdPlat { get; set; }

        [SwaggerSchema("Quantité commandée")]
        public int Quantite { get; set; }

        public GroupCommandeDTO() { }
        public GroupCommandeDTO(GroupCommande groupe) =>
            (Id, IdCommande, IdPlat, Quantite) = (groupe.Id, groupe.IdCommande, groupe.IdPlat, groupe.Quantite);
    }
}
