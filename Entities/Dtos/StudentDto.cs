using Core.Entities;

namespace Entities.Dtos;

public class StudentDto : IDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string MobilePhone { get; set; }
    public DateTime BirthDate { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public string Notes { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; }
    public int ParentId { get; set; }
    public string ParentName { get; set; }
}