using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Ocjena")]
    public partial class Ocjena
    {
        [Key]
        [Column("OcjenaID")]
        public int OcjenaId { get; set; }
        public int Zvijezda { get; set; }
        [Column("VijestID")]
        public int VijestId { get; set; }
        [Column("KorisnikID")]
        [StringLength(450)]
        public string KorisnikId { get; set; } = null!;

        [ForeignKey("VijestId")]
        [InverseProperty("Ocjenas")]
        public virtual Vijest Vijest { get; set; } = null!;
    }
}
