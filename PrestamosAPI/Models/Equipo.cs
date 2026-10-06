using System.ComponentModel.DataAnnotations;
using System.ComponentModel .DataAnnotations.Schema;

namespace PrestamosAPI.Models
{
    public class Equipo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Serial { get; set; } = string.Empty;

        [Required]
        public string Estado {get; set; } = string.Empty;

        [Column("categoria_id")]
        public int CategoriaId { get; set; }
    }
}