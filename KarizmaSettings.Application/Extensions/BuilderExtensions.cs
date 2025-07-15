using KarizmaPlatform.Settings.Application.Services;
using KarizmaPlatform.Settings.Infrastructure;
using KarizmaPlatform.Settings.Infrastructure.Repositories;
using KarizmaPlatform.Settings.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace KarizmaPlatform.Settings.Application.Extensions;

public static class BuilderExtensions
{
    public static IServiceCollection AddKarizmaSettings<TDatabase>(this IServiceCollection services, SettingOptions options) where TDatabase : ISettingsDatabase
    {
        services
            .AddScoped<ISettingRepository, SettingRepository>()
            .AddScoped<ISettingsDatabase>(provider => provider.GetRequiredService<TDatabase>())
            .AddSingleton<SettingsCache>()
            .AddSingleton<SettingsService>()
            .AddSingleton(options);
        
        if (options.HasUserSetting)
            services.AddScoped<IUserSettingRepository, UserSettingRepository>();
        
        return services;
    }
}