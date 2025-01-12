using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Dtos;

namespace Business.Concrete;

public class ReportService : IReportService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IPaymentRepository _paymentRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly IMapper _mapper;

    public ReportService(
        IStudentRepository studentRepository,
        IPaymentRepository paymentRepository,
        ISessionRepository sessionRepository,
        IMapper mapper)
    {
        _studentRepository = studentRepository;
        _paymentRepository = paymentRepository;
        _sessionRepository = sessionRepository;
        _mapper = mapper;
    }

    [RoleRequirement("admin")]
    public IDataResult<StudentReportDto> GetStudentReport()
    {
        var totalStudents = _studentRepository.GetCount();
        return new SuccessDataResult<StudentReportDto>(new StudentReportDto
        {
            TotalStudents = totalStudents
        }, "Student report generated successfully.");
    }

    [RoleRequirement("admin")]
    public IDataResult<IncomeReportDto> GetIncomeReport(DateTime startDate, DateTime endDate)
    {
        var payments = _paymentRepository.GetList(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate && p.IsPaid);
        var totalIncome = payments.Sum(p => p.Amount);
        return new SuccessDataResult<IncomeReportDto>(new IncomeReportDto
        {
            StartDate = startDate,
            EndDate = endDate,
            TotalIncome = totalIncome
        }, "Income report generated successfully.");
    }

    public IDataResult<AttendanceReportDto> GetAttendanceReport()
    {
        var sessions = _sessionRepository.GetList(null, includeProperties: "StudentSessions");
        var attendance = sessions.Select(s => new SessionAttendanceDto
        {
            SessionId = s.Id,
            SessionName = s.Name,
            AttendanceCount = s.StudentSessions.Count
        }).ToList();

        return new SuccessDataResult<AttendanceReportDto>(new AttendanceReportDto
        {
            AttendanceDetails = attendance
        }, "Attendance report generated successfully.");
    }

    public IDataResult<RegistrationReportDto> GetNewRegistrationsReport(DateTime startDate, DateTime endDate)
    {
        var newRegistrations = _studentRepository.GetList(s => s.RecordDate >= startDate && s.RecordDate <= endDate).Count();
        var cancellations = _studentRepository.GetList(s => s.Status == false && s.UpdateContactDate >= startDate && s.UpdateContactDate <= endDate).Count();

        return new SuccessDataResult<RegistrationReportDto>(new RegistrationReportDto
        {
            StartDate = startDate,
            EndDate = endDate,
            NewRegistrations = newRegistrations,
            Cancellations = cancellations
        }, "Registration report generated successfully.");
    }

    public IDataResult<AdminDashboardStatsDto> GetAdminDashboardStats()
    {
        var today = DateTime.Today;
        var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
        var firstDayOfPreviousMonth = firstDayOfMonth.AddMonths(-1);

        // Calculate total students and growth
        var totalStudents = _studentRepository.GetCount();
        var lastMonthStudents = _studentRepository.GetList(s => s.RecordDate < firstDayOfMonth).Count();
        var studentGrowth = lastMonthStudents > 0 
            ? (decimal)(totalStudents - lastMonthStudents) / lastMonthStudents * 100 
            : 0;

        // Calculate monthly income and growth
        var currentMonthIncome = _paymentRepository.GetList(p => 
            p.PaymentDate >= firstDayOfMonth && 
            p.PaymentDate < firstDayOfMonth.AddMonths(1) &&
            p.IsPaid
        ).Sum(p => p.Amount);

        var previousMonthIncome = _paymentRepository.GetList(p => 
            p.PaymentDate >= firstDayOfPreviousMonth && 
            p.PaymentDate < firstDayOfMonth &&
            p.IsPaid
        ).Sum(p => p.Amount);

        var monthlyIncomeGrowth = previousMonthIncome > 0 
            ? (currentMonthIncome - previousMonthIncome) / previousMonthIncome * 100 
            : 0;

        // Get delayed payments
        var delayedPayments = _paymentRepository.GetList(p => 
            p.DueDate < today && 
            !p.IsPaid
        ).ToList();

        // Get active sports and sessions
        var activeSports = _sessionRepository.GetList()
            .Select(s => s.BranchId)
            .Distinct()
            .Count();

        var activeSessions = _sessionRepository.GetList(s => 
            s.Date >= today
        ).Count();

        var stats = new AdminDashboardStatsDto
        {
            TotalStudents = totalStudents,
            MonthlyIncome = currentMonthIncome,
            MonthlyIncomeGrowth = decimal.Round(monthlyIncomeGrowth, 2),
            DelayedPaymentsCount = delayedPayments.Count,
            DelayedPaymentsAmount = delayedPayments.Sum(p => p.Amount),
            ActiveSportsCount = activeSports,
            ActiveSessionsCount = activeSessions,
            StudentGrowth = decimal.Round(studentGrowth, 2)
        };

        return new SuccessDataResult<AdminDashboardStatsDto>(stats, "Dashboard stats retrieved successfully.");
    }
}