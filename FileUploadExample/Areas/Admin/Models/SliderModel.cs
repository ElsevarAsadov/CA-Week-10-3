using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Areas.Admin.Models
{
    public class SliderModel
    {
        
        public int Id { get; set; }

        [Required]
        public string UpperText { get; set; }

        [Required]
        public string BottomText { get; set; }

        [Required]
        public string ButtonText { get; set; }
        
       
        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        

    }
}
