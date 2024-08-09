using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Relacija")]
    public partial class Relacija
    {
        public Relacija()
        {
            Lets = new HashSet<Let>();
        }

        [Key]
        [Column("RelacijaID")]
        public int RelacijaId { get; set; }
        [Column("Grad(od)")]
        [StringLength(50)]
        [Unicode(false)]
        public string GradOd { get; set; } = null!;
        [Column("Država(od)")]
        [StringLength(50)]
        [Unicode(false)]
        public string DržavaOd { get; set; } = null!;
        [Column("Aerodrom(od)")]
        [StringLength(50)]
        [Unicode(false)]
        public string AerodromOd { get; set; } = null!;
        [Column("Grad(do)")]
        [StringLength(50)]
        [Unicode(false)]
        public string GradDo { get; set; } = null!;
        [Column("Država(do)")]
        [StringLength(50)]
        [Unicode(false)]
        public string DržavaDo { get; set; } = null!;
        [Column("Aerodrom(do)")]
        [StringLength(50)]
        [Unicode(false)]
        public string AerodromDo { get; set; } = null!;
        [Column("Udaljenost(km)")]
        public short UdaljenostKm { get; set; }

        [InverseProperty("Relacija")]
        public virtual ICollection<Let> Lets { get; set; }
    }
}
