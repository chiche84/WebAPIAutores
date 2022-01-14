namespace WebAPIAutores1.DTOs
{
    public class ColecciondeRecursos<T>: Recurso where T : Recurso // la clase generica T va a tener q heredar (implementar) recurso
    {
        public List<T> Valores { get; set; }
    }
}
