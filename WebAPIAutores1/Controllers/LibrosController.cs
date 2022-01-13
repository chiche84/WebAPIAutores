using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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

        public ApplicationDbContext applicationDbContext { get; }
        
        public LibrosController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.mapper = mapper;
        }


        [HttpGet("{id:int}", Name ="obtenerLibro")]
        public async Task<ActionResult<LibroDTOconAutores>> Get(int id )
        {
            var libro = await applicationDbContext.Libros.
                Include(libroDB => libroDB.AutoresLibros).
                ThenInclude(autorLibroDB=> autorLibroDB.Autor).
                Include(librosDB => librosDB.Comentarios).FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {
                return NotFound();
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOconAutores>(libro);
        }

        [HttpPost(Name ="crearLibro")]
        public async Task<ActionResult> Post(LibroCreacionDTO libroCreacionDTO)
        {
            if (libroCreacionDTO.AutoresIds == null || libroCreacionDTO.AutoresIds.Count == 0)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }
            var autoresIds = await applicationDbContext.Autores.Where(autorBD => libroCreacionDTO.AutoresIds.Contains(autorBD.Id)).Select(x=> x.Id).ToListAsync();

            if (libroCreacionDTO.AutoresIds.Count != autoresIds.Count)
            {
                return BadRequest("No existe uno de los autores enviados");
            }
            Libro libro = mapper.Map<Libro>(libroCreacionDTO);

            AsignarOrdenAutores(libro);

            applicationDbContext.Add(libro);
            await applicationDbContext.SaveChangesAsync();

            LibroDTO libroDTO = mapper.Map<LibroDTO>(libro);
            return CreatedAtRoute("obtenerLibro", new { id = libroDTO.Id }, libroDTO);
        }
        [HttpPut("{id:int}", Name ="modificarLibro")]
        public async Task<ActionResult> Put(LibroCreacionDTO libroCreacionDTO, int id)
        {
            if (libroCreacionDTO.AutoresIds == null || libroCreacionDTO.AutoresIds.Count == 0)
            {
                return BadRequest("No se puede modificar un libro sin autores");
            }

            var libroDB = await applicationDbContext.Libros
                                .Include(x=> x.AutoresLibros)
                                .FirstOrDefaultAsync(x=> x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            libroDB = mapper.Map(libroCreacionDTO,libroDB);

            AsignarOrdenAutores(libroDB);

            await applicationDbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("{id:int}", Name ="patchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await applicationDbContext.Libros.FirstOrDefaultAsync(x=> x.Id == id);
            if (libroDB == null)
            {
                return NotFound();
            }

            var libroPatchDTO = mapper.Map<LibroPatchDTO>(libroDB);
            //aplico al libroPathDTO los cambios que vinieron en el patchdocument. Ej: si actualizo titulo, aca se hace eso, 
            //y le paso el modelstate para controlar q todas las reglas de validacion se estan cumpliendo
            patchDocument.ApplyTo(libroPatchDTO, ModelState);

            var esValido = TryValidateModel(libroPatchDTO);
            if (!esValido)
            {
                return BadRequest(ModelState);
            }

            mapper.Map(libroPatchDTO,libroDB);

            await applicationDbContext.SaveChangesAsync();
            return NoContent();
            //Formato del json para patch, uno para cada campo a modificar:
            //{
            //    "path": "/fechaPublicacion",
            //    "op": "replace",
            //    "value": "2022-01-07"
            //}
        }

        [HttpDelete("{id:int}", Name ="eliminarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            bool existe = applicationDbContext.Libros.AnyAsync(predicate => predicate.Id == id).Result;
            if (!existe)
            {
                return NotFound();
            }

            applicationDbContext.Remove(new Libro() { Id = id }); //con esto lo marco para borrarlo, pero todavia no lo borro
            await applicationDbContext.SaveChangesAsync();
            return Ok();
        }
        private void AsignarOrdenAutores(Libro libro)
        {
            for (int i = 0; i < libro.AutoresLibros.Count; i++)
            {
                libro.AutoresLibros[i].Orden = i;
            }
        }

    }
}
