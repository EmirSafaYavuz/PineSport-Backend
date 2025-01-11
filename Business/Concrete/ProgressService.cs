using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Concrete;

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _progressRepository;
    private readonly IMapper _mapper;

    public ProgressService(IProgressRepository progressRepository, IMapper mapper)
    {
        _progressRepository = progressRepository;
        _mapper = mapper;
    }

    public IDataResult<List<ProgressDto>> GetAllProgresses()
    {
        var progresses = _progressRepository.GetList(null, includeProperties: "Student");
        var mappedProgresses = _mapper.Map<List<ProgressDto>>(progresses);
        return new SuccessDataResult<List<ProgressDto>>(mappedProgresses, "Progress records retrieved successfully.");
    }

    public IDataResult<ProgressDto> GetProgressByStudentId(int studentId)
    {
        var progress = _progressRepository.Get(p => p.StudentId == studentId, includeProperties: "Student");
        if (progress == null)
            return new ErrorDataResult<ProgressDto>("Progress record not found for the student.");

        var mappedProgress = _mapper.Map<ProgressDto>(progress);
        return new SuccessDataResult<ProgressDto>(mappedProgress, "Progress record retrieved successfully.");
    }

    public IResult CreateProgress(ProgressCreateDto progressCreateDto)
    {
        var progress = _mapper.Map<Progress>(progressCreateDto);
        progress.RecordDate = DateTime.Now; // Set current date for the record
        _progressRepository.Add(progress);
        return new SuccessResult("Progress record created successfully.");
    }

    public IResult UpdateProgress(ProgressUpdateDto progressUpdateDto)
    {
        var existingProgress = _progressRepository.Get(p => p.StudentId == progressUpdateDto.StudentId);
        if (existingProgress == null)
            return new ErrorResult("Progress record not found for the student.");

        _mapper.Map(progressUpdateDto, existingProgress);
        _progressRepository.Update(existingProgress);
        return new SuccessResult("Progress record updated successfully.");
    }

    public IResult DeleteProgress(int studentId)
    {
        var existingProgress = _progressRepository.Get(p => p.StudentId == studentId);
        if (existingProgress == null)
            return new ErrorResult("Progress record not found for the student.");

        _progressRepository.Delete(existingProgress);
        return new SuccessResult("Progress record deleted successfully.");
    }
}