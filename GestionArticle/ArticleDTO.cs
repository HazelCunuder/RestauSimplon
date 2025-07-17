using Swashbuckle.AspNetCore.Annotations;

namespace RestauSimplon.GestionArticle
{
    public class ArticleDTO
    {
        [SwaggerSchema("Id de l'article")]
        public int Id { get; set; }

        [SwaggerSchema("Nom du plat")]
        public string Nom { get; set; }

        [SwaggerSchema("Categorie du plat")]
        public string Categorie { get; set; }

        [SwaggerSchema("Prix du plat")]
        public double Prix { get; set; }

        public ArticleDTO() { }
        public ArticleDTO(Article Article) =>
            (Id, Nom, Categorie, Prix) = (Article.Id, Article.Nom, Article.Categorie, Article.Prix);
    }
}
