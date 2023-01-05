using CleanArchitecture.CrossCutting;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace CleanArchitecture.Api;

public static class Extensions
{
    public static void AddJsonOptions(this IMvcBuilder builder)
    {
        builder.AddJsonOptions(options => options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);
    }

    public static Task<IActionResult> DeleteResultAsync(this Task<Result> taskResult)
    {
        return taskResult.DefaultResultAsync();
    }

    public static async Task<IActionResult> GetResultAsync<T>(this Task<Result<T>> taskResult)
    {
        var result = await taskResult;

        return result.IsError ? new BadRequestObjectResult(result.Message) : result.HasValue ? new OkObjectResult(result.Value) : new NoContentResult();
    }

    public static Task<IActionResult> PatchResultAsync(this Task<Result> result)
    {
        return result.DefaultResultAsync();
    }

    public static async Task<IActionResult> PostResultAsync<T>(this Task<Result<T>> taskResult)
    {
        var result = await taskResult;

        return result.IsError ? new BadRequestObjectResult(result.Message) : new OkObjectResult(result.Value);
    }

    public static Task<IActionResult> PutResultAsync(this Task<Result> taskResult)
    {
        return taskResult.DefaultResultAsync();
    }

    public static void UseException(this IApplicationBuilder application)
    {
        var environment = application.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        if (environment.IsDevelopment())
        {
            application.UseDeveloperExceptionPage();
        }
        else
        {
            application.UseExceptionHandler(builder => builder.Run(async context => await context.Response.WriteAsync(string.Empty)));
        }
    }

    public static void UseLocalization(this IApplicationBuilder application)
    {
        var cultures = new[] { "en", "pt" };

        application.UseRequestLocalization(options =>
        {
            options.AddSupportedCultures(cultures);
            options.AddSupportedUICultures(cultures);
            options.SetDefaultCulture(cultures.First());
        });
    }

    private static async Task<IActionResult> DefaultResultAsync(this Task<Result> taskResult)
    {
        var result = await taskResult;

        return result.IsError ? new BadRequestObjectResult(result.Message) : new OkResult();
    }
}
