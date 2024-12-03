using Core.Entities;

namespace Entities.Concrete;

public class Student : BaseEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string ParentName { get; set; }
    public string ParentPhone { get; set; }
    public int SchoolId { get; set; }
    public School School { get; set; }
    public ICollection<StudentSession> StudentSessions { get; set; }
    public ICollection<Payment> Payments { get; set; }
    public ICollection<StudentProgress> Progress { get; set; }
}