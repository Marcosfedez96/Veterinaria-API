using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Veterinaria_API.Models;

namespace Veterinaria_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TurnosController : ControllerBase
    {
        private readonly VeterinariaContext _context;
        public TurnosController(VeterinariaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Turno>>> GetAll([FromQuery]int? idVeterinario, [FromQuery]DateOnly? fecha, [FromQuery] bool? hoy)
        {
            DateOnly dia = DateOnly.FromDateTime(DateTime.Now);

            var resultadoBusqueda = _context.Turnos
                .Include(m => m.Mascota)
                .Include(v => v.Veterianario).AsQueryable();
            if(idVeterinario.HasValue || hoy == true || fecha.HasValue)
            {
                resultadoBusqueda = resultadoBusqueda
                    .Where(x => (idVeterinario.HasValue && x.VeterinarioId == idVeterinario)
                    || (hoy == true && x.DiaTurno == dia)
                    || (fecha.HasValue && x.DiaTurno == fecha));
            }

            var resultadoFinal = await resultadoBusqueda.ToListAsync();
                        
            return Ok(resultadoFinal);


        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Turno>> GetById([FromRoute]int id)
        {
            var resultadoBusqueda = await _context.Turnos
                .Include(m => m.Mascota)
                .Include(v => v.Veterianario)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El turno no existe.");
            }
            return Ok(resultadoBusqueda);
        }
        
        [HttpPost]
        public async Task<ActionResult<Turno>> PostTurno([FromBody]Turno turno)
        {
            var turnoOcupado = await _context.Turnos
                .FirstOrDefaultAsync(x => x.HoraTurno == turno.HoraTurno 
                && x.DiaTurno == turno.DiaTurno
                && x.VeterinarioId == turno.VeterinarioId);
            if(turnoOcupado != null)
            {
                return Conflict("El turno esta Ocupado");
            }
            turno.Mascota = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == turno.MascotaId);
            if (turno.Mascota == null)
            {
                return NotFound("La mascota no Existe en el sistema.");
            }
            turno.Veterianario = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == turno.VeterinarioId);
            if (turno.Veterianario == null)
            {
                return NotFound("El veterinario no existe en el sistema.");
            }
            _context.Turnos.Add(turno);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = turno.Id }, turno);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTurno([FromRoute]int id, [FromBody]Turno turno)
        {
            var turnoOcupado = await _context.Turnos
                .FirstOrDefaultAsync(x => x.HoraTurno == turno.HoraTurno
                && x.DiaTurno == turno.DiaTurno
                && x.VeterinarioId == turno.VeterinarioId);
            if (turnoOcupado != null)
            {
                return Conflict("El turno esta Ocupado");
            }
            var resultadoBusqueda = await _context.Turnos.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El turno no existe en el sistema");
            }
            turno.Mascota = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == turno.MascotaId);
            if (turno.Mascota == null)
            {
                return NotFound("La mascota no Existe en el sistema.");
            }
            turno.Veterianario = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == turno.VeterinarioId);
            if (turno.Veterianario == null)
            {
                return NotFound("El veterinario no existe en el sistema.");
            }
            resultadoBusqueda.DiaTurno = turno.DiaTurno;
            resultadoBusqueda.HoraTurno = turno.HoraTurno;
            resultadoBusqueda.MascotaId = turno.MascotaId;
            resultadoBusqueda.VeterinarioId = turno.VeterinarioId;
            await _context.SaveChangesAsync();
            return Ok(turno);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTurno([FromRoute]int id)
        {
            var resultadoBusqueda = await _context.Turnos.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound("El turno no existe en el sistema.");
            }

            _context.Turnos.Remove(resultadoBusqueda);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
