using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MyPantry.Core.Interfaces;
using MyPantry.Infrastructure.Data;
using MyPantry.Infrastructure.Repositories;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
    if (string.IsNullOrWhiteSpace(dbPassword))
    {
        throw new InvalidOperationException(
            "Database password is not configured. Set DB_PASSWORD or ConnectionStrings:DefaultConnection.");
    }

    var trustServerCertificate = Environment.GetEnvironmentVariable("DB_TRUST_SERVER_CERTIFICATE") ?? "True";
    if (!bool.TryParse(trustServerCertificate, out var trustCertificate))
    {
        throw new InvalidOperationException("DB_TRUST_SERVER_CERTIFICATE must be True or False.");
    }

    connectionString = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder
    {
        DataSource = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost,1433",
        InitialCatalog = Environment.GetEnvironmentVariable("DB_NAME") ?? "MyPantryDb",
        UserID = Environment.GetEnvironmentVariable("DB_USER") ?? "sa",
        Password = dbPassword,
        TrustServerCertificate = trustCertificate
    }.ConnectionString;
}

builder.Services.AddDbContext<MyPantryDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyPantryDbContext>();
    db.Database.Migrate();
}

app.Run();
