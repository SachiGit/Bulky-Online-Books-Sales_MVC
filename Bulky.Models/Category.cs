using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
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
        [Range(1,100,ErrorMessage = "Display Order Must Be Between 1-100")]
        public int DisplayName { get; set; }   //Should be 'DisplayOrder'


    }
}
