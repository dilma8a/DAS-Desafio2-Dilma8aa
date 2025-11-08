using Microsoft.EntityFrameworkCore;
using DAS_Desafio2_Dilma8a.Models;

namespace DAS_Desafio2_Dilma8a.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Alumno> Alumnos { get; set; } = null!;
        public DbSet<Materia> Materias { get; set; } = null!;
        public DbSet<Expediente> Expedientes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Alumno>(entity =>
            {
                entity.ToTable("Alumno");
                entity.HasKey(e => e.AlumnoId);
                entity.Property(e => e.Nombre).HasMaxLength(100).IsRequired();
                // Índice único compuesto para evitar alumnos con el mismo Nombre + Apellido
                entity.HasIndex(e => new { e.Nombre, e.Apellido }).IsUnique();
                entity.Property(e => e.Apellido).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Grado).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Materia>(entity =>
            {
                entity.ToTable("Materia");
                entity.HasKey(e => e.MateriaId);
                entity.Property(e => e.NombreMateria).HasMaxLength(150).IsRequired();
                // Índice único para evitar materias con el mismo nombre
                entity.HasIndex(e => e.NombreMateria).IsUnique();
                entity.Property(e => e.Docente).HasMaxLength(100);
            });

            modelBuilder.Entity<Expediente>(entity =>
            {
                entity.ToTable("Expediente");
                entity.HasKey(e => e.ExpedienteId);
                // Evitar borrado en cascada: no permitir eliminar Alumno o Materia si hay Expedientes relacionados
                entity.HasOne(e => e.Alumno)
                    .WithMany(a => a.Expedientes)
                    .HasForeignKey(e => e.AlumnoId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Materia)
                    .WithMany(m => m.Expedientes)
                    .HasForeignKey(e => e.MateriaId)
                    .OnDelete(DeleteBehavior.Restrict);
                entity.Property(e => e.NotaFinal).HasColumnType("decimal(5,2)");
                entity.Property(e => e.Observaciones).HasMaxLength(500);
            });
        }
    }
}
