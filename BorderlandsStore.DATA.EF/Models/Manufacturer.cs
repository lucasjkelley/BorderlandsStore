using System;
using System.Collections.Generic;

namespace BorderlandsStore.DATA.EF.Models
{
    public partial class Manufacturer
    {
        public Manufacturer()
        {
            Weapons = new HashSet<Weapon>();
        }

        public int ManufacturerId { get; set; }
        public string Manufacturer1 { get; set; } = null!;
        public string Location { get; set; } = null!;

        public virtual ICollection<Weapon> Weapons { get; set; }
    }
}
