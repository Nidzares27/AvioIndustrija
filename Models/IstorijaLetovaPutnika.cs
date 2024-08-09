using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("IstorijaLetovaPutnika")]
    public partial class IstorijaLetovaPutnika
    {
        [Key]
        [Column("PutnikID")]
        public int PutnikId { get; set; }
        [Key]
        [Column("LetID")]
        public int LetId { get; set; }
        public short RedniBrojSjedišta { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Klasa { get; set; } = null!;
        [Column("RučniPrtljag(<8kg)")]
        [StringLength(10)]
        [Unicode(false)]
        public string? RučniPrtljag8kg { get; set; }
        [Column("Koferi(kg)")]
        public byte? KoferiKg { get; set; }

        [ForeignKey("LetId")]
        [InverseProperty("IstorijaLetovaPutnikas")]
        public virtual Let Let { get; set; } = null!;
        [ForeignKey("PutnikId")]
        [InverseProperty("IstorijaLetovaPutnikas")]
        public virtual Putnik Putnik { get; set; } = null!;
    }
}
