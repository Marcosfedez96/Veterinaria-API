using AutoMapper;
using Veterinaria_API.DTOs;
using Veterinaria_API.Models;
namespace Veterinaria_API.Mapping
{
    public class VeterinariaProfile : Profile
    {
        public VeterinariaProfile()
        {
            CreateMap<Mascota, MascotaDto>().ForMember(dest => dest.TutorNombre, opt => opt.MapFrom(src => src.Tutor.Nombre));
            CreateMap<CrearMascotaDto, Mascota>();

            CreateMap<Tutor, TutorDto>()
                .ForMember(dest => dest.NombreMascotas, opt => opt.MapFrom(src => src.Mascotas.Select(m => m.Nombre)))
                .ForMember(dest => dest.MascotasId, opt => opt.MapFrom(src => src.Mascotas.Select(m => m.Id)));
            CreateMap<CrearTutorDto, Tutor>();
            CreateMap<Veterinario, VeterinarioDto>();
            CreateMap<CrearVeterinarioDto, Veterinario>();
            CreateMap<Servicio,ServicioDto>();
            CreateMap<CrearServicioDto, Servicio>();


        }
    }
}
