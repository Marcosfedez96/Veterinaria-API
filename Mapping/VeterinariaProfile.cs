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
        }
    }
}
