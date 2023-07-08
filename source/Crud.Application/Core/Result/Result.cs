using System.Net;

namespace Crud.Application.Core.Result;

/// <summary>
/// Represents the result of an operation with success or failure status and optional error message.
/// </summary>
public class Result
{
    /// <summary>
    /// Gets the success status of the result.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets the error message of the result, if any.
    /// </summary>
    public string Error { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the result represents a failure.
    /// </summary>
    public bool IsFailure => !Success;

    /// <summary>
    /// Gets the HTTP status code associated with the result.
    /// </summary>
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

    /// <summary>
    /// Creates a failure result with the specified error message and optional HTTP status code.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code associated with the result. Defaults to HttpStatusCode.BadRequest.</param>
    /// <returns>The failure result.</returns>
    public static Result Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result(false, message, statusCode);
    }

    /// <summary>
    /// Creates a typed failure result with the specified error message and optional HTTP status code.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code associated with the result. Defaults to HttpStatusCode.BadRequest.</param>
    /// <returns>The typed failure result.</returns>
    public static Result<T> Fail<T>(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
    {
        return new Result<T>(default, statusCode, false, message);
    }

    /// <summary>
    /// Creates a success result.
    /// </summary>
    /// <returns>The success result.</returns>
    public static Result Ok()
    {
        return new Result(true, string.Empty, HttpStatusCode.OK);
    }

    /// <summary>
    /// Creates a typed success result with the specified value and optional HTTP status code.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="value">The result value.</param>
    /// <param name="statusCode">The HTTP status code associated with the result. Defaults to HttpStatusCode.OK.</param>
    /// <returns>The typed success result.</returns>
    public static Result<T> Ok<T>(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new Result<T>(value, statusCode, true, string.Empty);
    }
}

/// <summary>
/// Represents the result of an operation with success or failure status, optional value, and optional error message.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// Gets or sets the result value.
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Gets or sets the HTTP status code associated with the result.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    protected internal Result(T value, HttpStatusCode statusCode, bool success, string error) : base(success, error, statusCode)
    {
        Value = value;
        StatusCode = statusCode;
    }
}
