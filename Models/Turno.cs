namespace Veterinaria_API.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public DateOnly DiaTurno { set; get; }
        public TimeOnly HoraTurno { set; get; }
        public Veterinario Veterianario { set; get; }
        public Mascota Mascota { set; get; }
    }
}
