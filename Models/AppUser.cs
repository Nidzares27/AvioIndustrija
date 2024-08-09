using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AvioIndustrija.Models
{
    [Keyless]
    [Table("AppUser")]
    public partial class AppUser : IdentityUser
    {
        [StringLength(50)]
        [Unicode(false)]
        public string Ime { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Prezime { get; set; } = null!;
    }
}
