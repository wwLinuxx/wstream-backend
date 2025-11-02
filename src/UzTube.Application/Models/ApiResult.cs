namespace UzTube.Application.Models;

public record ApiResult<T>
{
    private ApiResult()
    {
    }

    private ApiResult(bool succeeded, T? result, IEnumerable<string>? errors)
    {
        Succeeded = succeeded;
        Result = result;
        Errors = errors;
    }

    public bool Succeeded { get; init; }
    public T? Result { get; init; }
    public IEnumerable<string>? Errors { get; init; }

    public static ApiResult<T> Success(T result)
    {
        return new ApiResult<T>(true, result, []);
    }

    public static ApiResult<T> Failure(IEnumerable<string> errors)
    {
        return new ApiResult<T>(false, default!, errors);
    }
}