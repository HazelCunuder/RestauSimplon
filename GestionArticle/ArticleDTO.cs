using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace RestauSimplon.GestionArticle
{
    public class ArticleDTO
    {
        [SwaggerSchema("Id de l'article")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Un nom de plat valide est requis.")]
        [MinLength(2, ErrorMessage = "Le nom doit contenir au moins 2 caractères.")]
        [SwaggerSchema("Nom du plat")]
        public string Nom { get; set; }
        
        [Required(ErrorMessage = "Une categorie valide est requise.")]
        [MinLength(2, ErrorMessage = "La categorie doit contenir au moins 2 caractères.")]
        [SwaggerSchema("Categorie du plat")]
        public string Categorie { get; set; }

        [Required(ErrorMessage = "Un prix est requis.")]
        [Range(0.01, 1000, ErrorMessage = "Le prix doit être compris entre 0.01 et 1000.")]
        [SwaggerSchema("Prix du plat")]
        public double Prix { get; set; }

        public ArticleDTO() { }
        public ArticleDTO(Article Article) =>
            (Id, Nom, Categorie, Prix) = (Article.Id, Article.Nom, Article.Categorie, Article.Prix);
    }
}
