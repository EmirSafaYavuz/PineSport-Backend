using Core.Entities;

namespace Entities.Dtos;

public class SchoolDto : IDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string ManagerName { get; set; }
    public string ManagerEmail { get; set; }
    public string ManagerPhone { get; set; }
}