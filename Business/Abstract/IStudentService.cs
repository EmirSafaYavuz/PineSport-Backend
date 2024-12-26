using Business.Authentication.Model;
using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface IStudentService
{
    IResult RegisterStudent(StudentRegisterDto studentRegisterDto);
}