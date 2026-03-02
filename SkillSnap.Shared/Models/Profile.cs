using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SkillSnap.Shared.Models;

public class Profile
{
    [Key]
    public int Id { get; set; }
    public string ProfileName { get; set; } = string.Empty;
    public int Level { get; set; }
    public string LevelBadge { get; set; } = string.Empty;
    public int PrestigeLevel { get; set; }
    public string PrestigeBadge { get; set; } = string.Empty;

    [ForeignKey("ApplicationUser")]
    public string ApplicationUserId { get; set; } = string.Empty;
}