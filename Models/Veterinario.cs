using System.ComponentModel.DataAnnotations;
namespace Veterinaria_API.Models
{
    public class Veterinario
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [StringLength(20,MinimumLength = 2,ErrorMessage = "Numero maximo de caracteres excedido.")]
        public string Nombre { set; get; }
        public required string Especialidad { set; get; }

        public string Matricula { set; get; }
        [Required(ErrorMessage ="Es necesario saber si es practicante o no.")]
        public bool EsPracticante { get; set; }
    }
}
