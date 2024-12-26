using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface ISchoolService
{
    IResult RegisterSchool(SchoolRegisterDto schoolRegisterDto);
    IDataResult<List<SchoolDto>> GetSchools();
    IDataResult<SchoolDto> GetSchoolById(int schoolId);
}