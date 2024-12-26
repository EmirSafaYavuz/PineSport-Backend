using Core.Entities;

namespace Entities.Dtos;

public class StudentRegisterDto : RegisterDto
{
    public int BranchId { get; set; }
    public int ParentId { get; set; }
}