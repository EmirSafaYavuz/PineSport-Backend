using Core.Entities;

namespace Entities.Dtos.Register;

public class ProgressCreateDto : IDto
{
    public int StudentId { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public string? TrainerNote { get; set; }
}