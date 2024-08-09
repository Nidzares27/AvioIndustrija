using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Table("Dio")]
    public partial class Dio
    {
        public Dio()
        {
            ServisDios = new HashSet<ServisDio>();
        }

        [Key]
        public int SerijskiBroj { get; set; }
        [StringLength(50)]
        public string Naziv { get; set; } = null!;
        [StringLength(50)]
        public string Model { get; set; } = null!;
        [StringLength(50)]
        public string Proizvodjac { get; set; } = null!;
        public int GodinaProizvodnja { get; set; }

        [InverseProperty("SerijskiBrojNavigation")]
        public virtual ICollection<ServisDio> ServisDios { get; set; }
    }
}
