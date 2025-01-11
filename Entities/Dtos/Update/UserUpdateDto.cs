using Core.Entities;

namespace Entities.Dtos.Update;

public class UserUpdateDto : IDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string MobilePhone { get; set; }
    public DateTime BirthDate { get; set; }
    public int Gender { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
}