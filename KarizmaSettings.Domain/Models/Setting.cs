using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KarizmaPlatform.Core.Database;

namespace KarizmaPlatform.Settings.Domain.Models;

[Table("settings")]
public class Setting : BaseEntity
{
    [Column("type"), Required, MaxLength(20)] public required string Type { get; init; }
    [Column("key"), Required, MaxLength(20)] public required string Key { get; init; }
    [Column("value"), Required] public required string Value { get; init; }
    [Column("is_public"), Required] public required bool IsPublic { get; init; }
}