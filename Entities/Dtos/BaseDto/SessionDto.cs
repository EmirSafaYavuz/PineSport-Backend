using Core.Entities;

namespace Entities.Dtos.BaseDto;

public class SessionDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string BranchName { get; set; } // Branch name for display purposes
    public List<string> TrainerNames { get; set; } // List of trainer names for the session
    public List<string> StudentNames { get; set; }
}