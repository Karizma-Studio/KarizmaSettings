using System.Security.AccessControl;
using KarizmaPlatform.Settings.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace KarizmaPlatform.Settings.Infrastructure;

public class SettingsDatabaseUtilities
{
    public static void ConfigureDatabase<T>(ModelBuilder modelBuilder) where T : class, ISettingsUser
    {
        modelBuilder.Entity<UserSetting>()
            .HasOne<T>()
            .WithMany()
            .HasForeignKey(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserSetting>().Ignore(ur => ur.User);


        modelBuilder.Entity<Setting>()
            .HasIndex(x => new { x.Type, x.Key })
            .HasFilter("deleted_at IS NULL");

        modelBuilder.Entity<UserSetting>()
            .HasIndex(x => new { x.UserId, x.Type, x.Key })
            .HasFilter("deleted_at IS NULL");

        modelBuilder.Entity<UserSetting>()
            .HasIndex(x => new { x.UserId })
            .HasFilter("deleted_at IS NULL");

        modelBuilder.Entity<Setting>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<Setting>()
            .Property(b => b.UpdatedDate)
            .HasDefaultValueSql("now()");
    }
}