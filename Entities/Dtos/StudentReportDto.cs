using Core.Entities;

namespace Entities.Dtos;

public class StudentReportDto : IDto
{
    public int TotalStudents { get; set; }
}