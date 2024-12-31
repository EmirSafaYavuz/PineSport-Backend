using Cassandra;
using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete;

public class School : User
{
    public string SchoolName { get; set; } 
    public string SchoolAddress { get; set; } 
    public string SchoolPhone { get; set; } 
    
    public ICollection<Branch> Branches { get; set; }
    
}