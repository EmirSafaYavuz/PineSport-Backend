using Core.Entities;

namespace Entities.Dtos.Update;

public class ProgressUpdateDto : IDto
{
    public int StudentId { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string? TrainerNote { get; set; }
}