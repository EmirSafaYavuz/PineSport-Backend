using Core.Entities;

namespace Entities.Concrete;

public class StudentProgress : BaseEntity<int>
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
    
    public decimal Height { get; set; }
    public decimal Weight { get; set; }
    public DateTime RecordDate { get; set; }
    public string? TrainerNote { get; set; }

}