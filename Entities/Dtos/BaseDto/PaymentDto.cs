using Core.Entities;

namespace Entities.Dtos.BaseDto;

public class PaymentDto : IDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
    public string StudentName { get; set; }
    public string ParentName { get; set; }
    public string ParentEmail { get; set; }
}