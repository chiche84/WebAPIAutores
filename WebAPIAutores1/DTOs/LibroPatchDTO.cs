using EscueladeDanzasCore.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores1.DTOs
{
    public class LibroPatchDTO
    {
        [StringLength(250)]
        [PrimeraLetraMayuscula]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
