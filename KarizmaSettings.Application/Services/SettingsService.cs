using KarizmaPlatform.Settings.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KarizmaPlatform.Settings.Application.Services;

public class SettingsService(
    IServiceScopeFactory scopeFactory,
    SettingsCache settingsCache)
{
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