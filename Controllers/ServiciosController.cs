using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria_API.Common;
using Veterinaria_API.DTOs;
using Veterinaria_API.Models;
namespace Veterinaria_API.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServiciosController : ControllerBase
    {
        private readonly VeterinariaContext _context;
        private readonly IMapper _mapper;

        public ServiciosController(VeterinariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ServicioDto>>> GetAll( 
            [FromQuery] int numPagina = 1,
            [FromQuery] int tamPagina = 3)
        {
            var resultadoBusqueda = _context.Servicios.AsQueryable();
            int totalRegistros = await resultadoBusqueda.CountAsync();
            int totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamPagina);

            var listServicioDto = await resultadoBusqueda
                .OrderBy(s => s.Id)
                .Skip((numPagina -1)*tamPagina)
                .Take(tamPagina)
                .ProjectTo<ServicioDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            var pagina = new PagedResult<ServicioDto>()
            {
                Datos = listServicioDto,
                PaginaActual = numPagina,
                TamanioPagina = tamPagina,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas,
            };
            return Ok(pagina);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ServicioDto>> GetById(
            [FromRoute]int id)
        {
            var resultadoBusqueda = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El servicio que busca no existe. " });
            }
            var servicioDto = _mapper.Map<ServicioDto>(resultadoBusqueda);
            return Ok(servicioDto);
        }
        [HttpPost]
        public async Task<IActionResult> PostServicio(
            [FromBody]CrearServicioDto crearServicioDto)
        {
            var servicio = _mapper.Map<Servicio>(crearServicioDto);
            _context.Servicios.Add(servicio);
            await _context.SaveChangesAsync();
            var servicioDto = _mapper.Map<ServicioDto>(servicio);
            return CreatedAtAction(nameof(GetById), new { id = servicio.Id }, servicioDto);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutServicio(
            [FromRoute]int id, 
            [FromBody] CrearServicioDto actualizarServicioDto)
        {
            var resultadoBusqueda = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "No se encuentra el servicio que quiere actualizar." });
            }
            _mapper.Map(actualizarServicioDto, resultadoBusqueda);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteServicio(
            [FromRoute] int id)
        {
            var resultadoBusqueda = await _context.Servicios.FirstOrDefaultAsync(s => s.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El servicio que desea eliminar no existe en el sistema." });
            }
            _context.Servicios.Remove(resultadoBusqueda);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
