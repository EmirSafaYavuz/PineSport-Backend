using Core.Entities;
using Core.Entities.Concrete;

namespace Entities.Concrete;

public class Trainer : User
{
    public string Specialization { get; set; }
    public ICollection<Session> Sessions { get; set; }
}