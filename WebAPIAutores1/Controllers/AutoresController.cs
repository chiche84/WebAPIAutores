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
            var autores = await context.Autores.Include(x=> x.Libros).ToListAsync();
            return mapper.Map<List<AutorDTO>>(autores);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<AutorDTO>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(autorBD=> autorBD.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            
            return mapper.Map<AutorDTO>(autor);
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
            return Ok(autor);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor debe coincidir con el enviado en la URL");
            }

            bool existe = context.Autores.AnyAsync(predicate => predicate.Id == id).Result;
            if (! existe)
            {
                return NotFound();
            }   
            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok(autor);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            bool existe = context.Autores.AnyAsync(predicate => predicate.Id == id).Result;
            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
