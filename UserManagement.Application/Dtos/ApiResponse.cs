using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Dtos
{
    public class ApiResponse<T>
    {
        public bool Status {  get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }
        public object Errors { get; set; }
    }
}
