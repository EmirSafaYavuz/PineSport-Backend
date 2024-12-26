using Core.Entities.Concrete;

namespace Entities.Concrete;

public class Parent : User
{
    public ICollection<Student> Children { get; set; }
}