using KarizmaPlatform.Core.Logic;
using KarizmaPlatform.Settings.Domain.Models;

namespace KarizmaPlatform.Settings.Infrastructure.Repositories.Interfaces;

public interface ISettingRepository : IRepository<Setting>
{
    Task<List<Setting>> GetAllNotDeleted(bool asNoTracking = false);
}