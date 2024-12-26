using Core.Entities;

namespace Entities.Concrete;

public class Role : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
}