namespace WebAPIAutores1.DTOs
{
    public class DatoHATEOAS
    {
        public string Enlace { get; private set; }
        public string Descripcion { get; private set; }
        public string Metodo { get; private set; } //los hago private para crear una instancia de esta clase y no poder modificarla

        public DatoHATEOAS(string enlace, string descripcion, string metodo)
        {
            Enlace = enlace;
            Descripcion = descripcion;
            Metodo = metodo;
        }
    }
}
