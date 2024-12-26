using Core.Entities;

namespace Entities.Dtos;

public class StudentRegisterDto : UserRegisterDto
{
    public int BranchId { get; set; }
    public int ParentId { get; set; }
}