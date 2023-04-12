using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BorderlandsStore.DATA.EF//.Metadata
{
    //internal class Metadata
    //{
    //}

    public class CategoryMetadata
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Category")]
        [StringLength(15)]
        public string Category { get; set; } = null!;

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        [StringLength(75)]
        public string CategoryDescription { get; set; } = null!;
    }

    public class ElementMetadata
    {
        public int ElementId { get; set; }

        [Required]
        [Display(Name = "Element")]
        [StringLength(15)]
        public string ElementType { get; set; } = null!;
    }
    public class ManufacturerMetadata
    {
        public int ManufacturerId { get; set; }

        [Required]
        [StringLength(15)]
        public string Manufacturer { get; set; } = null!;

        [StringLength(15)]
        public string Location { get; set; } = null!;
    }
    public class WeaponMetadata
    {
        public int WeaponId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(150)]
        public string Description { get; set; } = null!;

        [Display(Name = "Category ID")]
        public int CategoryId { get; set; }

        [Display(Name = "Element ID")]
        public int? ElementId { get; set; }

        [Required]
        [Range(0, (double)decimal.MaxValue)]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [Display(Name = "Manufacturer ID")]
        public int ManufacturerId { get; set; }

        [Display(Name = "Weapon Image")]
        [StringLength(75)]
        public string? WeaponImage { get; set; }
    }
    public class WeaponStatusMetadata
    {
        public int WeaponId { get; set; }

        [Required]
        [Range(0, (double)short.MaxValue)]
        [Display(Name = "In Stock")]
        public int InStock { get; set; }

        [Display(Name = "Out of stock?")]
        public bool OutOfStock { get; set; }

        [Required]
        [Range(0, (double)short.MaxValue)]
        [Display(Name = "On Order")]
        public int OnOrder { get; set; }

        [Display(Name = "Discontinued?")]
        public bool Discontinued { get; set; }
    }

}
