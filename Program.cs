using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using RestauSimplon.Data_Existante;
using Swashbuckle.AspNetCore.Annotations;
using System.Numerics;
using RestauSimplon.GestionArticle;
using RestauSimplon.Endpoints;

var builder = WebApplication.CreateBuilder(args);
// -- Ajout de la BDD SQLite avec les infos de clients --
builder.Services.AddDbContext<ArticleDb>(opt => opt.UseSqlite("Data Source=ClientsDb.db"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "RestauSimplon",
        Version = "v1",
        Description = "Une API pour gerer les clients et commandes du restaurant",
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

// -- Appelle la methode permettant de generer les endpoints de /clients --
app.MapGroup("/clients").MapArticleEndpoints();

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
