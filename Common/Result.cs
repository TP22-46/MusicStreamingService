namespace MusicStreamingService.Common;

public class Result
{
     public bool Success { get; init; }
    public int StatusCode { get; init; }
    public string Message { get; init; } = "";
    public object? Data { get; init; }

    public static Result Ok(object data) =>
        new() { Success = true, StatusCode = 200, Data = data };

    public static Result Created(object data) =>
        new() { Success = true, StatusCode = 201, Data = data };

    public static Result NoContent() =>
        new() { Success = true, StatusCode = 204 };

    public static Result Fail(int code, string message) =>
        new() { StatusCode = code, Message = message };

    public static Result BadRequest(string message) => Fail(400, message);
    public static Result Unauthorized(string message) => Fail(401, message);
    public static Result Forbidden(string message) => Fail(403, message);
    public static Result NotFound(string message) => Fail(404, message);
    public static Result Conflict(string message) => Fail(409, message);
}
