using Core.Entities;

namespace Entities.Concrete;

public class Branch : BaseEntity<int>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    
    public int SchoolId { get; set; }
    public School School { get; set; }
    
    public ICollection<Session> Sessions { get; set; }
    public ICollection<Student> Students { get; set; }
}