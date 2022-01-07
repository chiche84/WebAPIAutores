using EscueladeDanzasCore.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores1.Entidades
{
    public class Autor
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="El campo {0} es requerido")]
        [StringLength(120)]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }

        public List<Libro> Libros { get; set; }
    }
}
