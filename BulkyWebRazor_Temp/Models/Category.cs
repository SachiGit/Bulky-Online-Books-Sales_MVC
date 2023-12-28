using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWebRazor_Temp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        //[Required]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order Must Be Between 1-100")]
        public int DisplayOrder { get; set; }   //'DisplayName' -> 'DisplayOrder' Corrected In Razor Application
    }
}
