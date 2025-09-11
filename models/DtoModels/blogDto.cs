

using System.ComponentModel.DataAnnotations;

namespace myblog.models.DtoModels
{
    public class blogCrudDto
    {
        [Required(ErrorMessage = "title required")]
        [StringLength(100, ErrorMessage = "for better seo more than 100 char is no good")]
        public string title { get; set; }
        [Required(ErrorMessage = "Description Requiered")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Category Required")]
        public string category { get; set; }
        public string writer { get; set; }


    }
    public class blogResponseDto
    {
        public Guid Id { get; set; }

        public string title { get; set; }
        public string Description { get; set; }
        public string category { get; set; }
        public string writer { get; set; }
        public DateTime createdAt { get; set; }
    }
}