using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PrestamosAPI.Models
{
    public class Prestamo
    {
        [Key]
        public int Id { get; set; }

        [Column("empleado_id")]
        public int EmpleadoId { get; set; }

        [Column("equipo_id")]
        public int EquipoId { get; set; }
        
        [Column ("fecha_prestamo")]
        public DateTime FechaPrestamo { get; set; }

        [Column("fecha_devolucion")]
        public DateTime? FechaDevolucion { get; set; }

        [Required]
        public string Estado { get; set; } = string.Empty;
    }
}