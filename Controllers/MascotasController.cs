using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using Veterinaria_API.Models;

namespace Veterinaria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MascotasController : ControllerBase
    {
        private static List<Mascota> mascotas = new List<Mascota>
        {
            new Mascota { Id = 1, Nombre = "Firulais", Especie = "Perro", Edad = 3 },
            new Mascota { Id = 2, Nombre = "Michi", Especie = "Gato", Edad = 2 },
            new Mascota { Id = 3, Nombre = "Nemo", Especie = "Pez", Edad = 1 }
        };

        [HttpGet("{id:int}")]
        public ActionResult<Mascota> GetById([FromRoute] int id)
        {
            var mascota = mascotas.FirstOrDefault(x => x.Id == id);
            if (mascota == null)
            {
                return NotFound();
            }
            return Ok(mascota);
        }
        [HttpPost]
        public IActionResult CrearMascota([FromBody] Mascota _mascota)
        {
            if(_mascota.Especie.Equals("Ave",StringComparison.OrdinalIgnoreCase) && _mascota.Edad > 15)
            {
               return BadRequest(new {messaje = "Las aves no puedes superar los 15 años" });
            }
            _mascota.Id = mascotas.Any() ? mascotas.Max(x => x.Id) + 1 : 1;
            mascotas.Add(_mascota);
            return CreatedAtAction(nameof(GetById), new { id = _mascota.Id }, _mascota);
        }
        [HttpPut("{id:int}")]
        public IActionResult ActualizarMasota([FromRoute] int id, [FromBody] Mascota _mascota)
        {
            var mascotaExistente = mascotas.FirstOrDefault(x => x.Id == id);
            if (mascotaExistente == null)
            {
                return NotFound();
            }
            if(_mascota.Especie.Equals("perro",StringComparison.OrdinalIgnoreCase) && _mascota.Edad > 22 ||
               _mascota.Especie.Equals("ave",StringComparison.OrdinalIgnoreCase)&& _mascota.Edad > 15 ||
               _mascota.Especie.Equals("raton",StringComparison.OrdinalIgnoreCase)&& _mascota.Edad > 4)
            {
                return BadRequest(new { Message = "la edad puesta se imposible para esta especie" });
            }
            mascotaExistente.Nombre = _mascota.Nombre;
            mascotaExistente.Especie = _mascota.Especie;
            mascotaExistente.Edad = _mascota.Edad;
            return Ok(mascotaExistente);
        }
        [HttpDelete("{id:int}")]
        public IActionResult EliminarMascota([FromRoute]int id)
        {
            var mascotaExistente = mascotas.FirstOrDefault(x => x.Id == id);
            if (mascotaExistente == null)
            {
                return NotFound();
            }
            mascotas.Remove(mascotaExistente);
            return NoContent();
        }
        [HttpGet]
        public ActionResult<List<Mascota>> GetAll([FromQuery]string? especie,[FromQuery] string? nombre)
        {
            var Resultado = mascotas.AsEnumerable();

            if(!string.IsNullOrEmpty(especie))
            {
                Resultado = Resultado.Where(x => x.Especie.Equals(especie, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                Resultado = Resultado.Where(x => x.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
            }
             
            return Ok(Resultado.ToList());
        }
        
    }
}
