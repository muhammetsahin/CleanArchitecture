using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Database;

public static class Extensions
{
    public static void AddContextInMemoryDatabase(this IServiceCollection services)
    {
        services.AddDbContextPool<Context>(options => options.UseInMemoryDatabase(nameof(Context)));

        services.BuildServiceProvider().GetRequiredService<Context>().Database.EnsureDeleted();
    }
}
