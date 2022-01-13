using Microsoft.AspNetCore.Mvc;
using WebAPIAutores1.Entidades;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores1.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebAPIAutores1.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "EsAdmin")]
    public class AutoresController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public ApplicationDbContext context { get; }
        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        [HttpGet("configuraciones")]
        [AllowAnonymous] //permite que alguien sin token ingrese a este end point
        public ActionResult<string> ObtenerConfiguraciones()
        {
            //proveedores de configuracion
            //iconfiguration sirve para tomar valores de los proveedores de configuracion: appsettings, secrets.json, lauchnsettings.. o proveedores de azure o sistema operativo
            //orden de declaracion de los proveedores de config es importante:
            //en caso de mismo nombre de campo, se toma el valor del ultimo proveedor agregado
            return configuration["datos:apellido"];
        }
        [HttpGet( Name ="obtenerAutores")]      
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            var autores = await context.Autores                
                                .ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}", Name ="obtenerAutor")]
        public async Task<ActionResult<AutorDTOconLibros>> Get(int id)
        {
            var autor = await context.Autores
                            .Include(autorDB => autorDB.AutoresLibros)
                            .ThenInclude(autoresLibrosDB => autoresLibrosDB.Libro)
                            .FirstOrDefaultAsync(autorBD=> autorBD.Id == id);
            if (autor == null)
            {
                return NotFound();
            }


            var autorDTO = mapper.Map<AutorDTOconLibros>(autor);
            GenerarEnlaces(autorDTO);
            return autorDTO;
        }

        private void GenerarEnlaces(AutorDTO autorDTO)
        {
            autorDTO.Enlaces.Add(new DatoHATEOAS(Url.Link("obtenerAutor", new {id = autorDTO.Id}), "Obtener-Autor", "GET" ));
            autorDTO.Enlaces.Add(new DatoHATEOAS(Url.Link("modificarAutor", new {id = autorDTO.Id}), "Modificar-Autor", "PUT" ));
            autorDTO.Enlaces.Add(new DatoHATEOAS(Url.Link("eliminarAutor", new {id = autorDTO.Id}), "Eliminar-Autor", "DELETE" ));
        }

        [HttpGet("{nombre}", Name ="obtenerAutoresPorNombre")]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(autorBD=> autorBD.Nombre.Contains(nombre)).ToListAsync();   
            if (autores == null)
            {
                return NotFound();
            }

            var autorDTO = mapper.Map<List<AutorDTO>>(autores);           
            return autorDTO;
        }

        [HttpPost(Name ="crearAutor")]
        public async Task<ActionResult> Post([FromBody] AutorCreacionDTO autorCreacionDTO)
        {
            var existeAutor = await context.Autores.AnyAsync(autorBD=> autorBD.Nombre == autorCreacionDTO.Nombre);

            if (existeAutor)
            {
                return BadRequest($"Ya existe un autor con el nombre {autorCreacionDTO.Nombre}");
            }

            Autor autor = mapper.Map<Autor>(autorCreacionDTO);            

            context.Add(autor);
            await context.SaveChangesAsync();

            AutorDTO autorDTO = mapper.Map<AutorDTO>(autor);
            
            return CreatedAtRoute("obtenerAutor", new { id = autorDTO.Id }, autorDTO);
        }

        [HttpPut("{id:int}", Name ="modificarAutor")]
        public async Task<ActionResult> Put(AutorCreacionDTO autorCreacionDTO, int id)
        {
            bool existe = context.Autores.AnyAsync(predicate => predicate.Id == id).Result;
            if (! existe)
            {
                return NotFound();
            }   

            var autor = mapper.Map<Autor>(autorCreacionDTO);
            autor.Id = id;
            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{id:int}", Name ="eliminarAutor")]
        public async Task<ActionResult> Delete(int id)
        {
            bool existe = context.Autores.AnyAsync(predicate => predicate.Id == id).Result;
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id }); //con esto lo marco para borrarlo, pero todavia no lo borro
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
