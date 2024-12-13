using System.Collections;
using Core.Entities;

namespace Entities.Concrete;

public class Session : BaseEntity<int>
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    
    public int BranchId { get; set; }
    public Branch Branch { get; set; }
    
    public ICollection<StudentSession> StudentSessions { get; set; }
    public ICollection<Trainer> Trainers { get; set; }
}