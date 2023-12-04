using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class PracticaDeConocimientosYHabilidadesContext : DbContext
{
    public PracticaDeConocimientosYHabilidadesContext()
    {
    }

    public PracticaDeConocimientosYHabilidadesContext(DbContextOptions<PracticaDeConocimientosYHabilidadesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetallesPedido> DetallesPedidos { get; set; }

    public virtual DbSet<Medicamento> Medicamentos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Rol> Rols { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-CHR9HTDV; Database=Practica de Conocimientos y Habilidades; TrustServerCertificate=True; User ID=sa; Password=pass@word1;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetallesPedido>(entity =>
        {
            entity.HasKey(e => e.DetallesPedidoId).HasName("PK__Detalles__CD309D2D5C92CB23");

            entity.ToTable("DetallesPedido");

            entity.Property(e => e.DetallesPedidoId).HasColumnName("DetallesPedidoID");
            entity.Property(e => e.MedicamentosId).HasColumnName("MedicamentosID");
            entity.Property(e => e.PedidosId).HasColumnName("PedidosID");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Medicamentos).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.MedicamentosId)
                .HasConstraintName("FK__DetallesP__Medic__182C9B23");

            entity.HasOne(d => d.Pedidos).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.PedidosId)
                .HasConstraintName("FK__DetallesP__Pedid__173876EA");
        });

        modelBuilder.Entity<Medicamento>(entity =>
        {
            entity.HasKey(e => e.MedicamentosId).HasName("PK__Medicame__07B203FCD63316A6");

            entity.Property(e => e.MedicamentosId).HasColumnName("MedicamentosID");
            entity.Property(e => e.Imagen).IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("SKU");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.PedidosId).HasName("PK__Pedidos__6256AA0B2A1C3086");

            entity.Property(e => e.PedidosId).HasColumnName("PedidosID");
            entity.Property(e => e.Fecha).HasColumnType("date");
            entity.Property(e => e.UsuariosId).HasColumnName("UsuariosID");

            entity.HasOne(d => d.Usuarios).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuariosId)
                .HasConstraintName("FK__Pedidos__Usuario__145C0A3F");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__2A49584C6D852E75");

            entity.ToTable("Rol");

            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuariosId).HasName("PK__Usuarios__48BE79E9FD883A78");

            entity.Property(e => e.UsuariosId).HasColumnName("UsuariosID");
            entity.Property(e => e.ApellidoMaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ApellidoPaterno)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contraseña)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuarios__IdRol__1B0907CE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
