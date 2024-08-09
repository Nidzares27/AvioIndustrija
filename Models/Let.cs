using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Let")]
    [Index("VrijemePoletanja", Name = "IX_Let_VrijemePoletanja")]
    public partial class Let
    {
        public Let()
        {
            IstorijaLetovaPutnikas = new HashSet<IstorijaLetovaPutnika>();
        }

        [Key]
        [Column("LetID")]
        public int LetId { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string BrojLeta { get; set; } = null!;
        [Column("AvionID")]
        public int AvionId { get; set; }
        [Column("RelacijaID")]
        public int RelacijaId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime VrijemePoletanja { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime VrijemeSletanja { get; set; }

        [ForeignKey("AvionId")]
        [InverseProperty("Lets")]
        public virtual Avion Avion { get; set; } = null!;
        [ForeignKey("RelacijaId")]
        [InverseProperty("Lets")]
        public virtual Relacija Relacija { get; set; } = null!;
        [InverseProperty("Let")]
        public virtual ICollection<IstorijaLetovaPutnika> IstorijaLetovaPutnikas { get; set; }
    }
}
