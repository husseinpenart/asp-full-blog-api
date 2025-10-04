using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace myblog.models.DtoModels
{
    public class blogCrudDto
    {
        public string slug { get; set; }

        [Required(ErrorMessage = "title required")]
        [StringLength(100, ErrorMessage = "for better seo more than 100 char is no good")]
        public string title { get; set; }

        public IFormFile? cover { get; set; }

        [Required(ErrorMessage = "Description Required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Category Required")]
        public string category { get; set; }

        public string writer { get; set; }
    }

    public class blogResponseDto
    {
        public Guid Id { get; set; }
        public string slug { get; set; }
        public string title { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string category { get; set; }
        public string writer { get; set; }
        public Guid UserId { get; set; }
        public DateTime createdAt { get; set; }
    }
}
