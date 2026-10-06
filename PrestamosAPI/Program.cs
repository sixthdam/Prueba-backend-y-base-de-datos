using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PrestamosContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("PrestamosConnection"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("PrestamosConnection"))));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

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

//Endpoints para los empleados

//GET todos los empleados
app.MapGet("/api/empleados", async (PrestamosContext db) =>
{
    return await db.Empleados.ToListAsync();
});

app.MapPost("/api/empleados", async (Empleado empleado, PrestamosContext db) =>
{
    if (string.IsNullOrWhiteSpace(empleado.Nombre))
    {
        return Results.BadRequest("El nombre del empleado es obligatorio.");
    }
    if (string.IsNullOrWhiteSpace(empleado.Documento))
    {
        return Results.BadRequest("El número de documento del empleado es obligatorio.");
    }
    if (string.IsNullOrWhiteSpace(empleado.Area))
    {
        return Results.BadRequest("El área del empleado es obligatoria.");
    }
    if (string.IsNullOrWhiteSpace(empleado.Correo))
    {
        return Results.BadRequest("El correo electrónico del empleado es obligatorio.");
    }

    db.Empleados.Add(empleado);
    await db.SaveChangesAsync();
    return Results.Created($"/api/empleados/{empleado.Id}", empleado);
});

app.Run();