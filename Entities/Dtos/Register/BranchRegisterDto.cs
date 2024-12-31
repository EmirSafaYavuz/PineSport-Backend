using Core.Entities;

namespace Entities.Dtos.Register;

public class BranchRegisterDto : UserRegisterDto
{
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string BranchPhone { get; set; }
    
    public int SchoolId { get; set; }
}