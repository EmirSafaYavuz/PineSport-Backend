using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract;

public interface IBranchService
{
    IResult CreateBranch(BranchCreateDto branchCreateDto);
}