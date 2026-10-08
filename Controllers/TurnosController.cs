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
                .Include(t => t.Mascota)
                .Include(t => t.Veterianario)
                .Include(t => t.TurnoServicios)
                    .ThenInclude(ts => ts.Servicio)
                .AsQueryable();

            if (idVeterinario.HasValue || hoy == true || fecha.HasValue)
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
                    MascotaNombre = t.Mascota.Nombre,
                    Servicios = t.TurnoServicios.Select(ts => ts.Servicio.Nombre).ToList()
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
            var resultadoBusqueda =await _context.Turnos
                .Include(t => t.Mascota)
                .Include(t => t.Veterianario)
                .Include(t => t.TurnoServicios)
                    .ThenInclude(ts => ts.Servicio)
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
                MascotaNombre = resultadoBusqueda.Mascota.Nombre,
                Servicios = resultadoBusqueda.TurnoServicios.Select(ts => ts.Servicio.Nombre).ToList()
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
        [HttpPost ("{turnoId:int}/servicios/{servicioId:int}")]
        public async Task<IActionResult> AgregarServicio([FromRoute]int turnoId, [FromRoute]int servicioId)
        {
            var turno = await _context.Turnos.FirstOrDefaultAsync(t => t.Id == turnoId);
            if(turno == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El turno no existe en el sistema." });
            }
            var servicio = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == servicioId);
            if(servicio == null)
            {
                return NotFound(new ErrorResponse { Mensaje = " El servicio no existe en el sistema." });
            }
            bool yaAsociado = await _context.TurnoServicios
                .AnyAsync(ts => ts.ServicioId == servicioId && ts.TurnoId == turnoId);
            if (yaAsociado)
            {
                return Conflict(new ErrorResponse { Mensaje = "Este servicio ya está asociado a este turno." });
            }
            var turnoServicio = new TurnoServicio 
            { 
                Servicio = servicio,
                Turno = turno,
                ServicioId = servicioId, 
                TurnoId = turnoId
            };
            _context.TurnoServicios.Add(turnoServicio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete("{turnoId:int}/servicios/{servicioId:int}")]
        public async Task<IActionResult> DeleteServicio([FromRoute] int turnoId, [FromRoute] int servicioId)
        {
            var turnoServicio = await _context.TurnoServicios
                .FirstOrDefaultAsync(ts => ts.TurnoId == turnoId && ts.ServicioId == servicioId);
            if(turnoServicio == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El Turno/Servicio que decea Eliminar no existe en el sistema." });
            }
            _context.TurnoServicios.Remove(turnoServicio);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
    
}
