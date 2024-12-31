using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete;

public class Branch : User
{
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string BranchPhone { get; set; }
    
    public int SchoolId { get; set; }
    public School School { get; set; }
    
    public ICollection<Session> Sessions { get; set; }
    public ICollection<Student> Students { get; set; }
}