using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores1.DTOs;
using WebAPIAutores1.Entidades;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPIAutores1.Controllers
{
    [Route("api/libros/{libroId:int}/comentarios")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ComentarioDTO>>> Get(int libroId)
        {
            var comentarios = await applicationDbContext.Comentarios.Where(comentarioDB => comentarioDB.LibroId == libroId).ToListAsync();
            return mapper.Map<List<ComentarioDTO>>(comentarios);
        }

     
        // POST api/<ComentariosController>
        [HttpPost]
        public async Task<ActionResult> Post([FromRoute] int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var libro = await applicationDbContext.Libros.AnyAsync(comentarioDB=> comentarioDB.AutorId == libroId);
            if (!libro)
            {
                return NotFound();
            }

            Comentario comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            applicationDbContext.Add(comentario);
            await applicationDbContext.SaveChangesAsync();
            return Ok(comentario);

        }

       
    }
}
