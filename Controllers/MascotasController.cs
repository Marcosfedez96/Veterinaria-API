using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using Veterinaria_API.Models;
using Veterinaria_API.DTOs;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Veterinaria_API.Common;

namespace Veterinaria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MascotasController : ControllerBase
    {
        private readonly VeterinariaContext _context;
        private readonly IMapper _mapper;
        public MascotasController(VeterinariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<PagedResult<MascotaDto>>> GetAll([FromQuery] string? especie,
            [FromQuery] string? nombre,
            [FromQuery]int numPagina = 1,
            [FromQuery]int tamanioPagina = 2)
        {
            var resultado = _context.Mascotas.
                Include(t=> t.Tutor)
                .AsQueryable();


            if (!string.IsNullOrEmpty(especie))
            {
                resultado = resultado.Where(x => x.Especie.Equals(especie, StringComparison.OrdinalIgnoreCase));
            }
            if (!string.IsNullOrEmpty(nombre))
            {
                resultado = resultado.Where(x => x.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));
            }


            int totalRegistros = await resultado.CountAsync();
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanioPagina);
            var mascotasDto = await resultado
                .OrderBy(m => m.Id)
                .Skip((numPagina - 1) * tamanioPagina)
                .Take(tamanioPagina)
                .ProjectTo<MascotaDto>(_mapper.ConfigurationProvider).ToListAsync();
            //var mascotasDtoList = await resultado.
            //    .Select(m => new MascotaDto
            //    {
            //        Id = m.Id,
            //        Nombre = m.Nombre,
            //        Especie = m.Especie,
            //        Edad = m.Edad,
            //        TutorId = m.TutorId,
            //        TutorNombre = m.Tutor.Nombre
            //    }

            //    ).ToListAsync();
            var paginas = new PagedResult<MascotaDto>
            {
                Datos = mascotasDto,
                PaginaActual = numPagina,
                TamanioPagina = tamanioPagina,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas
            };

            return Ok(paginas);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MascotaDto>> GetById([FromRoute] int id)
        {
            var resultadoBusqueda = await _context.Mascotas
                .Include(t => t.Tutor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "La mascota que busca no existe en el sistema" });
            }

            var mascotaDto = _mapper.Map<MascotaDto>(resultadoBusqueda);
            return Ok(mascotaDto);
        }
        [HttpPost]
        public async Task<IActionResult> CrearMascota([FromBody] CrearMascotaDto crearMascotaDto)
        {
            if(crearMascotaDto.Especie.Equals("Ave",StringComparison.OrdinalIgnoreCase) && crearMascotaDto.Edad > 15)
            {
               return BadRequest(new ErrorResponse { Mensaje = "Las aves no puedes superar los 15 años" });
            }
            Tutor tutor = await _context.Tutores.FirstOrDefaultAsync(x => x.Id == crearMascotaDto.TutorId);
            if(tutor == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "el Tutor no existe." });
            }
            var turnos = new List<Turno>();
            var mascota = _mapper.Map<Mascota>(crearMascotaDto);
            //Mascota mascota = new Mascota
            //{
            //    Nombre = crearMascotaDto.Nombre,
            //    Especie = crearMascotaDto.Especie,
            //    Edad = crearMascotaDto.Edad,
            //    TutorId = crearMascotaDto.TutorId,
            //    Tutor = tutor,
            //    Turnos = turnos
            //};
            _context.Add(mascota);
            await _context.SaveChangesAsync();
            var mascotaDto = _mapper.Map<MascotaDto>(mascota);
            return CreatedAtAction(nameof(GetById), new { id = mascota.Id }, mascotaDto);
                
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> ActualizarMascota([FromRoute] int id, [FromBody] Mascota _mascota)
        {
            var mascotaExistente = await _context.Mascotas.FirstOrDefaultAsync(x => x.Id == id);
            if (mascotaExistente == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "La mascota que quiere editar no existe" });
            }
            if(_mascota.Especie.Equals("perro",StringComparison.OrdinalIgnoreCase) && _mascota.Edad > 22 ||
               _mascota.Especie.Equals("ave",StringComparison.OrdinalIgnoreCase)&& _mascota.Edad > 15 ||
               _mascota.Especie.Equals("raton",StringComparison.OrdinalIgnoreCase)&& _mascota.Edad > 4)
            {
                return BadRequest(new ErrorResponse { Mensaje = "la edad puesta es imposible para esta especie" });
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
                return NotFound(new ErrorResponse { Mensaje = "La mascota que quiere eliminar no existe en el sistema" });
            }
            _context.Mascotas.Remove(mascotaExistente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        
    }
}
