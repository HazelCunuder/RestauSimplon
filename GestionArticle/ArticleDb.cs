using Microsoft.EntityFrameworkCore;
using RestauSimplon.GestionArticle;

namespace RestauSimplon.GestionArticle
{

    public class ArticleDb : DbContext
    {
        public ArticleDb(DbContextOptions<ArticleDb> options) 
        : base(options) { }

        public DbSet<Article> Articles { get; set; }
    }
}
