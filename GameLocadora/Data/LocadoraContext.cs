using System;
using System.Collections.Generic;
using GameLocadora.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLocadora.Data;

public partial class LocadoraContext : DbContext
{
    public LocadoraContext(DbContextOptions<LocadoraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Jogo> Jogos { get; set; }

    public virtual DbSet<Locacao> Locacaos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");

            entity.Property(e => e.Ativo).HasColumnType("BOOLEAN");
        });

        modelBuilder.Entity<Jogo>(entity =>
        {
            entity.ToTable("Jogo");

            entity.Property(e => e.Ativo).HasColumnType("BOOLEAN");
        });

        modelBuilder.Entity<Locacao>(entity =>
        {
            entity.ToTable("Locacao");

            entity.HasIndex(e => e.ClienteId, "IX_Locacao_ClienteId");

            entity.HasIndex(e => e.JogoId, "UX_Locacao_JogoAberto").IsUnique();

            entity.Property(e => e.DataDevolucao).HasColumnType("DATETIME");
            entity.Property(e => e.DataLocacao).HasColumnType("DATETIME");
            entity.Property(e => e.DataPrevistaDevolucao).HasColumnType("DATETIME");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Locacaos)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(d => d.Jogo).WithOne(p => p.Locacao)
                .HasForeignKey<Locacao>(d => d.JogoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
