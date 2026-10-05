namespace Veterinaria_API.Models
{
    public class TurnoServicio
    {
        public int Id { set; get; }
        public int TurnoId { set; get; }
        public int ServicioId { set; get; }
        public Turno Turno { set; get; }
        public Servicio Servicio { set; get; }
    }
}
