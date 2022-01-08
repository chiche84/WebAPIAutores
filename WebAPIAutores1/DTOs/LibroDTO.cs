using WebAPIAutores1.Entidades;

namespace WebAPIAutores1.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }
    }
}
