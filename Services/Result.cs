using Microsoft.AspNetCore.Mvc;

namespace UzTube.Services;

public class Result : IActionResult
{
    public string? Message { get; set; }

    public int StatusCode { get; set; }

    public async Task ExecuteResultAsync(ActionContext context)
    {
        JsonResult result = new JsonResult(this)
        {
            StatusCode = this.StatusCode
        };

        await result.ExecuteResultAsync(context);
    }
}

public class Result<T> : Result
{
    public T? Data { get; set; }
}
