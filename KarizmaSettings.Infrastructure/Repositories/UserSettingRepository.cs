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
        return settingsDatabase.GetUserSettings().SingleOrDefaultAsync((x => x.DeletedDate == new DateTimeOffset?() && x.Id == identifier));
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
}