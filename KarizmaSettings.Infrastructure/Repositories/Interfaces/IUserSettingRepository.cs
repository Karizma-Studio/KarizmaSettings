using KarizmaPlatform.Core.Logic;
using KarizmaPlatform.Settings.Domain.Models;

namespace KarizmaPlatform.Settings.Infrastructure.Repositories.Interfaces;

public interface IUserSettingRepository : IRepository<UserSetting>
{
    Task<List<UserSetting>> GetUserSettings(long userId, bool asNoTracking = false);
    Task<UserSetting?> GetUserSetting(long userId, string type, string key, bool asNoTracking = false);
}