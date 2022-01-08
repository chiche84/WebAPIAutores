using Microsoft.AspNetCore.Mvc;
using WebAPIAutores1.Entidades;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores1.DTOs;
using AutoMapper;

namespace WebAPIAutores1.Controllers
{
    [ApiController]
    [Route("api/autores")]

    public class AutoresController : ControllerBase
    {
        private readonly IMapper mapper;

        public ApplicationDbContext context { get; }
        public AutoresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        [HttpGet]      
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get()
        {
            var autores = await context.Autores                
                                .ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}", Name ="ObtenerAutor")]
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

            
            return mapper.Map<AutorDTOconLibros>(autor);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<IEnumerable<AutorDTO>>> Get(string nombre)
        {
            var autores = await context.Autores.Where(autorBD=> autorBD.Nombre.Contains(nombre)).ToListAsync();   
            if (autores == null)
            {
                return NotFound();
            }

            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpPost]
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
            return CreatedAtRoute("ObtenerAutor", new { id = autorDTO.Id }, autorDTO);
        }

        [HttpPut("{id:int}")]
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
        [HttpDelete("{id:int}")]
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
