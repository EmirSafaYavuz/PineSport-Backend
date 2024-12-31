using Core.Entities;

namespace Entities.Dtos.Register;

public class TrainerRegisterDto : UserRegisterDto
{
    public string Specialization { get; set; }
}