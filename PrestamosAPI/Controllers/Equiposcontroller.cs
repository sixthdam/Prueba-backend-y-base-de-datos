//Endpoints para los equipos

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrestamosAPI.data;
using PrestamosAPI.Models;

namespace PrestamosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquiposController : ControllerBase
    {
        private readonly PrestamosContext _context;

        public EquiposController(PrestamosContext context)
        {
            _context = context;
        }

//GET todos los equipos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipo>>> GetEquipos()
        {
            return await _context.Equipos.ToListAsync();
        }

//GET un equipo por id
        [HttpGet("{id}")]
        public async Task<ActionResult<Equipo>> GetEquipo(int id)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if(equipo is null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

//POST un nuevo equipo en la base de datos
        [HttpPost]
        public async Task<ActionResult<Equipo>> CrearEquipo(Equipo equipo)
        {
            if (string.IsNullOrWhiteSpace(equipo.Nombre))
            {
                return BadRequest("El nombre del equipo es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(equipo.Serial))
            {
                return BadRequest("El número de serie del equipo es obligatorio.");
            }
            var estadosValidos = new []
            {
                "Disponible",
                "Prestado",
                "En Mantenimiento"
            };

            if (!estadosValidos.Contains(equipo.Estado))
            {
                return BadRequest("El estado del equipo sólo puede ser 'Disponible', 'Prestado', o 'En Mantenimiento'.");
            }

            _context.Equipos.Add(equipo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEquipo), new { id = equipo.Id }, equipo);
        }       

//PUT actualizar un equipo existente
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarEquipo(int id, Equipo equipoActualizado)
        {
            var equipo = await _context.Equipos.FindAsync(id);
            if (equipo is null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(equipoActualizado.Nombre))
            {
                return BadRequest("El nombre del equipo es obligatorio.");
            }
            if (string.IsNullOrWhiteSpace(equipoActualizado.Serial))
            {
                return BadRequest("El número de serie del equipo es obligatorio.");
            }
            var estadosValidos = new []
            {
                "Disponible",
                "Prestado",
                "En Mantenimiento"
            };
            if (!estadosValidos.Contains(equipoActualizado.Estado))
            {
                return BadRequest("El estado del equipo sólo puede ser 'Disponible', 'Prestado', o 'En Mantenimiento'.");
            }

            equipo.Nombre = equipoActualizado.Nombre;
            equipo.Serial = equipoActualizado.Serial;
            equipo.Estado = equipoActualizado.Estado;
            equipo.CategoriaId = equipoActualizado.CategoriaId;

            await _context.SaveChangesAsync();
            return Ok(equipo);
        }
    }
}