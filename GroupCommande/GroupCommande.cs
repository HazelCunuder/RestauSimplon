using System.ComponentModel.DataAnnotations.Schema;

namespace RestauSimplon.GestionClient
{
    public class GroupCommande
    {
        // -- Créer les propriétés de la table Clients --
        public int Id { get; set; }

        [ForeignKey("Commande")]
        public int IdCommande { get; set; }

        [ForeignKey("Plat")]
        public int IdPlat { get; set; }

        public int Quantite { get; set; }
        public string? Secret { get; set; }
    }
}
