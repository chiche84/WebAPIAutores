using Microsoft.AspNetCore.Identity;

namespace WebAPIAutores1.Entidades
{
    public class Comentario
    {
        public int Id { get; set; } 
        public string Contenido { get; set; }
        public int LibroId { get; set; }
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }
    }   
}
