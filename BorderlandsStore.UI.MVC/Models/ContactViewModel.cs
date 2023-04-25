using System.ComponentModel.DataAnnotations;

namespace BorderlandsStore.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "* Name is Required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "* Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "* Message is Required")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; } = null!;
    }
}
