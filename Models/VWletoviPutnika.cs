using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Keyless]
    public partial class VWletoviPutnika
    {
        [StringLength(50)]
        [Unicode(false)]
        public string? Ime { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Prezime { get; set; }
        [Column("Aerodrom(od)")]
        [StringLength(50)]
        [Unicode(false)]
        public string? AerodromOd { get; set; }
        [Column("OD")]
        [StringLength(102)]
        [Unicode(false)]
        public string? Od { get; set; }
        [Column("Aerodrom(do)")]
        [StringLength(50)]
        [Unicode(false)]
        public string? AerodromDo { get; set; }
        [Column("DO")]
        [StringLength(102)]
        [Unicode(false)]
        public string? Do { get; set; }
        [StringLength(103)]
        [Unicode(false)]
        public string? Avion { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string? BrojLeta { get; set; }
        [Column("Vrijeme Poletanja")]
        [StringLength(56)]
        [Unicode(false)]
        public string? VrijemePoletanja { get; set; }
        [Column("Vrijeme Sletanja")]
        [StringLength(56)]
        [Unicode(false)]
        public string? VrijemeSletanja { get; set; }
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
    }
}
