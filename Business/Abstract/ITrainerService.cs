using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface ITrainerService
{
    IResult RegisterTrainer(TrainerRegisterDto trainerRegisterDto);
}