using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PrestamosContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("PrestamosConnection"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("PrestamosConnection"))));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//GET todos los equipos
app.MapGet("/api/equipos", async (PrestamosContext db) =>
{
    return await db.Equipos.ToListAsync();
});

//GET un equipo por id
app.MapGet("/api/equipos/{id}", async (int id, PrestamosContext db) =>
{
    var equipo = await db.Equipos.FindAsync(id);
    if (equipo is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(equipo);
});

//POST un nuevo equipo en la base de datos
app.MapPost("/api/equipos", async (Equipo equipo, PrestamosContext db) =>
{
    if (string.IsNullOrWhiteSpace(equipo.Nombre))
    {
        return Results.BadRequest("El nombre del equipo es obligatorio.");
    }
    if (string.IsNullOrWhiteSpace(equipo.Serial))
    {
        return Results.BadRequest("El número de serie del equipo es obligatorio.");
    }
    var estadosValidos = new []
    {
        "Disponible",
        "Prestado",
        "En Mantenimiento"
    };
    if (!estadosValidos.Contains(equipo.Estado))
    {
        return Results.BadRequest("El estado del equipo sólo puede ser 'Disponible', 'Prestado', o 'En Mantenimiento'.");
    }

    db.Equipos.Add(equipo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/equipos/{equipo.Id}", equipo);
});

//PUT actualizar un equipo existente
app.MapPut("/api/equipos/{id}", async (int id, Equipo equipoActualizado, PrestamosContext db) =>
{
    var equipo = await db.Equipos.FindAsync(id);
    if (equipo is null)
    {
        return Results.NotFound();
    }
    if (string.IsNullOrWhiteSpace(equipoActualizado.Nombre))
    {
        return Results.BadRequest("El nombre del equipo es obligatorio.");
    }
    if (string.IsNullOrWhiteSpace(equipoActualizado.Serial))
    {
        return Results.BadRequest("El número de serie del equipo es obligatorio.");
    }
    var estadosValidos = new []
    {
        "Disponible",
        "Prestado",
        "En Mantenimiento"
    };
    if (!estadosValidos.Contains(equipoActualizado.Estado))
    {
        return Results.BadRequest("El estado del equipo sólo puede ser 'Disponible', 'Prestado', o 'En Mantenimiento'.");
    }

    equipo.Nombre = equipoActualizado.Nombre;
    equipo.Serial = equipoActualizado.Serial;
    equipo.Estado = equipoActualizado.Estado;
    equipo.CategoriaId = equipoActualizado.CategoriaId;

    await db.SaveChangesAsync();
    return Results.Ok(equipo);
});

//DELETE eliminar un equipo existente
app.MapDelete("api/equipos/{id}", async (int id, PrestamosContext db) =>
{
    var equipo = await db.Equipos.FindAsync(id);
    if (equipo is null)
    {
        return Results.NotFound();
    }
    db.Equipos.Remove(equipo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();