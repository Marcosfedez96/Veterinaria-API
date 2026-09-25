using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using Veterinaria_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Veterinaria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MascotasController : ControllerBase
    {
        private readonly VeterinariaContext _context;
        public MascotasController(VeterinariaContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Mascota>>> GetAll([FromQuery] string? especie, [FromQuery] string? nombre)
        {
            var Resultado = _context.Mascotas.AsQueryable();

            if (!string.IsNullOrEmpty(especie))
            {
                Resultado = Resultado.Where(x => x.Especie.Equals(especie, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                Resultado = Resultado.Where(x => x.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
            }
            var resultadoFinal = await Resultado.ToListAsync();
            return Ok(resultadoFinal);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Mascota>> GetById([FromRoute] int id)
        {
            var mascota = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == id);
            if (mascota == null)
            {
                return NotFound();
            }
            return Ok(mascota);
        }
        [HttpPost]
        public async Task<IActionResult> CrearMascota([FromBody] Mascota _mascota)
        {
            if(_mascota.Especie.Equals("Ave",StringComparison.OrdinalIgnoreCase) && _mascota.Edad > 15)
            {
               return BadRequest(new {messaje = "Las aves no puedes superar los 15 años" });
            }
           
            _context.Add(_mascota);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = _mascota.Id }, _mascota);
        
            
            
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarMascota([FromRoute] int id, [FromBody] Mascota _mascota)
        {
            var mascotaExistente = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == id);
            if (mascotaExistente == null)
            {
                return NotFound();
            }
            if(_mascota.Especie.Equals("perro",StringComparison.OrdinalIgnoreCase) && _mascota.Edad > 22 ||
               _mascota.Especie.Equals("ave",StringComparison.OrdinalIgnoreCase)&& _mascota.Edad > 15 ||
               _mascota.Especie.Equals("raton",StringComparison.OrdinalIgnoreCase)&& _mascota.Edad > 4)
            {
                return BadRequest(new { message = "la edad puesta es imposible para esta especie" });
            }
            mascotaExistente.Nombre = _mascota.Nombre;
            mascotaExistente.Especie = _mascota.Especie;
            mascotaExistente.Edad = _mascota.Edad;
            await _context.SaveChangesAsync();
            return Ok(mascotaExistente);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> EliminarMascota([FromRoute]int id)
        {
            var mascotaExistente = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == id);
            if (mascotaExistente == null)
            {
                return NotFound();
            }
            _context.Mascotas.Remove(mascotaExistente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}
