using System.Text.Json.Serialization;

namespace Veterinaria_API.Models;
public class Tutor
{
    public int Id { set; get; }
    public string Nombre { set; get; }
    public string Telefono { set; get; }
    public string Email { set; get; }
    public List<Mascota> Mascotas { set; get; } = new();

}

