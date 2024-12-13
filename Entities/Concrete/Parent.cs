using Core.Entities.Concrete;

namespace Entities.Concrete;

public class Parent : User
{
    public string PhoneNumber { get; set; }
    public ICollection<Student> Children { get; set; }
}