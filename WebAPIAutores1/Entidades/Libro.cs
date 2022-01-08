using EscueladeDanzasCore.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores1.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [StringLength(250)]
        [PrimeraLetraMayuscula]
        [Required]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }

        public List<AutorLibro> AutoresLibros { get; set; }
    }
}
