namespace RestauSimplon.GestionClient
{
    public class Clients
    {
        // -- Créer les propriétés de la table Clients --
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string? Adresse { get; set; }
        public string? Phone { get; set; }
        public string? Secret { get; set; }
    }
}
