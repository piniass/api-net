using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace apinet.Models;

public partial class BddMiguelpContext : DbContext
{
    public BddMiguelpContext()
    {
    }

    public BddMiguelpContext(DbContextOptions<BddMiguelpContext> options)
        : base(options)
    {
    }



    public virtual DbSet<Marcazapa> Marcazapas { get; set; }


    public virtual DbSet<Modelozapatilla> Modelozapatillas { get; set; }






    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { } /*
#warning  To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=db4free.net;database=bdd_miguelp;user id=miguel_pinan;password=palomeras98", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.3.0-mysql"));
    */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");



        modelBuilder.Entity<Marcazapa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("marcazapa");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });


        modelBuilder.Entity<Modelozapatilla>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("modelozapatilla")
                .UseCollation("utf8mb4_general_ci");

            entity.HasIndex(e => e.IdMarca, "ID_Marca");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.IdMarca).HasColumnName("ID_Marca");
            entity.Property(e => e.Imagen).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(255);
            entity.Property(e => e.Precio).HasPrecision(10, 2);

            entity.HasOne(d => d.oMarcaZapa).WithMany(p => p.Modelozapatillas)
                .HasForeignKey(d => d.IdMarca)
                .HasConstraintName("modelozapatilla_ibfk_1");
        });






        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
