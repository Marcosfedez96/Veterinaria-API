using Microsoft.EntityFrameworkCore;
namespace Veterinaria_API.Models
{
    public class VeterinariaContext : DbContext
    {
        public VeterinariaContext(DbContextOptions<VeterinariaContext> options) : base(options)
        {

        }
        public DbSet<Mascota> Mascotas { set; get; }
        public DbSet<Veterinario> Veterinarios { set; get; }
        public DbSet<Tutor> Tutores { set; get; }
        public DbSet<Turno> Turnos { set;get; }
    }
}
