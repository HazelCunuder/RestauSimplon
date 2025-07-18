using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestauSimplon.Data_Existante;
using RestauSimplon.Endpoints;
using RestauSimplon.GestionArticle;
using RestauSimplon.GestionClient;

namespace RestauSimplon
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -- Ajout de la BDD SQLite avec les infos de clients --
            builder.Services.AddDbContext<ClientsDb>(opt => opt.UseSqlite("Data Source=ClientsDb.db"));

            builder.Services.AddDbContext<GroupCommandeDb>(opt => opt.UseSqlite("Data Source=GroupCommandeDb.db"));

            builder.Services.AddDbContext<ArticleDb>(opt => opt.UseSqlite("Data Source=ArticleDb.db"));

            builder.Services.AddDbContext<CommandeDb>(opt => opt.UseSqlite("Data Source=CommandeDb.db"));

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RestauSimplon",
                    Version = "v1",
                    Description = "Une API pour gérer les clients et commandes du restaurant",
                    Contact = new OpenApiContact
                    {
                        Name = "Groupe Best",
                        Email = "lesbest@gmail.com"
                    }
                });

                c.EnableAnnotations();
            });

            var app = builder.Build();

// -- Ajout de plats.json dans la BDD --
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ArticleDb>();
    Task.Run(() => ArticleSeeder.SeedAsync(db)).Wait();
}
            // -- Ajout de clients.json dans la BDD --
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ClientsDb>();
                Task.Run(() => ClientSeeder.SeedAsync(db)).Wait();
            }

            // -- Appelle la méthode permettant de générer les endpoints de /clients --
            app.MapGroup("/clients").MapClientsEndpoints();

            // -- Appelle la methode permettant de generer les endpoints de /articles --
            app.MapGroup("/articles").MapArticleEndpoints();

            // -- Appelle la méthode permettant de générer les endpoints de /commandes --
            app.MapGroup("/commandes").MapCommandeEndpoints();

            // -- Appelle la méthode permettant de générer les endpoints de /groupes-commandes --
            app.MapGroup("/groupe-commandes").MapGroupEndpoints();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RestauSimplon v1");
                    c.RoutePrefix = "";
                });
            }


            app.Run();

        }
    }
}
