using EscueladeDanzasCore.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace WebAPIAutores1.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [StringLength(250)]
        [PrimeraLetraMayuscula]
        public string Titulo { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
        public List<Comentario> Comentarios { get; set; }
    }
}
