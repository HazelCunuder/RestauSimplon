using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swashbuckle.AspNetCore.Annotations;

public class CommandeItemDTO
{
    [SwaggerSchema("Identifiant unique de la commande")]
    public int Id { get; set; }

    [SwaggerSchema("Montant total calculé automatiquement")]
    public decimal PrixTotal { get; set; }

    [SwaggerSchema("Statut de la commande (false = en cours, true = terminée)")]
    public bool StatutCommande { get; set; }

    [SwaggerSchema("Date et heure de création de la commande")]
    public DateTime DateCommande { get; set; }

    [SwaggerSchema("Liste des prix des articles (utilisé uniquement en création de commande)")]
    public List<decimal>? PrixArticles { get; set; }  // ajout temporaire pour calcul prix

    public CommandeItemDTO() { }

    public CommandeItemDTO(Commande commandeItem) =>
        (Id, PrixTotal, StatutCommande, DateCommande) =
        (commandeItem.Id, commandeItem.PrixTotal, commandeItem.StatutCommande, commandeItem.DateCommande);
}
