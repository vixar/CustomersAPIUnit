namespace Application.Extentions;

public class ApiResponse
{
    public interface IResponse
    {
        string Message
        {
            get;
            set;
        }
        bool Succeeded
        {
            get;
            set;
        }
    }

    public class BaseResponse : IResponse
    {
        //public bool Failed => !Succeeded;

        public string Message
        {
            get;
            set;
        }

        public bool Succeeded
        {
            get;
            set;
        }

        public static IResponse Fail()
        {
            return new BaseResponse
            {
                Succeeded = false
            };
        }

        public static IResponse Fail(string message)
        {
            return new BaseResponse
            {
                Succeeded = false,
                Message = message
            };
        }

        public static Task<IResponse> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IResponse> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static IResponse Success()
        {
            return new BaseResponse
            {
                Succeeded = true
            };
        }

        public static IResponse Success(string message)
        {
            return new BaseResponse
            {
                Succeeded = true,
                Message = message
            };
        }

        public static Task<IResponse> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResponse> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }

    public class Response<T> : BaseResponse
    {
        public T Data
        {
            get;
            set;
        }

        public new static Response<T> Fail()
        {
            return new Response<T>
            {
                Succeeded = false
            };
        }

        public new static Response<T> Fail(string message)
        {
            return new Response<T>
            {
                Succeeded = false,
                Message = message
            };
        }

        public new static Task<Response<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<Response<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static Task<Response<T>> FailAsync(T data, string message)
        {
            return Task.FromResult(Fail(data, message));
        }

        public static Response<T> Fail(T data, string message)
        {
            return new Response<T>
            {
                Succeeded = false,
                Data = data,
                Message = message
            };
        }

        public new static Response<T> Success()
        {
            return new Response<T>
            {
                Succeeded = true
            };
        }

        public new static Response<T> Success(string message)
        {
            return new Response<T>
            {
                Succeeded = true,
                Message = message
            };
        }

        public static Response<T> Success(T data)
        {
            return new Response<T>
            {
                Succeeded = true,
                Data = data
            };
        }

        public static Response<T> Success(T data, string message)
        {
            return new Response<T>
            {
                Succeeded = true,
                Data = data,
                Message = message
            };
        }

        public new static Task<Response<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public new static Task<Response<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Response<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Response<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }

    public class PaginatedResponse<T> : BaseResponse
    {
        public List<T>? Data
        {
            get;
            set;
        }
        public int Page
        {
            get;
            set;
        }
        public int TotalPages
        {
            get;
            set;
        }
        public long TotalCount
        {
            get;
            set;
        }
        public bool HasPreviousPage => Page > 1;
        public bool HasNextPage => Page < TotalPages;

        public PaginatedResponse(List<T>? data)
        {
            Data = data;
        }

        internal PaginatedResponse(bool succeeded, List<T>? data = null, List<string>? messages = null, long count = 0L, int page = 1, int pageSize = 10)
        {
            Data = data;
            Page = page;
            base.Succeeded = succeeded;
            TotalPages = (int)Math.Ceiling((double)count / pageSize);
            TotalCount = count;
        }

        public static PaginatedResponse<T> Failure(List<string>? messages)
        {
            return new PaginatedResponse<T>(succeeded: false, null, messages, 0L);
        }

        public static PaginatedResponse<T> Success(List<T>? data, long count, int page, int pageSize)
        {
            return new PaginatedResponse<T>(succeeded: true, data, null, count, page, pageSize);
        }
    }
}