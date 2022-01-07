using Microsoft.AspNetCore.Mvc.Filters;

namespace EscueladeDanzasCore.Filters
{
    public class MiFiltrodeAcccion : IActionFilter
    {
        private readonly ILogger<MiFiltrodeAcccion> logger;

        public MiFiltrodeAcccion(ILogger<MiFiltrodeAcccion> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar la accion");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de ejecutar la accion");
        }

    }
}
