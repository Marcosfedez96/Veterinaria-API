using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Veterinaria_API.Models;

namespace Veterinaria_API.DTOs
{
    public class CrearMascotaDto
    {
        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "El nombre debe tener minimo 2 y maximo 50 Caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La especie Es Obligatoria.")]
        public string Especie { get; set; }

        [Range(0, 40, ErrorMessage = "La edad debe estar entre 0 y 40 años.")]
        public int Edad { get; set; }

        public int TutorId { set; get; }
    }
}
