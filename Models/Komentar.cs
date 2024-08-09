using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Komentar")]
    public partial class Komentar
    {
        [Key]
        [Column("KomentarID")]
        public int KomentarId { get; set; }
        public string Sadrzaj { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime Vrijeme { get; set; }
        [Column("VijestID")]
        public int VijestId { get; set; }
        [Column("KorisnikID")]
        [StringLength(450)]
        public string KorisnikId { get; set; } = null!;
        public int? NadKomentar { get; set; }

        [ForeignKey("VijestId")]
        [InverseProperty("Komentars")]
        public virtual Vijest Vijest { get; set; } = null!;
    }
}

