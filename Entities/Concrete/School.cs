using Cassandra;
using Core.Entities;

namespace Entities.Concrete;

public class School : BaseEntity<int>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    
    public ICollection<Branch> Branches { get; set; }
}