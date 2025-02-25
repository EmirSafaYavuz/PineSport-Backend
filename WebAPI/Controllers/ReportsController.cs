using Business.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : BaseApiController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // GET /api/reports/students
        [HttpGet("students")]
        public IActionResult GetStudentReport()
        {
            var result = _reportService.GetStudentReport();
            return GetResponseOnlyResultData(result);
        }

        // GET /api/reports/income?startDate={start}&endDate={end}
        [HttpGet("income")]
        public IActionResult GetIncomeReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _reportService.GetIncomeReport(startDate, endDate);
            return GetResponseOnlyResultData(result);
        }

        // GET /api/reports/attendance
        [HttpGet("attendance")]
        public IActionResult GetAttendanceReport()
        {
            var result = _reportService.GetAttendanceReport();
            return GetResponseOnlyResultData(result);
        }

        // GET /api/reports/new-registrations?startDate={start}&endDate={end}
        [HttpGet("new-registrations")]
        public IActionResult GetNewRegistrationsReport([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var result = _reportService.GetNewRegistrationsReport(startDate, endDate);
            return GetResponseOnlyResultData(result);
        }

        // GET /api/reports/admin-dashboard-stats
        [HttpGet("admin-dashboard-stats")]
        public IActionResult GetAdminDashboardStats()
        {
            var result = _reportService.GetAdminDashboardStats();
            return GetResponseOnlyResultData(result);
        }
    }
}