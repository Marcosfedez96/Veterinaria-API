using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Veterinaria_API.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public DateOnly DiaTurno { set; get; }
        public TimeOnly HoraTurno { set; get; }
        public int VeterinarioId { set; get; }
        public int MascotaId { set; get; }
        [ValidateNever]
        public Veterinario Veterianario { set; get; }
        [ValidateNever]
        public Mascota Mascota { set; get; }
    }
}
