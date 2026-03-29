using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Lib.BaseResponse
{
    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public ApiResponse(T data, bool isSuccess, string message, string errorCode = null)
        {
            Data = data;
            IsSuccess = isSuccess;
            Message = message;
            ErrorCode = errorCode;
        }

        public ApiResponse() { }

        public static ApiResponse<T> Success(T data, string message = null)
        {
            return new ApiResponse<T>(data, true, message);
        }

        public static ApiResponse<T> Failure(string errorCode, string message){
            return new ApiResponse<T>(default, false, message, errorCode);
        }
    }
}
