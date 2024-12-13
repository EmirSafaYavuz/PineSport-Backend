using Core.Entities;

namespace Entities.Concrete;

public class Payment : BaseEntity<int>
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
}