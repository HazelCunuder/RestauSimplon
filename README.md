---

# API RestauSimplon

## Table des mati�res

- [Pr�sentation du projet](#pr�sentation-du-projet)
- [Objectif](#objectif)
- [Technologies utilis�es](#technologies-utilis�es)
- [Fonctionnalit�s](#fonctionnalit�s)
- [Configuration](#configuration)
- [Installation](#installation)
- [Utilisation](#utilisation)
- [Endpoints de l'API](#endpoints-de-lapi)
- [Cr�dits](#cr�dits)

---

## Pr�sentation du projet

Dans le cadre de notre formation D�veloppeur C#/.NET � SIMPLON HdF, nous avons eu pour t�che de cr�er une API pour le restaurant RestauSimplon. Ce dernier a besoin d'une API pour simplifier la gestion de ses commandes et de ses clients.

## Objectif

L'objectif principal de cette API est de **digitaliser** la gestion des commandes du restaurant. L'API doit permettre au restaurant de :

- G�rer les articles du menu (ajout, modification, suppression).
- G�rer les clients (enregistrer leurs informations, consulter l'historique de leurs commandes).
- Suivre les commandes (cr�ation, consultation par date ou client, calcul des montants, mise � jour des statuts).

## Technologies utilis�es

- **C#** : Langage de programmation principal pour le d�veloppement de l'API.
- **ASP.NET Core** : Framework utilis� pour construire des applications web et des API robustes et performantes.
- **SQLite** : Base de donn�es l�g�re et embarqu�e, id�ale pour les projets de petite � moyenne envergure.
- **Swagger** : Outil de visualisation et d'interaction avec les endpoints de l'API, facilitant la documentation et les tests.

## Fonctionnalit�s

L'API RestauSimplon offre les fonctionnalit�s suivantes :

- **Gestion des articles** : CRUD (Cr�er, Lire, Mettre � jour, Supprimer) des articles du menu (nom, description, prix, cat�gorie).
- **Gestion des clients** : Enregistrement et consultation des informations client (nom, pr�nom, coordonn�es).
- **Gestion des commandes** :
    - Cr�ation de nouvelles commandes avec association � un client et ajout d'articles.
    - Consultation des commandes par date ou par client.
    - Calcul automatique du montant total d'une commande.
    - Mise � jour du statut des commandes (en pr�paration, pr�te, livr�e, annul�e).

## Configuration

Pour configurer et ex�cuter l'API, suivez les �tapes ci-dessous.

1.  **Pr�requis** : Assurez-vous d'avoir le SDK .NET 8.0 (ou version sup�rieure) install� sur votre machine.
2.  **Clonage du d�p�t** : Clonez le d�p�t GitHub de l'API sur votre machine locale :
    
    ```bash
    git clone [https://github.com/votre-utilisateur/RestauSimplonAPI.git](https://github.com/votre-utilisateur/RestauSimplonAPI.git)
    cd RestauSimplonAPI
    ```
 
3.  **Base de donn�es** : SQLite ne n�cessite pas de serveur de base de donn�es externe. Le fichier de base de donn�es sera cr�� automatiquement lors de la premi�re ex�cution de l'API, ou lors des migrations Entity Framework Core.
    
    *Si vous utilisez Entity Framework Core Migrations, vous devrez peut-�tre ex�cuter les commandes suivantes pour cr�er ou mettre � jour la base de donn�es \:*
    
    ```bash
    dotnet ef database update
    ```
 
    *Assurez-vous d'avoir les outils Entity Framework Core install�s \:*
    
    ```bash
    dotnet tool install --global dotnet-ef
    ```

## Installation

Pour installer les d�pendances du projet :

```bash
dotnet restore
```

## Utilisation

Pour d�marrer l'API, ex�cutez la commande suivante depuis le r�pertoire racine du projet :

```bash
dotnet run
```

L'API sera accessible par d�faut sur https://localhost:7000 (le port peut varier selon votre configuration ou si un autre processus utilise ce port).

## Cr�dits

Ce projet a �t� r�alis� dans le cadre de la formation D�veloppeur C#/.NET chez SIMPLON Hauts-de-France.
�quipe projet :

- Amine BENFETTA

- Hazel CUNUDER

- Vincent FAIVRE

Merci � l��quipe p�dagogique de SIMPLON et � nos formateurs Alexandre LINE et Benjamin QUINET pour leur accompagnement..
