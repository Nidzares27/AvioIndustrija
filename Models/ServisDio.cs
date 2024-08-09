using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("ServisDio")]
    public partial class ServisDio
    {
        [Key]
        [Column("ServisID")]
        public int ServisId { get; set; }
        [Key]
        public int SerijskiBroj { get; set; }
        public int? Kolicina { get; set; }

        [ForeignKey("SerijskiBroj")]
        [InverseProperty("ServisDios")]
        public virtual Dio SerijskiBrojNavigation { get; set; } = null!;
        [ForeignKey("ServisId")]
        [InverseProperty("ServisDios")]
        public virtual Servi Servis { get; set; } = null!;
    }
}

