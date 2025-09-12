using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myblog.models.DtoModels
{
    public class PaginatedResponseDto<T>
    {
        public List<T> Data { get; set; }
        public int PageNumber { get; set; } // Current page
        public int PageSize { get; set; } // Items per page
        public int TotalItems { get; set; } // Total items in the dataset
        public int TotalPages { get; set; } // Total number of pages
    }
}