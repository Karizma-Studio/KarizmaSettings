using KarizmaPlatform.Settings.Domain.Models;
using KarizmaPlatform.Settings.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarizmaPlatform.Settings.Infrastructure.Repositories;

public class UserSettingRepository(ISettingsDatabase settingsDatabase) : IUserSettingRepository
{
    public async Task<UserSetting> Add(UserSetting userSetting)
    {
        settingsDatabase.GetUserSettings().Add(userSetting);
        await settingsDatabase.SaveChangesAsync();
        return userSetting;
    }

    public Task Update(UserSetting userSetting)
    {
        settingsDatabase.GetUserSettings().Update(userSetting);
        return settingsDatabase.SaveChangesAsync();
    }

    public async Task DeleteById(long identifier)
    {
        var byId = await FindById(identifier);
        if (byId is null)
            return;
        settingsDatabase.GetUserSettings().Remove(byId);
        await settingsDatabase.SaveChangesAsync();
    }

    public async Task SoftDeleteById(long identifier)
    {
        var byId = await FindById(identifier);
        if (byId is null)
            return;
        byId.DeletedDate = DateTimeOffset.UtcNow;
        settingsDatabase.GetUserSettings().Update(byId);
        await settingsDatabase.SaveChangesAsync();
    }

    public Task<UserSetting?> FindById(long identifier)
    {
        return settingsDatabase.GetUserSettings().SingleOrDefaultAsync(x => x.Id == identifier);
    }

    public Task<UserSetting?> FindNotDeletedById(long identifier)
    {
        return settingsDatabase.GetUserSettings()
            .SingleOrDefaultAsync((x => x.DeletedDate == new DateTimeOffset?() && x.Id == identifier));
    }

    public Task<List<UserSetting>> GetAll()
    {
        return settingsDatabase.GetUserSettings().ToListAsync();
    }

    public Task<List<UserSetting>> GetUserSettings(long userId, bool asNoTracking = false)
    {
        return (asNoTracking ? settingsDatabase.GetUserSettings().AsNoTracking() : settingsDatabase.GetUserSettings())
            .Where(us => us.UserId == userId && us.DeletedDate == null)
            .ToListAsync();
    }

    public Task<UserSetting?> GetUserSetting(long userId, string type, string key, bool asNoTracking = false)
    {
        return (asNoTracking ? settingsDatabase.GetUserSettings().AsNoTracking() : settingsDatabase.GetUserSettings())
            .SingleOrDefaultAsync(us => us.UserId == userId &&
                                        us.Type == type &&
                                        us.Key == key &&
                                        us.DeletedDate == null);
    }

    public async Task<T?> GetUserSettingValue<T>(long userId, string type, string key, T? defaultValue = default)
    {
        var settingValue = await settingsDatabase.GetUserSettings().AsNoTracking()
            .Where(us => us.UserId == userId &&
                         us.Type == type &&
                         us.Key == key &&
                         us.DeletedDate == null)
            .Select(us => us.Value)
            .SingleOrDefaultAsync();

        if (settingValue == null)
        {
            return defaultValue;
        }

        return (T?)Convert.ChangeType(settingValue, typeof(T));
    }

    
    public async Task SetUserSetting<T>(long userId, string type, string key, T value)
    {
        
        var userSetting = await GetUserSetting(userId, type, key);
        if (userSetting is null)
        {
            await Add(new UserSetting
            {
                UserId = userId,
                Type = type,
                Key = key,
                Value = value.ToString()
            });
        }
        else
        {
            userSetting.Value = value.ToString();
            await Update(userSetting);
        }
    }
}