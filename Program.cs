using Scalar.AspNetCore;
using Veterinaria_API;
using Veterinaria_API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<VeterinariaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VeterinariaConnection")));
var app = builder.Build();
/*builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // el origen de tu futuro frontend
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});*/
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();
//app.UseCors("PermitirFrontend");
app.MapControllers();

app.MapGet("/api/minimal/mascotas", () => {
    return Results.Ok(BaseDeDatos.mascotas);
});
app.MapGet("/api/minimal/mascotas/{id:int}", (int id) =>
{
    var mascota = BaseDeDatos.mascotas.FirstOrDefault(x => x.Id == id);
    return mascota is null ? Results.NotFound() : Results.Ok(mascota);
})
.WithName("GetMascotaMinimal");
app.MapPost("/api/minimal/mascotas", (Mascota mascota) =>
{
    mascota.Id = BaseDeDatos.mascotas.Any() ? BaseDeDatos.mascotas.Max(x => x.Id) + 1 : 1;
    BaseDeDatos.mascotas.Add(mascota);
    return Results.CreatedAtRoute("GetMascotaMinimal", new { id = mascota.Id}, mascota);
});

app.Run();

