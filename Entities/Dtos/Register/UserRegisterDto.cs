using Core.Entities;

namespace Entities.Dtos.Register;

public class UserRegisterDto : IDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public DateTime BirthDate { get; set; }
    public int Gender { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
    public string Password { get; set; }
}