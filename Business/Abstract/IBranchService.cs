using Core.Utilities.Results;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Abstract;

public interface IBranchService
{
    IDataResult<List<BranchDto>> GetBranches();
    IResult RegisterBranch(BranchRegisterDto branchRegisterDto);
    IDataResult<BranchDto> GetBranchById(int branchId);
    IDataResult<List<BranchDto>> GetBranchesBySchoolId(int schoolId);
    IDataResult<BranchDto> UpdateBranch(BranchUpdateDto branchUpdateDto);
    IResult DeleteBranch(int id);
    IDataResult<List<BranchDto>> GetBranchesByTrainerId(int trainerId);
}