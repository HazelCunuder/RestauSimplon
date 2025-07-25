﻿using RestauSimplon.GestionClient;
using System.ComponentModel.DataAnnotations.Schema;

public class Commande
{
    // -- Créer les propriétés de la table commande
    public int Id { get; set; }
    public decimal PrixTotal { get; set; }
    public bool StatutCommande { get; set; }
    public DateTime DateCommande { get; set; }

    [ForeignKey("IdClient")]
    public int IdClient { get; set; }

    public List<GroupCommande> lignes { get; set; } = new();
    
    // Constructeur
    public Commande()
    {
        // Date actuelle par défaut
        DateCommande = DateTime.Now;

        // Commande non terminée par défaut
        StatutCommande = false;
    }
}