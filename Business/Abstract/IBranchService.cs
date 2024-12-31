using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Abstract;

public interface IBranchService
{
    IDataResult<List<BranchDto>> GetBranches();
    IResult RegisterBranch(BranchRegisterDto branchRegisterDto);
    IDataResult<BranchDto> GetBranchById(int branchId);
    IDataResult<List<BranchDto>> GetBranchesBySchoolId(int schoolId);
}