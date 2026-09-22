using Veterinaria_API.Models;

namespace Veterinaria_API
{
    public static class BaseDeDatos
    {
        public static List<Mascota> mascotas = new List<Mascota>
        {
            new Mascota { Id = 1, Nombre = "Firulais", Especie = "Perro", Edad = 3 },
            new Mascota { Id = 2, Nombre = "Michi", Especie = "Gato", Edad = 2 },
            new Mascota { Id = 3, Nombre = "Nemo", Especie = "Pez", Edad = 1 }
        };
    }
}
