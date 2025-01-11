using Core.Utilities.Results;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface IProgressService
{
    IDataResult<List<ProgressDto>> GetAllProgresses();
    IDataResult<ProgressDto> GetProgressByStudentId(int studentId);
    IResult CreateProgress(ProgressCreateDto progressCreateDto);
    IResult UpdateProgress(ProgressUpdateDto progressUpdateDto);
    IResult DeleteProgress(int studentId);
}