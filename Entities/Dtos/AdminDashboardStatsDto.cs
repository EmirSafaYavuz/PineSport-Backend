using Core.Entities;

namespace Entities.Dtos;

public class AdminDashboardStatsDto : IDto
{
    public int TotalStudents { get; set; }
    public decimal MonthlyIncome { get; set; }
    public decimal MonthlyIncomeGrowth { get; set; }
    public int DelayedPaymentsCount { get; set; }
    public decimal DelayedPaymentsAmount { get; set; }
    public int ActiveSportsCount { get; set; }
    public int ActiveSessionsCount { get; set; }
    public decimal StudentGrowth { get; set; }
}
