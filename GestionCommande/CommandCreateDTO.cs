using RestauSimplon.GestionClient;
using RestauSimplon.GestionArticle;

namespace RestauSimplon.GestionCommande
{
    public class CommandCreateDTO
    {
        public int IdClient { get; set; }
        public int IdPlat { get; set; }
        public List<GroupCommandeDTO> Articles { get; set; } = new();
        }
}
