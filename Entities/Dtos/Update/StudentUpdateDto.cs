namespace Entities.Dtos.Update;

public class StudentUpdateDto : UserUpdateDto
{
    public int BranchId { get; set; }
    public int ParentId { get; set; }
}