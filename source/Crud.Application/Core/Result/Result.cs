using System.Net;

namespace Crud.Application.Core.Result;

public class Result
{
    public bool Success { get; set; }

    public string Error { get; private set; }

    public bool IsFailure => !Success;

    public HttpStatusCode StatusCode { get; set; }

    protected Result(bool success, string error, HttpStatusCode statusCode)
    {
        if (success && !string.IsNullOrEmpty(error))
            throw new InvalidOperationException();

        if (!success && string.IsNullOrEmpty(error))
            throw new InvalidOperationException();

        Success = success;
        Error = error;
        StatusCode = statusCode;
    }

    public static Result Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result(false, message, statusCode);
    }

    public static Result<T> Fail<T>(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>(default, statusCode, false, message);
    }

    public static Result Ok()
    {
        return new Result(true, string.Empty, HttpStatusCode.OK);
    }

    public static Result<T> Ok<T>(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new Result<T>(value, statusCode, true, string.Empty);
    }
}

public class Result<T> : Result
{
    public T Value { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    protected internal Result(T value, HttpStatusCode statusCode, bool success, string error) : base(success, error, statusCode)
    {
        Value = value;
        StatusCode = statusCode;
    }
}