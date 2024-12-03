using Cassandra;
using Core.Entities;

namespace Entities.Concrete;

public class School : BaseEntity<int>
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int? ParentSchoolId { get; set; }
    public School ParentSchool { get; set; }
    public ICollection<School> Branches { get; set; }
    public ICollection<Student> Students { get; set; }
    public ICollection<Session> Sessions { get; set; }
}