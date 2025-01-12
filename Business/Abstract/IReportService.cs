using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface IReportService
{
    IDataResult<StudentReportDto> GetStudentReport();
    IDataResult<IncomeReportDto> GetIncomeReport(DateTime startDate, DateTime endDate);
    IDataResult<AttendanceReportDto> GetAttendanceReport();
    IDataResult<RegistrationReportDto> GetNewRegistrationsReport(DateTime startDate, DateTime endDate);
    IDataResult<AdminDashboardStatsDto> GetAdminDashboardStats();
}