using Core.Entities;

namespace Entities.Dtos;

public class RegistrationReportDto : IDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int NewRegistrations { get; set; }
    public int Cancellations { get; set; }
}