using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores1.DTOs;
using WebAPIAutores1.Entidades;

namespace WebAPIAutores1.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : Controller
    {
        private readonly IMapper mapper;

        public ApplicationDbContext Context { get; }
        
        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            Context = context;
            this.mapper = mapper;
        }


        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibroDTO>> Get(int id )
        {
            var libro = await Context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }
            return mapper.Map<LibroDTO>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            //var existe = await Context.Autores.AnyAsync(x=> x.Id == libro.AutorId);
            //if (! existe) { 
            //    return BadRequest($"No existe el autor con el id { libro.AutorId}"); 
            //};
            Libro libro = mapper.Map<Libro>(libroCreacionDTO);
            Context.Add(libro);
            await Context.SaveChangesAsync();
            return Ok(libro);
        }
    }
}
