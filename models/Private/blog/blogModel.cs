
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using myblog.models.Private.users;

namespace myblog.models.Private.blog
{
    public class blogModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required(ErrorMessage = "title required")]
        [StringLength(100, ErrorMessage = "for better seo more than 100 char is no good")]
        public string title { get; set; }
        [Required(ErrorMessage = "Cover is Required")]
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "Description Requiered")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Category Required")]
        public string category { get; set; }
        public string writer { get; set; }
        public Guid UserId { get; set; } // Foreign key to userModel

        [ForeignKey("UserId")]
        public userModel User { get; set; }
        public DateTime createdAt { get; set; } = DateTime.UtcNow;


    }
}