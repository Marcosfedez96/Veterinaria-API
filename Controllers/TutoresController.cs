using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria_API.Models;

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
        public async Task<ActionResult<List<Tutor>>> GetAll()
        {

            var resultadoBusqueda = await _context.Tutores.Include(t => t.Mascotas).ToListAsync();
            
            return Ok(resultadoBusqueda);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Tutor>> GetById([FromRoute] int id)
        {
            var resultadoBusqueda = await _context.Tutores
                .Include(t => t.Mascotas)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if(resultadoBusqueda == null)
            {
                return NotFound("El tutor que busca no existe");
            }
            return Ok(resultadoBusqueda);
        }
        [HttpPost]
        public async Task <ActionResult<Tutor>> PostTutor([FromBody] Tutor tutor)
        {

            _context.Tutores.Add(tutor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = tutor.Id }, tutor);
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
