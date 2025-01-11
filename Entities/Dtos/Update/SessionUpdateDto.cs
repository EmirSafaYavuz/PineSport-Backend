using Core.Entities;

namespace Entities.Dtos.Update;

public class SessionUpdateDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan Time { get; set; }
    public int BranchId { get; set; }
}