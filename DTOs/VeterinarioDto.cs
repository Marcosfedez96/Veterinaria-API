using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Veterinaria_API.Models;

namespace Veterinaria_API.DTOs
{
    public class VeterinarioDto
    {
        public int Id { set; get; }
        public string Nombre { set; get; }
        public required string Especialidad { set; get; }

        public string Matricula { set; get; }
        public bool EsPracticante { get; set; }
        /*proximos turnos con fecha y hora 
         */
    }
}
