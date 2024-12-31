namespace Entities.Dtos.Register;

public class StudentRegisterDto : UserRegisterDto
{
    public int BranchId { get; set; }
    public int ParentId { get; set; }
}