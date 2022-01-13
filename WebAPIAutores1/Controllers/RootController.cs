using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIAutores1.DTOs;

namespace WebAPIAutores1.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RootController : ControllerBase
    {
        private readonly IAuthorizationService authorizationService;

        public RootController(IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
        }
        [HttpGet(Name ="obtenerRoot")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DatoHATEOAS>>> Get()
        {
            var dateHATEOAS = new List<DatoHATEOAS>();
            var autorizacion = await authorizationService.AuthorizeAsync(User, "EsAdmin");

            if (autorizacion.Succeeded)
            {
                dateHATEOAS.Add(new DatoHATEOAS(Url.Link("crearAutor", new { }), "self",  "POST"));
                dateHATEOAS.Add(new DatoHATEOAS(Url.Link("crearLibro", new { }), "self",  "POST"));
            }
            dateHATEOAS.Add(new DatoHATEOAS(Url.Link("obtenerRoot", new { }), "self",  "GET"));
            dateHATEOAS.Add(new DatoHATEOAS(Url.Link("obtenerAutores", new { }), "self",  "GET"));
            return dateHATEOAS;
        }
    }
}
