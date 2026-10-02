using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria_API.DTOs;
using Veterinaria_API.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Veterinaria_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TutoresController : ControllerBase
    {
        private VeterinariaContext _context;
        public TutoresController(VeterinariaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<TutorDto>>> GetAll()
        {

            var TutoresDto = await _context.Tutores
                .Select(m => new TutorDto
                {
                    Id = m.Id,
                    Nombre = m.Nombre,
                    Telefono = m.Telefono,
                    Email = m.Email,
                    MascotasId = m.Mascotas.Select(m=> m.Id).ToList(),
                    NombreMascotas = m.Mascotas.Select(m => m.Nombre).ToList()
                }).ToListAsync();
            
            return Ok(TutoresDto);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TutorDto>> GetById([FromRoute] int id)
        {
            var resultadoBusqueda = await _context.Tutores
                .Include(t => t.Mascotas)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(resultadoBusqueda == null)
            {
                return NotFound("El tutor que busca no existe");
            }

            TutorDto tutorDto = new TutorDto
            {

                Id = resultadoBusqueda.Id,
                Nombre = resultadoBusqueda.Nombre,
                Telefono = resultadoBusqueda.Telefono,
                Email = resultadoBusqueda.Email,
                MascotasId = resultadoBusqueda.Mascotas.Select(m => m.Id).ToList(),
                NombreMascotas = resultadoBusqueda.Mascotas.Select(m => m.Nombre).ToList()
            };

            return Ok(tutorDto);
        }
        [HttpPost]
        public async Task <ActionResult<Tutor>> PostTutor([FromBody] CrearTutorDto crearTutorDto)
        {
            Tutor tutor = new Tutor()
            {
                Nombre = crearTutorDto.Nombre,
                Telefono = crearTutorDto.Telefono,
                Email = crearTutorDto.Email
            };
            _context.Tutores.Add(tutor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, crearTutorDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Tutor>> PutTutor([FromRoute] int id, [FromBody] Tutor tutor)
        {
            var resultadoBusqueda = await _context.Tutores.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El tutor que busca no existe.");
            }
            resultadoBusqueda.Nombre = tutor.Nombre;
            resultadoBusqueda.Telefono = tutor.Telefono;
            resultadoBusqueda.Email = tutor.Email;
            await _context.SaveChangesAsync();
            return Ok(resultadoBusqueda);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTutor([FromRoute]int id)
        {
            var resultadoBusqueda = await _context.Tutores.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El tutor que intenta eliminar no exite en el sistema.");
            }
            _context.Tutores.Remove(resultadoBusqueda);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
