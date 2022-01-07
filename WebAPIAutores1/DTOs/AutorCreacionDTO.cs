using EscueladeDanzasCore.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores1.DTOs
{
    public class AutorCreacionDTO
    {       
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(120)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
