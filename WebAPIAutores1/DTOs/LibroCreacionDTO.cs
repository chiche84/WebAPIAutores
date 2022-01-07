using EscueladeDanzasCore.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores1.DTOs
{
    public class LibroCreacionDTO
    {
        [StringLength(250)]
        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public int AutorId { get; set; }
    }
}
