using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RestauSimplon.Data_Existante;
using RestauSimplon.Endpoints;
using RestauSimplon.GestionClient;
using Swashbuckle.AspNetCore.Annotations;
using System.Numerics;

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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RestauSimplon",
                    Version = "v1",
                    Description = "Une API pour g�rer les clients et commandes du restaurant",
                    Contact = new OpenApiContact
                    {
                        Name = "Groupe Best",
                        Email = "lesbest@gmail.com"
                    }
                });

                c.EnableAnnotations();
            });

            var app = builder.Build();

            // -- Ajout de clients.json dans la BDD --
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ClientsDb>();
                Task.Run(() => ClientSeeder.SeedAsync(db)).Wait();
            }

            // -- Appelle la m�thode permettant de g�n�rer les endpoints de /clients --
            app.MapGroup("/clients").MapClientsEndpoints();

            // -- Appelle la m�thode permettant de g�n�rer les endpoints de /groupes-commandes --
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
