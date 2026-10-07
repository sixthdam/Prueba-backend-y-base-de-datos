using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;
using PrestamosAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PrestamosContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("PrestamosConnection"),
ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("PrestamosConnection"))));
builder.Services.AddControllers();
builder.Services.AddScoped<PrestamoService>();
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

app.Run();