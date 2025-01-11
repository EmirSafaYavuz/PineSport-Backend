namespace Entities.Dtos.Update;

public class SchoolUpdateDto : UserUpdateDto
{
    public string SchoolName { get; set; }
    public string SchoolAddress { get; set; }
    public string SchoolPhone { get; set; }
}