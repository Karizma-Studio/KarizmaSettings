using KarizmaPlatform.Settings.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace KarizmaPlatform.Settings.Application.Extensions;

public static class WebApplicationExtension
{
    public static async Task LoadSettings(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var settingsService = scope.ServiceProvider.GetRequiredService<SettingsService>();

        await settingsService.ReloadCache();
    }
}