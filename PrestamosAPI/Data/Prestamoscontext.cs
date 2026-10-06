using Microsoft.EntityFrameworkCore;
using PrestamosAPI.Models;

namespace PrestamosAPI.data
{
    public class PrestamosContext : DbContext
    {
        public PrestamosContext(DbContextOptions<PrestamosContext> options) : base (options)
        {
        }
        public DbSet<Equipo> Equipos { get; set; }

    }
    
}