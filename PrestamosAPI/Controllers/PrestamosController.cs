//Endpoints para los prestamos

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;
using PrestamosAPI.Services;

namespace PrestamosAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]

    public class PrestamosController : ControllerBase
    {
        private readonly PrestamosContext _context;
        private readonly PrestamoService _service;

        public PrestamosController(PrestamosContext context, PrestamoService service)
        {
            _context = context;
            _service = service;
        }

//POST crear un nuevo prestamo en la base de datos
        [HttpPost]
        public async Task<ActionResult<Prestamo>> CrearPrestamo(PrestamoRequest request)
        {
            var resultado = await _service.CrearPrestamo(request);
            if (resultado.Prestamo is not null)
            {
                return Ok(resultado.Prestamo);
            }
            if (resultado.Error!.Contains("empleado no existe."))
            {
                return NotFound(resultado.Error);
            }
            if (resultado.Error!.Contains("equipo no existe."))
            {
                return NotFound(resultado.Error);
            }
            return BadRequest(resultado.Error);
        }

//PUT devolver un equipo prestado y actualizar el estado del prestamo y del equipo
        [HttpPut("{id}/devolver")]
        public async Task<ActionResult<Prestamo>> DevolverPrestamo(int id)
        {
            var resultado = await _service.DevolverPrestamo(id);
            if (resultado.Prestamo is not null)
            {
                return Ok(resultado.Prestamo);
            }
            if (resultado.Error!.Contains("préstamo no existe"))
            {
                return NotFound(resultado.Error);
            }
            if (resultado.Error!.Contains("equipo asociado"))
            {
                return NotFound(resultado.Prestamo);
            }
            return BadRequest(resultado.Error);
        }

//GET mostrar todos los prestamos
        [HttpGet("activos")]
        public async Task<ActionResult<IEnumerable<Prestamo>>> GetPrestamosActivos()
        {
            var resultado = await _service.GetPrestamosActivos();
            return Ok(resultado);
        }
    }
}