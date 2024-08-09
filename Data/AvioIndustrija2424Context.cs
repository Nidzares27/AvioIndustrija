using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using AvioIndustrija.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AvioIndustrija.Data
{
    public partial class AvioIndustrija2424Context : IdentityDbContext<AppUser>
    {
        public AvioIndustrija2424Context()
        {
        }

        public AvioIndustrija2424Context(DbContextOptions<AvioIndustrija2424Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<Avion> Avions { get; set; } = null!;
        public virtual DbSet<Dio> Dios { get; set; } = null!;
        public virtual DbSet<IstorijaLetovaPutnika> IstorijaLetovaPutnikas { get; set; } = null!;
        public virtual DbSet<Komentar> Komentars { get; set; } = null!;
        public virtual DbSet<Let> Lets { get; set; } = null!;
        public virtual DbSet<Ocjena> Ocjenas { get; set; } = null!;
        public virtual DbSet<Putnik> Putniks { get; set; } = null!;
        public virtual DbSet<Relacija> Relacijas { get; set; } = null!;
        public virtual DbSet<Servi> Servis { get; set; } = null!;
        public virtual DbSet<ServisDio> ServisDios { get; set; } = null!;
        public virtual DbSet<VWletoviPutnika> VWletoviPutnikas { get; set; } = null!;
        public virtual DbSet<VWrelacijeSaBrojemPutnikaPoKlasama> VWrelacijeSaBrojemPutnikaPoKlasamas { get; set; } = null!;
        public virtual DbSet<Vijest> Vijests { get; set; } = null!;

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        //                optionsBuilder.UseSqlServer("Data Source=.\\sqlexpress;Initial Catalog=AvioIndustrija2424;Integrated Security=True");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IstorijaLetovaPutnika>(entity =>
            {
                base.OnModelCreating(modelBuilder);
                entity.HasKey(e => new { e.PutnikId, e.LetId });

                entity.Property(e => e.RučniPrtljag8kg)
                    .HasDefaultValueSql("('NE')")
                    .IsFixedLength();

                entity.HasOne(d => d.Let)
                    .WithMany(p => p.IstorijaLetovaPutnikas)
                    .HasForeignKey(d => d.LetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IstorijaLetovaPutnika_Let");

                entity.HasOne(d => d.Putnik)
                    .WithMany(p => p.IstorijaLetovaPutnikas)
                    .HasForeignKey(d => d.PutnikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IstorijaLetovaPutnika_Putnik");
            });

            modelBuilder.Entity<Komentar>(entity =>
            {
                //entity.HasOne(d => d.Korisnik)
                //    .WithMany(p => p.Komentars)
                //    .HasForeignKey(d => d.KorisnikId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Komentar_AspNetUsers");

                entity.HasOne(d => d.Vijest)
                    .WithMany(p => p.Komentars)
                    .HasForeignKey(d => d.VijestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Komentar_Vijest");
            });


            modelBuilder.Entity<Let>(entity =>
            {
                entity.HasOne(d => d.Avion)
                    .WithMany(p => p.Lets)
                    .HasForeignKey(d => d.AvionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Let_Avion");

                entity.HasOne(d => d.Relacija)
                    .WithMany(p => p.Lets)
                    .HasForeignKey(d => d.RelacijaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Let_Relacija");
            });

            modelBuilder.Entity<Ocjena>(entity =>
            {
                //entity.HasOne(d => d.Korisnik)
                //    .WithMany(p => p.Ocjenas)
                //    .HasForeignKey(d => d.KorisnikId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Ocjena_AspNetUsers");

                entity.HasOne(d => d.Vijest)
                    .WithMany(p => p.Ocjenas)
                    .HasForeignKey(d => d.VijestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Ocjena_Vijest");
            });


            modelBuilder.Entity<Putnik>(entity =>
            {
                entity.Property(e => e.Pol).IsFixedLength();
            });

            modelBuilder.Entity<Servi>(entity =>
            {
                entity.HasOne(d => d.Avion)
                    .WithMany(p => p.Servis)
                    .HasForeignKey(d => d.AvionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Servis_Avion");
            });

            modelBuilder.Entity<ServisDio>(entity =>
            {
                entity.HasKey(e => new { e.ServisId, e.SerijskiBroj });

                entity.HasOne(d => d.SerijskiBrojNavigation)
                    .WithMany(p => p.ServisDios)
                    .HasForeignKey(d => d.SerijskiBroj)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServisDio_Dio");

                entity.HasOne(d => d.Servis)
                    .WithMany(p => p.ServisDios)
                    .HasForeignKey(d => d.ServisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServisDio_Servis");
            });


            modelBuilder.Entity<VWletoviPutnika>(entity =>
            {
                entity.ToView("vWLetoviPutnika");

                entity.Property(e => e.RučniPrtljag8kg).IsFixedLength();
            });

            modelBuilder.Entity<VWrelacijeSaBrojemPutnikaPoKlasama>(entity =>
            {
                entity.ToView("vWRelacijeSaBrojemPutnikaPoKlasama");
            });

            modelBuilder.Entity<Vijest>(entity =>
            {
                //entity.HasOne(d => d.Korisnik)
                //    .WithMany(p => p.Vijests)
                //    .HasForeignKey(d => d.KorisnikId)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("FK_Vijest_AspNetUsers");
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
