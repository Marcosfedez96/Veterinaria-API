using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Veterinaria_API.DTOs;
using Veterinaria_API.Models;
using Veterinaria_API.Common;

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
        public async Task<ActionResult<PagedResult<TurnoDto>>> GetAll(
            [FromQuery]int? idVeterinario, 
            [FromQuery]DateOnly? fecha, 
            [FromQuery] bool? hoy,
            [FromQuery] int numPagina = 1 ,
            [FromQuery] int tamPagina = 3)
        {

            DateOnly dia = DateOnly.FromDateTime(DateTime.Now);

            var resultadoBusqueda = _context.Turnos
                .Include(m => m.Mascota)
                .Include(v => v.Veterianario)
                .AsQueryable();
           
            if(idVeterinario.HasValue || hoy == true || fecha.HasValue)
            {
                resultadoBusqueda = resultadoBusqueda
                    .Where(x => (idVeterinario.HasValue && x.VeterinarioId == idVeterinario)
                    || (hoy == true && x.DiaTurno == dia)
                    || (fecha.HasValue && x.DiaTurno == fecha));
            }
            int totalRegistros = await resultadoBusqueda.CountAsync();
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamPagina);

           List <TurnoDto> listTurnosDto = await resultadoBusqueda
                .OrderBy(t => t.Id)
                .Skip((numPagina - 1)* tamPagina)
                .Take(tamPagina)
                .Select(t => new TurnoDto
                {
                    Id = t.Id,
                    DiaTurno = t.DiaTurno,
                    HoraTurno = t.HoraTurno,
                    VeterinarioId = t.VeterinarioId,
                    VeterinarioNombre = t.Veterianario.Nombre,
                    MascotaId = t.MascotaId,
                    MascotaNombre = t.Mascota.Nombre
                }).ToListAsync();

            var paginas = new PagedResult<TurnoDto>
            {
                Datos = listTurnosDto,
                PaginaActual = numPagina,
                TamanioPagina = tamPagina,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas
            };
                        
            return Ok(paginas);


        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TurnoDto>> GetById([FromRoute]int id)
        {
            var resultadoBusqueda = await _context.Turnos
                .Include(m => m.Mascota)
                .Include(v => v.Veterianario)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El turno no existe." });
            }
            TurnoDto turnoDto = new TurnoDto()
            {
                Id = resultadoBusqueda.Id,
                DiaTurno = resultadoBusqueda.DiaTurno,
                HoraTurno = resultadoBusqueda.HoraTurno,
                VeterinarioId = resultadoBusqueda.VeterinarioId,
                VeterinarioNombre = resultadoBusqueda.Veterianario.Nombre,
                MascotaId = resultadoBusqueda.MascotaId,
                MascotaNombre = resultadoBusqueda.Mascota.Nombre
            };
            return Ok(turnoDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<Turno>> PostTurno([FromBody] CrearTurnoDto crearTurnoDto)
        {
            var turnoOcupado = await _context.Turnos
                .FirstOrDefaultAsync(x => x.HoraTurno == crearTurnoDto.HoraTurno 
                && x.DiaTurno == crearTurnoDto.DiaTurno
                && x.VeterinarioId == crearTurnoDto.VeterinarioId);
            if(turnoOcupado != null)
            {
                return Conflict(new ErrorResponse { Mensaje = "El turno esta Ocupado" });
            }
            Turno turno = new Turno()
            {
                DiaTurno = crearTurnoDto.DiaTurno,
                HoraTurno = crearTurnoDto.HoraTurno,
                VeterinarioId = crearTurnoDto.VeterinarioId,
                MascotaId = crearTurnoDto.MascotaId
            };
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
            return CreatedAtAction(nameof(GetById), new { id = turno.Id }, crearTurnoDto);
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
                return Conflict(new ErrorResponse { Mensaje = "El turno esta Ocupado" });
            }
            var resultadoBusqueda = await _context.Turnos.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El turno no existe en el sistema" });
            }
            turno.Mascota = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == turno.MascotaId);
            if (turno.Mascota == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "La mascota no Existe en el sistema." });
            }
            turno.Veterianario = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == turno.VeterinarioId);
            if (turno.Veterianario == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El veterinario no existe en el sistema." });
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
                return NotFound(new ErrorResponse { Mensaje = "El turno no existe en el sistema." });
            }

            _context.Turnos.Remove(resultadoBusqueda);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
