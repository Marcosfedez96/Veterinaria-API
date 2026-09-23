using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinaria_API.Models;

namespace Veterinaria_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeterinariosController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Veterinario>> GetAll([FromQuery]string? especialidad, [FromQuery]bool? esPracticante)
        {
            var resultadoBusqueda = BaseDeDatos.veterinarios.AsEnumerable(); 
            if (!string.IsNullOrEmpty(especialidad))
            {
                resultadoBusqueda = resultadoBusqueda.Where(x => x.Especialidad.Contains(especialidad, StringComparison.OrdinalIgnoreCase));
            }
            if (esPracticante.HasValue){
                resultadoBusqueda = resultadoBusqueda.Where(x => x.EsPracticante.Equals(esPracticante));
            }
            return Ok(resultadoBusqueda.ToList());
        }
        [HttpGet("{id:int}")]
        public ActionResult<Veterinario> GetById([FromRoute] int id)
        {
            var resultadoBusqueda = BaseDeDatos.veterinarios.FirstOrDefault(x => x.Id == id);
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
        public IActionResult PostVeterinario([FromBody] Veterinario veterinario)
        {
            if(veterinario.EsPracticante.Equals(true) && !String.IsNullOrEmpty(veterinario.Matricula))
            {
                return BadRequest("Un practicante no tiene matricula");
            }
            veterinario.Id = BaseDeDatos.veterinarios.Any() ? BaseDeDatos.veterinarios.Max(x => x.Id) + 1 : 1;
            BaseDeDatos.veterinarios.Add(veterinario);
            return CreatedAtAction(nameof(GetById), new { id = veterinario.Id }, veterinario);
        }
        [HttpPut("{id:int}")]
        public IActionResult PutVeterinario([FromRoute]int id, [FromBody]Veterinario veterinario)
        {
            var resultadoBusqueda = BaseDeDatos.veterinarios.FirstOrDefault(x => x.Id == id);
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
                return Ok(resultadoBusqueda);
            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteVeterinario([FromRoute]int id)
        {
            var resultadoBusqueda = BaseDeDatos.veterinarios.FirstOrDefault(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El veterinario que intenta eliminar no existe en el sistema.");
            }
            else
            {
                BaseDeDatos.veterinarios.Remove(resultadoBusqueda);
                return NoContent();
            }
        }

    }
}
    
