using Core.Entities;

namespace Entities.Dtos.Register;

public class PaymentCreateDto : IDto
{
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
}