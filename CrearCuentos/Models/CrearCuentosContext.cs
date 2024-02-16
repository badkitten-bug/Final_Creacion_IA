using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CrearCuentos.Models;

public partial class CrearCuentosContext : DbContext
{
    public CrearCuentosContext()
    {
    }

    public CrearCuentosContext(DbContextOptions<CrearCuentosContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<Usuario> Usuarios { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=161.132.41.80;port=3306;database=CrearCuentos;uid=root;password=Thewarriors900", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.5.21-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("ID");
            entity.Property(e => e.Contraseña).HasMaxLength(255);
            entity.Property(e => e.Correo).HasMaxLength(255);
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .HasColumnName("DNI");
            entity.Property(e => e.Estado).HasColumnType("bit(1)");
            entity.Property(e => e.NombreCompletos).HasMaxLength(255);
            entity.Property(e => e.NombreDeUsuario).HasMaxLength(50);
            entity.HasData(new Usuario
            {
                Id = 1,
                NombreCompletos = "Emilio Granada",
                Dni = "123123123",
                NombreDeUsuario = "educadordrayo@gmail.com",
                Correo = "educadordrayo@gmail.com",
                Contraseña = "Edasac@20",
                PermisoInicial = true,
                PermisoPrimaria = true,
                PermisoSecundaria = true,
                Estado = 1
            });
            ;
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
