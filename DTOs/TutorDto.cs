using Veterinaria_API.Models;

namespace Veterinaria_API.DTOs
{
    public class TutorDto
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public string Telefono { set; get; }
        public string Email { set; get; }
        public List<int> MascotasId { set; get; }
        public List<string> NombreMascotas { set; get; }
    }
}
