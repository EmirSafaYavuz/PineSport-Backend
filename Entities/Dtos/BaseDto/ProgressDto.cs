using Core.Entities;

namespace Entities.Dtos.BaseDto;

public class ProgressDto : IDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string StudentName { get; set; }
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public DateTime RecordDate { get; set; }
    public string? TrainerNote { get; set; } 
}