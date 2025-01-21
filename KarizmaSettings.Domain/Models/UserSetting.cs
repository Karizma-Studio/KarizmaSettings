using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KarizmaPlatform.Core.Database;

namespace KarizmaPlatform.Settings.Domain.Models;

[Table("user_settings")]
public class UserSetting : BaseEntity
{
    [Column("user_id"), Required] public required long UserId { get; init; }
    [Column("type"), Required, MaxLength(20)] public required string Type { get; init; }
    [Column("key"), Required, MaxLength(20)] public required string Key { get; init; }
    [Column("value"), Required] public required string Value { get; init; }

    public ISettingsUser? User { get; init; }
}