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
            var resultadoBusqueda =  _context.Veterinarios.AsQueryable();
            
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
            var resultadoBusqueda = await _context.Veterinarios.Include(t=>t.Turnos).FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<IActionResult> PostVeterinario([FromBody] CrearVeterinarioDto crearVeterinarioDto)
        {
            if(crearVeterinarioDto.EsPracticante.Equals(true) && !string.IsNullOrEmpty(crearVeterinarioDto.Matricula))
            {
                return BadRequest("Un practicante no tiene matricula");
            }
            Veterinario veterinario = new Veterinario()
            {
                Nombre = crearVeterinarioDto.Nombre,
                Especialidad = crearVeterinarioDto.Especialidad,
                Matricula = crearVeterinarioDto.Matricula,
                EsPracticante = crearVeterinarioDto.EsPracticante
            };
            //veterinario.Id = BaseDeDatos.veterinarios.Any() ? BaseDeDatos.veterinarios.Max(x => x.Id) + 1 : 1;
            _context.Veterinarios.Add(veterinario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = veterinario.Id }, crearVeterinarioDto);
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
    
