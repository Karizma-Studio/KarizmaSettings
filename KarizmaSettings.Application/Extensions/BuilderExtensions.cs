using KarizmaPlatform.Settings.Application.Services;
using KarizmaPlatform.Settings.Infrastructure;
using KarizmaPlatform.Settings.Infrastructure.Repositories;
using KarizmaPlatform.Settings.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace KarizmaPlatform.Settings.Application.Extensions;

public static class BuilderExtensions
{
    public static IServiceCollection AddKarizmaSettings<TDatabase>(this IServiceCollection services) where TDatabase : ISettingsDatabase
    {
        services
            .AddScoped<ISettingRepository, SettingRepository>()
            .AddScoped<IUserSettingRepository, UserSettingRepository>()
            .AddScoped<ISettingsDatabase>(provider => provider.GetRequiredService<TDatabase>())
            .AddSingleton<SettingsCache>()
            .AddHostedService<SettingsHostedService>();

        return services;
    }
}