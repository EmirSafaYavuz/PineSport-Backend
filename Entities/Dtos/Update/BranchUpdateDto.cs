namespace Entities.Dtos.Update;

public class BranchUpdateDto : UserUpdateDto
{
    public string BranchName { get; set; }
    public string BranchAddress { get; set; }
    public string BranchPhone { get; set; }
    public int SchoolId { get; set; }
}