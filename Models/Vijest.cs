using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Vijest")]
    public partial class Vijest
    {
        public Vijest()
        {
            Komentars = new HashSet<Komentar>();
            Ocjenas = new HashSet<Ocjena>();
        }

        [Key]
        [Column("VijestID")]
        public int VijestId { get; set; }
        public string Naslov { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        [Column(TypeName = "datetime")]
        public DateTime VrijemeObjave { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int BrojPregleda { get; set; }
        public double ProsjecnaOcjena { get; set; }
        [Column("KorisnikID")]
        [StringLength(450)]
        public string KorisnikId { get; set; } = null!;

        [InverseProperty("Vijest")]
        public virtual ICollection<Komentar> Komentars { get; set; }
        [InverseProperty("Vijest")]
        public virtual ICollection<Ocjena> Ocjenas { get; set; }
    }
}

