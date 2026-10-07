using System.Timers;
using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;

namespace PrestamosAPI.Services
{
    public class PrestamoService
    {
        private readonly PrestamosContext _context;
        
        public PrestamoService(PrestamosContext context)
        {
            _context = context;
        }

//Service para POST de nuevo préstamo
        public async Task<(Prestamo? Prestamo, string? Error)> CrearPrestamo(PrestamoRequest request)
        {
            var empleado = await _context.Empleados.FindAsync(request.EmpleadoId);
            if (empleado is null)
            {
                return (null, "El empleado no existe, favor de verificar el ID proporcionado, e intente nuevamente.");
            }
            var equipo =await _context.Equipos.FindAsync(request.EquipoId);
            if (equipo is null)
            {
                return (null, "El equipo no existe, favor de verificar el ID proporcionado e intente nuevamente.");
            }
            if (equipo.Estado != "Disponible")
            {
                return (null, "El equipo no se encuentra disponible para préstamo.");
            }
            var prestamo = new Prestamo
            {
                EmpleadoId = request.EmpleadoId,
                EquipoId = request.EquipoId,
                FechaPrestamo = DateTime.Now,
                Estado = "Activo"
            };

            equipo.Estado = "Prestado";
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
            return (prestamo, null);
        }

//Service para PUT devolver equipo y actualizar el estado del préstamo y el equipo correspondiente.
        public async Task<(Prestamo? Prestamo, string? Error)> DevolverPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo is null)
            {
                return (null, "El préstamo no existe, favor de verificar el ID proporcionado e intente nuevamente.");
            }
            if (prestamo.Estado == "Devuelto")
            {
                return (null, "El equipo ya fue devuelto");
            }
            var equipo = await _context.Equipos.FindAsync(prestamo.EquipoId);
            if (equipo is null)
            {
                return (null, "El equipo asociado al préstamo no existe, favor de verificar el ID proporcionado e intente nuevamente.");
            }
            prestamo.FechaDevolucion = DateTime.Now;
            prestamo.Estado = "Devuelto";
            equipo.Estado = "Disponible";
            await _context.SaveChangesAsync();

            return (prestamo, null);
        }

//Service para GET mostrar todos los préstamos activos
        public async Task<IEnumerable<object>> GetPrestamosActivos()
        {
            var PrestamosActivos = await (
                from prestamo in _context.Prestamos
                join equipo in _context.Equipos
                    on prestamo.EquipoId equals equipo.Id
                join empleado in _context.Empleados
                    on prestamo.EmpleadoId equals empleado.Id
                where prestamo.Estado == "Activo"
                select new
                {
                    prestamo.Id,
                    Equipo = equipo.Nombre,
                    Empleado = empleado.Nombre,
                    prestamo.FechaPrestamo,
                    prestamo.Estado
                }).ToListAsync();
                return PrestamosActivos;
        }
    }
}