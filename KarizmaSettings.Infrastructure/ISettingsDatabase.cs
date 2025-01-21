using KarizmaPlatform.Core.Database;
using KarizmaPlatform.Settings.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KarizmaPlatform.Settings.Infrastructure;

public interface ISettingsDatabase : IBaseContext
{
    DbSet<Setting> GetSettings();
    DbSet<UserSetting> GetUserSettings();
}