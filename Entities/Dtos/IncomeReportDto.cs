using Core.Entities;

namespace Entities.Dtos;

public class IncomeReportDto : IDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalIncome { get; set; }
}