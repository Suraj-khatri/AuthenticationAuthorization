using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAuthorization.Domain
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public ApiResponse() { }
        public ApiResponse(bool success,int statusCode, string? message, T? data)
        {
            Success = success;
            Message = message;
            Data = data;
            StatusCode = statusCode;
        }
        // Static helper for successful response
        public static ApiResponse<T> SuccessResponse(T data, string? message = null, int statusCode = 200)
        {
            return new ApiResponse<T>(true, statusCode, message, data);
        }

        // Static helper for failed response
        public static ApiResponse<T> FailureResponse(string message, int statusCode = 400)
        {
            return new ApiResponse<T>(false, statusCode, message, default);
        }
    }
}
