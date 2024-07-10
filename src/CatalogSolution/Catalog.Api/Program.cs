using Catalog.Api.Catalog;
using Marten;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFeatureManagement();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("data")
    ?? throw new Exception("Couldn't find connection string in environment. Bailing");

builder.Services.AddMarten(config =>
{
    config.Connection(connectionString);
}).UseLightweightSessions();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Todo: Talk about Feature Flags Tomorrow.
if (await app.Services.GetRequiredService<IFeatureManager>().IsEnabledAsync("Catalog"))
{
    app.MapCatalog();
}
app.Run();

public partial class Program { }