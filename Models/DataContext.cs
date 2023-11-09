using Microsoft.EntityFrameworkCore;

namespace API_asistecia;

public class DataContext : DbContext {
    public DataContext(DbContextOptions<DataContext> options) : base(options) {}

    public DbSet<Usuario> usuarios { get; set; }
    public DbSet<Genero> generos { get; set; }
    public DbSet<EstCivil> estCiviles { get; set; }
    public DbSet<Rol> roles { get; set; }
    public DbSet<Ingreso> ingresos { get; set; }
    public DbSet<Asistencia> asistencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Asistencia>().HasKey(asis => new { asis.codIngreso, asis.idUsuario });
    }
}