using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IntegraMuni2023._01.Models;

public partial class IntegraMuni2023Context : DbContext
{
    public IntegraMuni2023Context()
    {
    }

    public IntegraMuni2023Context(DbContextOptions<IntegraMuni2023Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Proyecto> Proyectos { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<ServiciosContratado> ServiciosContratados { get; set; }

    public virtual DbSet<Tarea> Tareas { get; set; }

    public virtual DbSet<Tramite> Tramites { get; set; }

    public virtual DbSet<PagosTramites> PagosTramites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Clientes__71ABD0A75F8D6E01");

            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Clave).IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Identificacion)
                .HasMaxLength(11)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(70)
                .IsUnicode(false);
            entity.Property(e => e.Telefono)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__958BE6F0A583FEAB");

            entity.HasIndex(e => e.CorreoElectronico, "UQ__Empleado__531402F3414950AE").IsUnique();

            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CorreoElectronico)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B6158D58C2CA2");

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.EstadoPago)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Identificacion).IsUnicode(false);
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.MontoPago).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServicioContratadoId).HasColumnName("ServicioContratadoID");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__ClienteID__300424B4");

            entity.HasOne(d => d.ServicioContratado).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.ServicioContratadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__ServicioC__2F10007B");
        });

        modelBuilder.Entity<Proyecto>(entity =>
        {
            entity.HasKey(e => e.NoticiaId).HasName("PK__Proyecto__F33000EF6FEC6216");

            entity.Property(e => e.NoticiaId).HasColumnName("NoticiaID");
            entity.Property(e => e.Desarrollo).HasColumnType("text");
            entity.Property(e => e.Estado)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.NivelPrioridad)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TituloNoticia)
                .HasMaxLength(300)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEEC225AA6B496");

            entity.Property(e => e.ServicioId)
                .ValueGeneratedNever()
                .HasColumnName("ServicioID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.NombreServicio)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Tarifa).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoTarifa)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServiciosContratado>(entity =>
        {
            entity.HasKey(e => e.ServicioContratadoId).HasName("PK__Servicio__872007F9577521DD");

            entity.Property(e => e.ServicioContratadoId).HasColumnName("ServicioContratadoID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Consumo).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.EstadoServicio)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ServicioId).HasColumnName("ServicioID");

            entity.HasOne(d => d.Cliente).WithMany(p => p.ServiciosContratados)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Servicios__Clien__2C3393D0");

            entity.HasOne(d => d.Servicio).WithMany(p => p.ServiciosContratados)
                .HasForeignKey(d => d.ServicioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Servicios__Servi__2B3F6F97");
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.TareaId).HasName("PK__Tareas__5CD836719685CBB5");

            entity.Property(e => e.TareaId).HasColumnName("TareaID");
            entity.Property(e => e.Descripcion).HasColumnType("text");
            entity.Property(e => e.EmpleadoId).HasColumnName("EmpleadoID");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Prioridad)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Titulo)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Empleado).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tareas__Empleado__35BCFE0A");
        });

        modelBuilder.Entity<Tramite>(entity =>
        {
            entity.HasKey(e => e.TramiteId).HasName("PK__Tramites__BDB793F660E72B4F");

            entity.Property(e => e.TramiteId).HasColumnName("TramiteID");
            entity.Property(e => e.ClienteId).HasColumnName("ClienteID");
            entity.Property(e => e.Descripcion).IsUnicode(false);
            entity.Property(e => e.FormularioPagoCompletado)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.Identificacion).IsUnicode(false);
            entity.Property(e => e.PagoPendiente)
                .HasMaxLength(5)
                .IsUnicode(false);

            entity.HasOne(d => d.Cliente).WithMany(p => p.Tramites)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Tramites__Client__267ABA7A");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
