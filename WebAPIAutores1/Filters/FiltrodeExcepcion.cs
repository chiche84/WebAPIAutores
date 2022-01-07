using Microsoft.AspNetCore.Mvc.Filters;

namespace EscueladeDanzasCore.Filters
{
    public class FiltrodeExcepcion: ExceptionFilterAttribute
    {
        private readonly ILogger<FiltrodeExcepcion> logger;

        public FiltrodeExcepcion(ILogger<FiltrodeExcepcion> logger)
        {
            this.logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);
        }
    }
}
