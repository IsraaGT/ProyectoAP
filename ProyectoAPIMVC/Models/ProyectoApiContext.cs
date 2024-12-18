using Microsoft.EntityFrameworkCore;

namespace ProyectoAPIMVC.Models;

public partial class ProyectoApiContext : DbContext
{
    public ProyectoApiContext(DbContextOptions<ProyectoApiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrera> Carreras { get; set; }
    public virtual DbSet<Equipo> Equipos { get; set; }
    public virtual DbSet<Piloto> Pilotos { get; set; }
    public virtual DbSet<ResultadoCarrera> ResultadoCarreras { get; set; }
    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrera>(entity =>
        {
            entity.HasKey(e => e.Idcarrera).HasName("PK__Carreras__C9EBEE6416D64B4F");
            entity.Property(e => e.Idcarrera).HasColumnName("IDCarrera");
            entity.Property(e => e.NombreCarrera).HasMaxLength(100);
            entity.Property(e => e.Ubicación).HasMaxLength(100);
        });

        modelBuilder.Entity<Equipo>(entity =>
        {
            entity.HasKey(e => e.Idequipo).HasName("PK__Equipos__034DC40FC2A52469");
            entity.Property(e => e.Idequipo).HasColumnName("IDEquipo");
            entity.Property(e => e.Director).HasMaxLength(100);
            entity.Property(e => e.NombreEquipo).HasMaxLength(100);
            entity.Property(e => e.País).HasMaxLength(50);
        });

        modelBuilder.Entity<Piloto>(entity =>
        {
            entity.HasKey(e => e.Idpiloto).HasName("PK__Pilotos__7DA4E419EF77CA46");
            entity.Property(e => e.Idpiloto).HasColumnName("IDPiloto");
            entity.Property(e => e.Idequipo).HasColumnName("IDEquipo");
            entity.Property(e => e.Nacionalidad).HasMaxLength(50);
            entity.Property(e => e.NombrePiloto).HasMaxLength(100);

            entity.HasOne(d => d.IdequipoNavigation)
                .WithMany(p => p.Pilotos)
                .HasForeignKey(d => d.Idequipo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pilotos__IDEquip__5070F446");
        });

        modelBuilder.Entity<ResultadoCarrera>(entity =>
        {
            entity.HasKey(e => e.Idresultado).HasName("PK__Resultad__CA8EAAD7478F1A3A");
            entity.ToTable("ResultadoCarrera");
            entity.Property(e => e.Idresultado).HasColumnName("IDResultado");
            entity.Property(e => e.Idcarrera).HasColumnName("IDCarrera");
            entity.Property(e => e.Idpiloto).HasColumnName("IDPiloto");
            entity.Property(e => e.TiempoFinal)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdcarreraNavigation)
                .WithMany(p => p.ResultadoCarreras)
                .HasForeignKey(d => d.Idcarrera)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resultado__IDCar__5535A963");

            entity.HasOne(d => d.IdpilotoNavigation)
                .WithMany(p => p.ResultadoCarreras)
                .HasForeignKey(d => d.Idpiloto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Resultado__IDPil__5629CD9C");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Idusuario).HasName("PK__Usuario__52311169981D1380");
            entity.ToTable("Usuario");
            entity.HasIndex(e => e.Correo, "UQ__Usuario__60695A199AFF41CD").IsUnique();
            entity.HasIndex(e => e.NombreUsuario, "UQ__Usuario__6B0F5AE0CD910DE6").IsUnique();
            entity.Property(e => e.Idusuario).HasColumnName("IDUsuario");
            entity.Property(e => e.Contraseña).HasMaxLength(64);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
