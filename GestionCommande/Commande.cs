public class Commande
{

    // -- Créer les propriétés de la table commande
    public int Id { get; set; }
    public decimal PrixTotal { get; set; }
    public bool StatutCommande { get; set; }
    public DateTime DateCommande { get; set; } 
    
    // Constructeur
    public Commande()
    {
        // Date actuelle par défaut
        DateCommande = DateTime.Now;

        // Commande non terminée par défaut
        StatutCommande = false;
    }
}