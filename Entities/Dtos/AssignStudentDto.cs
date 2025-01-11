using Core.Entities;

namespace Entities.Dtos;

public class AssignStudentDto : IDto
{
    public int SessionId { get; set; }
    public int StudentId { get; set; }
}