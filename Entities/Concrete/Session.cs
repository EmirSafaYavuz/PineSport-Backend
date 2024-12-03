using Core.Entities;

namespace Entities.Concrete;

public class Session : BaseEntity<int>
{
    public string Name { get; set; }
    public string Day { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int SchoolId { get; set; }
    public School School { get; set; }
    public ICollection<StudentSession> StudentSessions { get; set; }
    public ICollection<Trainer> Trainers { get; set; }
}