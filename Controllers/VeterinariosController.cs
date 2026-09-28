using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinaria_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Veterinaria_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinariosController : ControllerBase
    {
        private readonly VeterinariaContext _context;
        public VeterinariosController(VeterinariaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Veterinario>>> GetAll([FromQuery]string? especialidad, [FromQuery]bool? esPracticante)
        {
            var resultadoBusqueda =  _context.Veterinarios.ToAsyncEnumerable();
            
            if (!string.IsNullOrEmpty(especialidad))
            {
                resultadoBusqueda = resultadoBusqueda.Where(x => x.Especialidad.Contains(especialidad, StringComparison.OrdinalIgnoreCase));
            }
            if (esPracticante.HasValue){
                resultadoBusqueda = resultadoBusqueda.Where(x => x.EsPracticante.Equals(esPracticante));
            }
            return Ok(await resultadoBusqueda.ToListAsync());
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Veterinario>> GetById([FromRoute] int id)
        {
            var resultadoBusqueda = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El veterinario no existe");
            }
            else
            {
                return Ok(resultadoBusqueda);
            }
            
        }
        [HttpPost]
        public async Task<IActionResult> PostVeterinario([FromBody] Veterinario veterinario)
        {
            if(veterinario.EsPracticante.Equals(true) && !String.IsNullOrEmpty(veterinario.Matricula))
            {
                return BadRequest("Un practicante no tiene matricula");
            }
            //veterinario.Id = BaseDeDatos.veterinarios.Any() ? BaseDeDatos.veterinarios.Max(x => x.Id) + 1 : 1;
            _context.Veterinarios.Add(veterinario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = veterinario.Id }, veterinario);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutVeterinario([FromRoute]int id, [FromBody]Veterinario veterinario)
        {
            var resultadoBusqueda = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El veterinario que busca no existe en el sistema.");
            }
            else
            {
                resultadoBusqueda.Nombre = veterinario.Nombre;
                resultadoBusqueda.Especialidad = veterinario.Especialidad;
                resultadoBusqueda.Matricula = veterinario.Matricula;
                resultadoBusqueda.EsPracticante = veterinario.EsPracticante;
                await _context.SaveChangesAsync();
                return Ok(resultadoBusqueda);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVeterinario([FromRoute]int id)
        {
            var resultadoBusqueda = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El veterinario que intenta eliminar no existe en el sistema.");
            }
            else
            {
                _context.Veterinarios.Remove(resultadoBusqueda);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}
    
