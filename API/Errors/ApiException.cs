using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Errors
{
    public class ApiException
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Detailes { get; set; }
        public ApiException(int statusCode, string message = null, string detailes = null)
        {
            StatusCode = statusCode;
            Message = message;
            Detailes = detailes;
        } 
    }
}