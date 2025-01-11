using Core.Entities;

namespace Entities.Dtos.BaseDto;

public class TrainerDto : IDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; }
    public string Specialization { get; set; }
}