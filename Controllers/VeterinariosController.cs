using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Veterinaria_API.Common;
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
        private readonly IMapper _mapper;
        public VeterinariosController(VeterinariaContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<VeterinarioDto>>> GetAll([FromQuery]string? especialidad, [FromQuery]bool? esPracticante)
        {
            var resultadoBusqueda =  _context.Veterinarios.AsQueryable();
            
            if (!string.IsNullOrEmpty(especialidad))
            {
                resultadoBusqueda = resultadoBusqueda.Where(x => x.Especialidad.Contains(especialidad, StringComparison.OrdinalIgnoreCase));
            }
            if (esPracticante.HasValue){
                resultadoBusqueda = resultadoBusqueda.Where(x => x.EsPracticante.Equals(esPracticante));
            }
            var ListVeterinarDto = await resultadoBusqueda.ProjectTo<VeterinarioDto>(_mapper.ConfigurationProvider).ToListAsync();
            //List<VeterinarioDto> listVeterinarioDto = await resultadoBusqueda
            //    .Select(v => new VeterinarioDto()
            //    {
            //        Id = v.Id,
            //        Nombre = v.Nombre,
            //        Especialidad = v.Especialidad,
            //        Matricula = v.Matricula,
            //        EsPracticante = v.EsPracticante
            //    }).ToListAsync();
            return Ok(ListVeterinarDto);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<VeterinarioDto>> GetById([FromRoute] int id)
        {
            var resultadoBusqueda = await _context.Veterinarios
                .FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El veterinario no existe" });
            }
            var veterinarioDto = _mapper.Map<VeterinarioDto>(resultadoBusqueda);
            //VeterinarioDto veterinarioDto = new VeterinarioDto()
            //{
            //    Id = resultadoBusqueda.Id,
            //    Especialidad = resultadoBusqueda.Especialidad,
            //    Matricula = resultadoBusqueda.Matricula,
            //    EsPracticante = resultadoBusqueda.EsPracticante
            //};
             return Ok(veterinarioDto);
           
            
        }
        [HttpPost]
        public async Task<IActionResult> PostVeterinario([FromBody] CrearVeterinarioDto crearVeterinarioDto)
        {
            if(crearVeterinarioDto.EsPracticante.Equals(true) && !string.IsNullOrEmpty(crearVeterinarioDto.Matricula))
            {
                return BadRequest(new ErrorResponse { Mensaje = "Un practicante no tiene matricula" });
            }
            var veterinario = _mapper.Map<Veterinario>(crearVeterinarioDto);
            //Veterinario veterinario = new Veterinario()
            //{
            //    Nombre = crearVeterinarioDto.Nombre,
            //    Especialidad = crearVeterinarioDto.Especialidad,
            //    Matricula = crearVeterinarioDto.Matricula,
            //    EsPracticante = crearVeterinarioDto.EsPracticante
            //};
            //veterinario.Id = BaseDeDatos.veterinarios.Any() ? BaseDeDatos.veterinarios.Max(x => x.Id) + 1 : 1;
            _context.Veterinarios.Add(veterinario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = veterinario.Id }, crearVeterinarioDto);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutVeterinario([FromRoute]int id, [FromBody] CrearVeterinarioDto crearVeterinarioDto)
        {
            var resultadoBusqueda = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El veterinario que busca no existe en el sistema." });
            }
            _mapper.Map(crearVeterinarioDto, resultadoBusqueda);

            //resultadoBusqueda.Nombre = veterinario.Nombre;
            //resultadoBusqueda.Especialidad = veterinario.Especialidad;
            //resultadoBusqueda.Matricula = veterinario.Matricula;
            //resultadoBusqueda.EsPracticante = veterinario.EsPracticante;
            await _context.SaveChangesAsync();
            return Ok(resultadoBusqueda);
            
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteVeterinario([FromRoute]int id)
        {
            var resultadoBusqueda = await _context.Veterinarios.FirstOrDefaultAsync(x => x.Id == id);
            if(resultadoBusqueda == null)
            {
                return NotFound(new ErrorResponse { Mensaje = "El veterinario que intenta eliminar no existe en el sistema." });
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
    
