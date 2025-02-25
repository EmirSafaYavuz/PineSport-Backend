using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Concrete;

public class TrainerService : ITrainerService
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IMapper _mapper;

    public TrainerService(ITrainerRepository trainerRepository, IMapper mapper)
    {
        _trainerRepository = trainerRepository;
        _mapper = mapper;
    }

    public IResult RegisterTrainer(TrainerRegisterDto trainerRegisterDto)
    {
        var trainer = _mapper.Map<Trainer>(trainerRegisterDto);

        _trainerRepository.Add(trainer);

        return new SuccessResult("Trainer successfully registered.");
    }

    public IDataResult<TrainerDto> GetTrainerById(int trainerId)
    {
        var trainer = _trainerRepository.Get(t => t.Id == trainerId);

        if (trainer == null)
        {
            return new ErrorDataResult<TrainerDto>("Trainer not found.");
        }

        var trainerDto = _mapper.Map<TrainerDto>(trainer);
        return new SuccessDataResult<TrainerDto>(trainerDto);
    }

    public IDataResult<List<TrainerDto>> GetTrainers()
    {
        var trainers = _trainerRepository.GetList().ToList();

        var trainerDtos = _mapper.Map<List<TrainerDto>>(trainers);

        return new SuccessDataResult<List<TrainerDto>>(trainerDtos);
    }

    public IDataResult<List<TrainerDto>> GetTrainersByBranchId(int branchId)
    {
        var trainers = _trainerRepository.GetTrainersByBranchId(branchId);

        var trainerDtos = _mapper.Map<List<TrainerDto>>(trainers);

        return new SuccessDataResult<List<TrainerDto>>(trainerDtos);
    }

    public IDataResult<TrainerDto> UpdateTrainer(TrainerUpdateDto trainerUpdateDto)
    {
        var trainer = _mapper.Map<Trainer>(trainerUpdateDto);

        _trainerRepository.Update(trainer);

        var trainerDto = _mapper.Map<TrainerDto>(trainer);

        return new SuccessDataResult<TrainerDto>(trainerDto);
    }

    public IResult DeleteTrainer(int id)
    {
        var trainer = _trainerRepository.Get(t => t.Id == id);

        if (trainer == null)
        {
            return new ErrorResult("Trainer not found.");
        }

        _trainerRepository.Delete(trainer);

        return new SuccessResult("Trainer successfully deleted.");
    }

    public IDataResult<List<TrainerDto>> SearchTrainersByName(string name)
    {
        var trainers = _trainerRepository.GetList(t => t.FullName.Contains(name)).ToList();

        var trainerDtos = _mapper.Map<List<TrainerDto>>(trainers);

        return new SuccessDataResult<List<TrainerDto>>(trainerDtos);
    }
}