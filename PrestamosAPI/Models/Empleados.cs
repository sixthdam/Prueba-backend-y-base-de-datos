using System.ComponentModel.DataAnnotations;

namespace PrestamosAPI.Models
{
    public class Empleado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Documento { get; set; } = string.Empty;

        [Required]
        public string Area { get; set; } = string.Empty;

        [Required]
        public string Correo { get; set; } = string.Empty;
    }
}