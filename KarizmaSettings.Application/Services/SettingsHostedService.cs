using KarizmaPlatform.Settings.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KarizmaPlatform.Settings.Application.Services;

public class SettingsHostedService(
    IServiceScopeFactory scopeFactory,
    SettingsCache settingsCache) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await FillSettingsCache();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Settings Cache Stopping ...");
        return Task.CompletedTask;
    }

    public async Task ReloadCache()
    {
        await FillSettingsCache();
    }

    private async Task FillSettingsCache()
    {
        using var scope = scopeFactory.CreateScope();
        var database = scope.ServiceProvider.GetRequiredService<ISettingsDatabase>();
        var settings = await database.GetSettings().Where(s => s.DeletedDate == null).ToListAsync();
        settingsCache.Populate(settings);
    }
}