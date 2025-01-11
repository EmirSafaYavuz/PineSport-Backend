using Core.Entities;

namespace Entities.Dtos.Update;

public class PaymentUpdateDto : IDto
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
}