using AutoMapper;
using Business.Abstract;
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

    public IDataResult<StudentReportDto> GetStudentReport()
    {
        var totalStudents = _studentRepository.GetCount();
        return new SuccessDataResult<StudentReportDto>(new StudentReportDto
        {
            TotalStudents = totalStudents
        }, "Student report generated successfully.");
    }

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
}