using System;
using System.Collections.Generic;

namespace BorderlandsStore.DATA.EF.Models
{
    public partial class Weapon
    {
        public int WeaponId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int CategoryId { get; set; }
        public int? ElementId { get; set; }
        public decimal Price { get; set; }
        public int ManufacturerId { get; set; }
        public string? WeaponImage { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual Element? Element { get; set; }
        public virtual Manufacturer Manufacturer { get; set; } = null!;
        public virtual WeaponStatus? WeaponStatus { get; set; }
    }
}
