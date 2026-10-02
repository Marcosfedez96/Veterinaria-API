using System.ComponentModel.DataAnnotations;
using Veterinaria_API.Models;

namespace Veterinaria_API.DTOs
{
    public class MascotaDto
    {
        public int Id { get; set; }        
        public string Nombre { get; set; }
        public string Especie { get; set; }
        public int Edad { get; set; }
        public int TutorId { set; get; }
        public string TutorNombre { set; get; }
    }
}
