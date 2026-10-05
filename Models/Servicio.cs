namespace Veterinaria_API.Models
{
    public class Servicio
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public decimal Precio { set; get; }

        public List<TurnoServicio> TurnoServicio { set; get; } = new();
    }
}
