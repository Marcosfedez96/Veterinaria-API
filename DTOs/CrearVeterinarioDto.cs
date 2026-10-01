using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Veterinaria_API.Models;

namespace Veterinaria_API.DTOs
{
    public class CrearVeterinarioDto
    {
        [Required(ErrorMessage = "El Nombre es obligatorio")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Numero maximo de caracteres excedido.")]
        public string Nombre { set; get; }
        public required string Especialidad { set; get; }

        public string Matricula { set; get; }
        [Required(ErrorMessage = "Es necesario saber si es practicante o no.")]
        public bool EsPracticante { get; set; }
    }
}
