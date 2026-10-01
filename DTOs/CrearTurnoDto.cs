using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Veterinaria_API.Models;

namespace Veterinaria_API.DTOs
{
    public class CrearTurnoDto
    {
        public DateOnly DiaTurno { set; get; }
        public TimeOnly HoraTurno { set; get; }
        public int VeterinarioId { set; get; }
        public int MascotaId { set; get; }
    }
}
