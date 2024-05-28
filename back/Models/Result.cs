using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace back.Models
{
    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public T Data { get; private set; }
        public static Result<T> Success(T data) => new Result<T> { IsSuccess = true, Data = data };
        public static Result<T> Fail(string message) => new Result<T> { IsSuccess = false, ErrorMessage = message };
    }
}