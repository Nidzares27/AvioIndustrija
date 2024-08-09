using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Keyless]
    public partial class VWrelacijeSaBrojemPutnikaPoKlasama
    {
        [Column("RelacijaID")]
        public int? RelacijaId { get; set; }
        [Column("Grad(od)")]
        [StringLength(50)]
        [Unicode(false)]
        public string? GradOd { get; set; }
        [Column("Država(od)")]
        [StringLength(50)]
        [Unicode(false)]
        public string? DržavaOd { get; set; }
        [Column("Grad(do)")]
        [StringLength(50)]
        [Unicode(false)]
        public string? GradDo { get; set; }
        [Column("Država(do)")]
        [StringLength(50)]
        [Unicode(false)]
        public string? DržavaDo { get; set; }
        [Column("Broj Putnika")]
        public int? BrojPutnika { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Klasa { get; set; } = null!;
    }
}
