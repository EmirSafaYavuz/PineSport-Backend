using Core.Entities;

namespace Entities.Dtos.BaseDto;

public class ParentDto : IDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
    public List<string> ChildrenNames { get; set; }
}