//Endpoints para los empleados
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;

namespace PrestamosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class EmpleadosController : ControllerBase
    {
        private readonly PrestamosContext _context;
        public EmpleadosController(PrestamosContext context)
        {
            _context = context;
        }

//GET todos los empleados
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            return await _context.Empleados.ToListAsync();
        }

//POST crear un nuevo empleado en la base de datos
        [HttpPost]
        public async Task<ActionResult<Empleado>> CrearEmpleado(Empleado empleado)
        {
            if (string.IsNullOrWhiteSpace(empleado.Nombre))
            {
                return BadRequest("El nombre del empleado es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(empleado.Documento))
            {
                return BadRequest("El número de documento del empelado es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(empleado.Area))
            {
                return BadRequest("El área del empleado es obligatoria.");
            }
            if (string.IsNullOrWhiteSpace(empleado.Correo))
            {
                return BadRequest("El correo eléctronico del empleado es obligatorio.");
            }

            _context.Empleados.Add(empleado);
            await _context.SaveChangesAsync();
            return StatusCode(201, empleado);
        }
    }
}