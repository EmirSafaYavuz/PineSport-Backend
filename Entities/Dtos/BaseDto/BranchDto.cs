using Core.Entities;

namespace Entities.Dtos;

public class BranchDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public int SchoolId { get; set; }
    public string SchoolName { get; set; }
}