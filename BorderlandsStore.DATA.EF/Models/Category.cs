using System;
using System.Collections.Generic;

namespace BorderlandsStore.DATA.EF.Models
{
    public partial class Category
    {
        public Category()
        {
            Weapons = new HashSet<Weapon>();
        }

        public int CategoryId { get; set; }
        public string Category1 { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;

        public virtual ICollection<Weapon> Weapons { get; set; }
    }
}
