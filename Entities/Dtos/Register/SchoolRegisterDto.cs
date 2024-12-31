namespace Entities.Dtos.Register;

public class SchoolRegisterDto : UserRegisterDto
{
    public string SchoolName { get; set; }
    public string SchoolAddress { get; set; }
    public string SchoolPhone { get; set; }
}