using Core.Entities;

namespace Entities.Concrete;

public class Trainer : BaseEntity<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; }
}