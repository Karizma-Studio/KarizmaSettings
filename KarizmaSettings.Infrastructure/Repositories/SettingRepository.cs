using KarizmaPlatform.Settings.Domain.Models;
using KarizmaPlatform.Settings.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KarizmaPlatform.Settings.Infrastructure.Repositories;

public class SettingRepository(ISettingsDatabase settingsDatabase) : ISettingRepository
{
    public async Task<Setting> Add(Setting userSetting)
    {
        settingsDatabase.GetSettings().Add(userSetting);
        await settingsDatabase.SaveChangesAsync();
        return userSetting;
    }

    public Task Update(Setting userSetting)
    {
        settingsDatabase.GetSettings().Update(userSetting);
        return settingsDatabase.SaveChangesAsync();
    }

    public async Task DeleteById(long identifier)
    {
        var byId = await FindById(identifier);
        if (byId is null)
            return;
        settingsDatabase.GetSettings().Remove(byId);
        await settingsDatabase.SaveChangesAsync();
    }
    
    public async Task SoftDelete(long identifier)
    {
        var byId = await FindById(identifier);
        if (byId is null)
            return;
        byId.DeletedDate = DateTimeOffset.UtcNow;
        settingsDatabase.GetSettings().Update(byId);
        await settingsDatabase.SaveChangesAsync();
    }

    public Task<Setting?> FindById(long identifier)
    {
        return settingsDatabase.GetSettings().SingleOrDefaultAsync(x => x.Id == identifier);
    }

    public Task<Setting?> FindNotDeletedById(long identifier)
    {
        return settingsDatabase.GetSettings().SingleOrDefaultAsync((x => x.DeletedDate == new DateTimeOffset?() && x.Id == identifier));
    }

    public Task<List<Setting>> GetAll()
    {
        return settingsDatabase.GetSettings().ToListAsync();
    }

    public Task<List<Setting>> GetAllNotDeleted()
    {
        throw new NotImplementedException();
    }

    public Task<List<Setting>> GetAllNotDeleted(bool asNoTracking = false)
    {
        return (asNoTracking ? settingsDatabase.GetSettings().AsNoTracking() : settingsDatabase.GetSettings())
            .Where(x => x.DeletedDate == new DateTimeOffset?()).ToListAsync();
    }
}