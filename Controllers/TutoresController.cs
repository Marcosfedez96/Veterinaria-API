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

            var resultadoBusqueda = _context.Tutores.AsQueryable();
            var resultadoFinal = await _context.Tutores.Include(t => t.Mascotas).ToListAsync();
            
            return Ok(resultadoFinal);
        }
        [HttpPost]
        public async Task <ActionResult<Tutor>> PostTutor([FromBody] Tutor tutor)
        {

            _context.Tutores.Add(tutor);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = tutor.Id }, tutor);
        }
    }
}
