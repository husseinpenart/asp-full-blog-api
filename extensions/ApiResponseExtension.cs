using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myblog.extensions
{
    public class ApiResponseExtension<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int ItemLength { get; set; }
        public int StatusCode { get; set; }
        public T? Data { get; set; }
        public object? Extra { get; set; }
    }
}