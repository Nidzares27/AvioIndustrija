using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    public partial class Servi
    {
        public Servi()
        {
            ServisDios = new HashSet<ServisDio>();
        }

        [Key]
        [Column("ServisID")]
        public int ServisId { get; set; }
        [Column("AvionID")]
        public int AvionId { get; set; }
        [Column(TypeName = "date")]
        public DateTime DatumServisa { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Status { get; set; } = null!;
        public string? Komentar { get; set; }
        [ForeignKey("AvionId")]
        [InverseProperty("Servis")]
        public virtual Avion Avion { get; set; } = null!;
        [InverseProperty("Servis")]
        public virtual ICollection<ServisDio> ServisDios { get; set; }
    }
}
