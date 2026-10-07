//Endpoints para los prestamos

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;

namespace PrestamosAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]

    public class PrestamosController : ControllerBase
    {
        private readonly PrestamosContext _context;

        public PrestamosController(PrestamosContext context)
        {
            _context = context;
        }

//POST crear un nuevo prestamo en la base de datos
        [HttpPost]
        public async Task<ActionResult<Prestamo>> CrearPrestamo(PrestamoRequest request)
        {
            var empleado = await _context.Empleados.FindAsync(request.EmpleadoId);
            if (empleado is null)
            {
                return NotFound("El empleado no existe, favor de verificar el ID proporcionado e intente nuevamente.");
            } 
            var equipo = await _context.Equipos.FindAsync(request.EquipoId);
            if (equipo is null)
            {
                return NotFound("El equipo no existe, favor de verificar el ID proporcionado e intente nuevamente.");
            }
            if (equipo.Estado != "Disponible")
            {
                return BadRequest($"El equipo con ID {equipo.Id} no se encuentra disponible para préstamo. Estado actual: {equipo.Estado}");
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
            return Ok(prestamo);
        }

//PUT devolver un equipo prestado y actualizar el estado del prestamo y del equipo
        [HttpPut("{id}/devolver")]
        public async Task<ActionResult<Prestamo>> DevolverPrestamo(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo is null)
            {
                return NotFound("El préstamo no se encuentra registrado, favor de verificar el ID proporcionado e intente nuevamente.");
            }
            if (prestamo.Estado == "Devuelto")
            {
                return BadRequest("El equipo ya ha sido devuelto.");
            }
            var equipo = await _context.Equipos.FindAsync(prestamo.EquipoId);
            if (equipo is null)
            {
                return NotFound("El equipo asociado al préstamo no se encuentra registrado, favor de verificar el ID proporcionado e intente nuevamente.");
            }

            prestamo.FechaDevolucion = DateTime.Now;
            prestamo.Estado = "Devuelto";
            equipo.Estado = "Disponible";
            await _context.SaveChangesAsync();
            return Ok(prestamo);
        }

//GET mostrar todos los prestamos
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamos()
        {
            var prestamosActivos = await (
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

                return Ok(prestamosActivos);
        }
    }
}