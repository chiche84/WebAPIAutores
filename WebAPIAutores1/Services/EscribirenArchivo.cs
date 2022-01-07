using System.IO;

namespace EscueladeDanzasCore.Services
{
    public class EscribirenArchivo : IHostedService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string nombreArchivo = "archivo1.txt";
        private Timer timer;
        public EscribirenArchivo(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
#pragma warning disable CS8622 // La nulabilidad de los tipos de referencia del tipo de parámetro no coincide con el delegado de destino (posiblemente debido a los atributos de nulabilidad).
            timer = new Timer(Hacer, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
#pragma warning restore CS8622 // La nulabilidad de los tipos de referencia del tipo de parámetro no coincide con el delegado de destino (posiblemente debido a los atributos de nulabilidad).
        }
        public Task StartAsync(CancellationToken cancellationToken)        {

            Escribir("Proceso Iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();    
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }

        public void Hacer(object state)
        {
            Escribir("Proceso ejecutado a las :" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        private void Escribir(string mensaje)
        {
            var ruta = $@"{webHostEnvironment.ContentRootPath}/wwwroot/{nombreArchivo}";

            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
