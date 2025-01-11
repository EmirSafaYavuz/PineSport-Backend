using Core.Entities;

namespace Entities.Dtos;

public class AssignCoachDto : IDto
{
    public int SessionId { get; set; }
    public int TrainerId { get; set; }
}