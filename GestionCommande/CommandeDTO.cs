using Microsoft.EntityFrameworkCore.ChangeTracking;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

public class CommandeItemDTO
{
    [SwaggerSchema("Identifiant unique de la commande")]
    public int Id { get; set; }
    
    [SwaggerSchema("Montant total calculé automatiquement")]
    [Required(ErrorMessage = "Un montant est requis.")]
    [Range(0.02, 10000, ErrorMessage = "Le montant doit être compris entre 0.02 et 10000.")]
    [RegularExpression(@"^\d+([.,]\d{1,2})?$", ErrorMessage = "Le prix doit être un nombre positif avec jusqu'à deux décimales.")]
    public decimal PrixTotal { get; set; }

    [SwaggerSchema("Statut de la commande (false = en cours, true = terminée)")]
    public bool StatutCommande { get; set; }

    [SwaggerSchema("Date et heure de création de la commande")]
    [Required(ErrorMessage = "La date de commande est obligatoire.")]
    public DateTime DateCommande { get; set; }

    [SwaggerSchema("Liste des prix des articles (utilisé uniquement en création de commande)")]
    public List<decimal>? PrixArticles { get; set; }  // ajout temporaire pour calcul prix

    public CommandeItemDTO() { }

    public CommandeItemDTO(Commande commandeItem) =>
        (Id, PrixTotal, StatutCommande, DateCommande) =
        (commandeItem.Id, commandeItem.PrixTotal, commandeItem.StatutCommande, commandeItem.DateCommande);
}
