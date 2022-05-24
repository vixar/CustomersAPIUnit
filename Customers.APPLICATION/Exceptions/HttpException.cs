using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Application.Exceptions;


    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public  class HttpException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public HttpException(HttpStatusCode statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpException(HttpStatusCode statusCode, Exception inner)
            : this(statusCode, inner.ToString()) { }

        public HttpException(HttpStatusCode statusCode, JObject errorObject)
            : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
