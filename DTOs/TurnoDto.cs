using Veterinaria_API.Models;

namespace Veterinaria_API.DTOs
{
    public class TurnoDto
    {
        public int Id { get; set; }
        public DateOnly DiaTurno { set; get; }
        public TimeOnly HoraTurno { set; get; }
        public int VeterinarioId { set; get; }
        public string VeterinarioNombre { set; get; }
        public int MascotaId { set; get; }
        public string MascotaNombre { set; get; }
        public List<string> Servicios { get; set; } = new();
    }
}
