# API RestauSimplon

## Table des matières

- [Présentation du projet](#présentation-du-projet)
- [Objectif](#objectif)
- [Technologies utilisées](#technologies-utilisées)
- [Fonctionnalités](#fonctionnalités)
- [Configuration](#configuration)
- [Installation](#installation)
- [Utilisation](#utilisation)
- [Endpoints de l'API](#endpoints-de-lapi)
- [Crédits](#crédits)

---

## Présentation du projet

Dans le cadre de notre formation Développeur C#/.NET à SIMPLON HdF, nous avons eu pour tâche de créer une API pour le restaurant RestauSimplon. Ce dernier a besoin d'une API pour simplifier la gestion de ses commandes et de ses clients.

## Objectif

L'objectif principal de cette API est de **digitaliser** la gestion des commandes du restaurant. L'API doit permettre au restaurant de :

- Gérer les articles du menu (ajout, modification, suppression).
- Gérer les clients (enregistrer leurs informations, consulter l'historique de leurs commandes).
- Suivre les commandes (création, consultation par date ou client, calcul des montants, mise à jour des statuts).

## Technologies utilisées

- **C#** : Langage de programmation principal pour le développement de l'API.
- **ASP.NET Core** : Framework utilisé pour construire des applications web et des API robustes et performantes.
- **SQLite** : Base de données légère et embarquée, idéale pour les projets de petite à moyenne envergure.
- **Swagger** : Outil de visualisation et d'interaction avec les endpoints de l'API, facilitant la documentation et les tests.

## Fonctionnalités

L'API RestauSimplon offre les fonctionnalités suivantes :

- **Gestion des articles** : CRUD (Créer, Lire, Mettre à jour, Supprimer) des articles du menu (nom, description, prix, catégorie).
- **Gestion des clients** : Enregistrement et consultation des informations client (nom, prénom, coordonnées).
- **Gestion des commandes** :
    - Création de nouvelles commandes avec association à un client et ajout d'articles.
    - Consultation des commandes par date ou par client.
    - Calcul automatique du montant total d'une commande.
    - Mise à jour du statut des commandes (en préparation, prête, livrée, annulée).

## Configuration

Pour configurer et exécuter l'API, suivez les étapes ci-dessous.

1.  **Prérequis** : Assurez-vous d'avoir le SDK .NET 8.0 (ou version supérieure) installé sur votre machine.
2.  **Clonage du dépôt** : Clonez le dépôt GitHub de l'API sur votre machine locale :
    
    ```bash
    git clone [https://github.com/votre-utilisateur/RestauSimplonAPI.git](https://github.com/votre-utilisateur/RestauSimplonAPI.git)
    cd RestauSimplonAPI
    ```
 
3.  **Base de données** : SQLite ne nécessite pas de serveur de base de données externe. Le fichier de base de données sera créé automatiquement lors de la première exécution de l'API, ou lors des migrations Entity Framework Core.
    
    *Si vous utilisez Entity Framework Core Migrations, vous devrez peut-être exécuter les commandes suivantes pour créer ou mettre à jour la base de données \:*
    
    ```bash
    dotnet ef database update
    ```
 
    *Assurez-vous d'avoir les outils Entity Framework Core installés \:*
    
    ```bash
    dotnet tool install --global dotnet-ef
    ```

## Installation

Pour installer les dépendances du projet :

```bash
dotnet restore
```

## Utilisation

Pour démarrer l'API, exécutez la commande suivante depuis le répertoire racine du projet :

```bash
dotnet run
```

L'API sera accessible par défaut sur https://localhost:7000 (le port peut varier selon votre configuration ou si un autre processus utilise ce port).

## Crédits

Ce projet a été réalisé dans le cadre de la formation Développeur C#/.NET chez SIMPLON Hauts-de-France.
Équipe projet :

- Amine BENFETTA

- Hazel CUNUDER

- Vincent FAIVRE

Merci à l’équipe pédagogique de SIMPLON et à nos formateurs Alexandre LINE et Benjamin QUINET pour leur accompagnement.
