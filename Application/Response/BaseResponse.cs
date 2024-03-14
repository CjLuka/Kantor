using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Response
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> ValidationErrors { get; set; }

        public BaseResponse() { }
        public BaseResponse(bool success)
        {
            Success = success;
        }
        public BaseResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string>? ValidationErrors { get; set; }

        public BaseResponse(T data, bool success, string? message = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }
        public BaseResponse(bool success, string? message = null)
        {
            Success = success;
            Message = message;
        }
        public BaseResponse(bool success, List<string> validationErrors)
        {
            Success = success;
            ValidationErrors = validationErrors;
        }
        public BaseResponse()
        {
            
        }
    }
}
