using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace BorderlandsStore.DATA.EF.Models//.Metadata
{
    //internal class Partials
    //{
    //}

    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category { }
    
    [ModelMetadataType(typeof(ElementMetadata))]
    public partial class Element { }
    
    [ModelMetadataType(typeof(ManufacturerMetadata))]
    public partial class Manufacturer { }
    
    [ModelMetadataType(typeof(WeaponMetadata))]
    public partial class Weapon { }
    
    [ModelMetadataType(typeof(WeaponStatusMetadata))]
    public partial class WeaponStatus { }

}
