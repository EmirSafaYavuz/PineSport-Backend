using Core.Entities;

namespace Entities.Concrete;

public class StudentSession : BaseEntity<int>
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; }
}