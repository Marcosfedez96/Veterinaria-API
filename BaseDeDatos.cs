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
        public static List<Veterinario> veterinarios = new List<Veterinario>
        {
            new Veterinario {Id = 1,Nombre = "Andrea Villalva",Especialidad = "Cirugía",Matricula="MP-1023" },
            new Veterinario {Id = 2,Nombre = "Martín Suárez",Especialidad = "Clínica general",Matricula="MP-1044" },
            new Veterinario {Id = 3,Nombre = "Lucía Ferrero",Especialidad = "Dermatología",Matricula="MP-1067" },
            new Veterinario {Id = 4,Nombre = "Gonzalo Pereyra",Especialidad = "Traumatología",Matricula="MP-1089" }
        };
    }
}
