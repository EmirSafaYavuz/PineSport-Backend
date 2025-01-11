namespace Entities.Dtos;

public class SessionAttendanceDto
{
    public int SessionId { get; set; }
    public string SessionName { get; set; }
    public int AttendanceCount { get; set; }
}