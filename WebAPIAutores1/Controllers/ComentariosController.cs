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

        [HttpGet("{id:int}", Name ="ObtenerComentario")]
        public async Task<ActionResult<ComentarioDTO>> GetById(int id)
        {
            var comentario = await applicationDbContext.Comentarios.FirstOrDefaultAsync(comentarioDB=> comentarioDB.Id == id);
            return mapper.Map<ComentarioDTO>(comentario);
        }
     
        // POST api/<ComentariosController>
        [HttpPost]
        public async Task<ActionResult> Post([FromRoute] int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var libro = await applicationDbContext.Libros.AnyAsync(libroBD=> libroBD.Id == libroId);
            if (!libro)
            {
                return NotFound();
            }

            Comentario comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            applicationDbContext.Add(comentario);
            await applicationDbContext.SaveChangesAsync();

            ComentarioDTO comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("ObtenerComentario", new { id = comentarioDTO.Id, libroId = libroId }, comentarioDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int libroId, [FromRoute] int id, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var existeLibro = await applicationDbContext.Libros.AnyAsync(libroBD => libroBD.Id == libroId);
            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await applicationDbContext.Comentarios.AnyAsync(comentarioDB => comentarioDB.Id == id);
            if (!existeComentario)
            {
                return NotFound();
            }

            Comentario comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibroId = libroId;
            comentario.Id = id;
            applicationDbContext.Update(comentario);
            await applicationDbContext.SaveChangesAsync();

            ComentarioDTO comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return CreatedAtRoute("ObtenerComentario", new { id = id, libroId = libroId }, comentarioDTO);
        }
       
    }
}
