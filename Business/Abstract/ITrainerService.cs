using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface ITrainerService
{
    IResult RegisterTrainer(TrainerRegisterDto trainerRegisterDto);
    IDataResult<TrainerDto> GetTrainerById(int trainerId);
    IDataResult<List<TrainerDto>> GetTrainers();
    IDataResult<List<TrainerDto>> GetTrainersByBranchId(int branchId);
    IDataResult<TrainerDto> UpdateTrainer(TrainerUpdateDto trainerUpdateDto);
    IResult DeleteTrainer(int id);
    IDataResult<List<TrainerDto>> SearchTrainersByName(string name);
}