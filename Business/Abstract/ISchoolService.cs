using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface ISchoolService
{
    IResult RegisterSchool(SchoolRegisterDto schoolRegisterDto);
    IDataResult<List<SchoolDto>> GetSchools();
    IDataResult<SchoolDto> GetSchoolById(int schoolId);
    IDataResult<SchoolDto> UpdateSchool(SchoolUpdateDto schoolUpdateDto);
    IResult DeleteSchool(int schoolId);
}