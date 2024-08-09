using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Putnik")]
    [Index("Email", Name = "IX_Putnik_Email", IsUnique = true)]
    public partial class Putnik
    {
        public Putnik()
        {
            IstorijaLetovaPutnikas = new HashSet<IstorijaLetovaPutnika>();
        }

        [Key]
        public int PutnikId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Ime { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Prezime { get; set; } = null!;
        [StringLength(10)]
        [Unicode(false)]
        public string? Pol { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Email { get; set; }
        [Column("ImageURL")]
        [Unicode(false)]
        public string? ImageUrl { get; set; }

        [InverseProperty("Putnik")]
        public virtual ICollection<IstorijaLetovaPutnika> IstorijaLetovaPutnikas { get; set; }
    }
}
