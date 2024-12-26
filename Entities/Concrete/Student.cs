using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete;

public class Student : User
{
    public int BranchId { get; set; }
    public Branch Branch { get; set; }
    
    public int ParentId { get; set; }
    public Parent Parent { get; set; }
    
    public ICollection<StudentSession> StudentSessions { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<StudentProgress> ProgressRecords { get; set; }
}