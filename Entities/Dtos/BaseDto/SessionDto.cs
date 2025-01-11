using Core.Entities;

namespace Entities.Dtos;

public class SessionDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public string BranchName { get; set; }
    public List<string> Trainers { get; set; }
}