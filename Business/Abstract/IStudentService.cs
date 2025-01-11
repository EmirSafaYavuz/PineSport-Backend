using Business.Authentication.Model;
using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface IStudentService
{
    IResult RegisterStudent(StudentRegisterDto studentRegisterDto);
    IDataResult<StudentDto> GetStudentById(int studentId);
    IDataResult<List<StudentDto>> GetStudents();
    IDataResult<StudentDto> UpdateStudent(StudentUpdateDto studentDto);
    IResult DeleteStudent(int studentId);
    IDataResult<List<StudentDto>> SearchStudentsByName(string name);
    IDataResult<List<StudentDto>> GetStudentsByParentId(int id);
    IDataResult<List<StudentDto>> GetStudentsByBranchId(int id);
}