using System;
using System.Collections.Generic;

namespace BorderlandsStore.DATA.EF.Models
{
    public partial class WeaponStatus
    {
        public int WeaponId { get; set; }
        public int InStock { get; set; }
        public bool OutOfStock { get; set; }
        public int OnOrder { get; set; }
        public bool Discontinued { get; set; }

        public virtual Weapon Weapon { get; set; } = null!;
    }
}
