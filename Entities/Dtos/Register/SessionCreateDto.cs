using Core.Entities;

namespace Entities.Dtos.Register;

public class SessionCreateDto : IDto
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public int BranchId { get; set; }
}