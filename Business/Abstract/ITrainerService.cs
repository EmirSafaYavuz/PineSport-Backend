using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Abstract;

public interface ITrainerService
{
    IResult RegisterTrainer(TrainerRegisterDto trainerRegisterDto);
    IDataResult<TrainerDto> GetTrainerById(int trainerId);
    IDataResult<List<TrainerDto>> GetTrainers();
}