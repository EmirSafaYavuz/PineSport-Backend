using Core.Entities;

namespace Entities.Dtos;

public class BranchRegisterDto : IDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    
    public int SchoolId { get; set; }
}