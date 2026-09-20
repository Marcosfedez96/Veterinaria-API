using System.ComponentModel.DataAnnotations;

namespace Veterinaria_API.Models
{
    public class Mascota
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(50,MinimumLength = 2, ErrorMessage = "El nombre debe tener minimo 2 y maximo 50 Caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage="La especie Es Obligatoria.")]
        public string Especie { get; set; }

        [Range(0,40,ErrorMessage="La edad debe estar entre 0 y 40 años.")]
        public int Edad { get; set; }
    }
}
