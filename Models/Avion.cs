using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Avion")]
    [Index("GodinaProizvodnje", Name = "IX_Avion_GodinaProizvodnje")]
    public partial class Avion
    {
        public Avion()
        {
            Lets = new HashSet<Let>();
        }

        [Key]
        [Column("AvionID")]
        public int AvionId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string ImeAviona { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Kompanija { get; set; } = null!;
        public short GodinaProizvodnje { get; set; }
        [Column("Visina(m)")]
        public short VisinaM { get; set; }
        [Column("Širina(m)")]
        public short ŠirinaM { get; set; }
        [Column("Dužina(m)")]
        public short DužinaM { get; set; }
        public short BrojSjedištaBiznisKlase { get; set; }
        public short BrojSjedištaEkonomskeKlase { get; set; }
        [Column("Nosivost(kg)")]
        public short NosivostKg { get; set; }
        [Column("KapacitetRezervoara(L)")]
        public short KapacitetRezervoaraL { get; set; }
        [Column("MaksimalniDomet(km)")]
        public short MaksimalniDometKm { get; set; }
        [Column("ImageURL")]
        [Unicode(false)]
        public string? ImageUrl { get; set; }

        [InverseProperty("Avion")]
        public virtual ICollection<Let> Lets { get; set; }

        [InverseProperty("Avion")]
        public virtual ICollection<Servi> Servis { get; set; }
    }
}
