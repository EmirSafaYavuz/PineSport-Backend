using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using Business.Constants;
using Core.Utilities.Helpers;
using System.Linq.Expressions;

namespace Business.Concrete;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _branchRepository;
    private readonly ISchoolRepository _schoolRepository;
    private readonly IMapper _mapper;
    private readonly IUserContextHelper _userContextHelper;

    public BranchService(IBranchRepository branchRepository, 
        IMapper mapper, 
        ISchoolRepository schoolRepository,
        IUserContextHelper userContextHelper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _schoolRepository = schoolRepository;
        _userContextHelper = userContextHelper;
    }

    public IDataResult<List<BranchDto>> GetBranches()
    {
        Expression<Func<Branch, bool>> filter = null;

        if (_userContextHelper.IsInRole("school"))
        {
            var schoolId = _userContextHelper.GetCurrentSchoolId();
            filter = b => b.SchoolId == schoolId;
        }
        else if (!_userContextHelper.IsInRole("admin"))
        {
            throw new UnauthorizedAccessException(Messages.UnauthorizedBranchList);
        }

        var branches = _branchRepository.GetList(filter);
        var branchDtos = _mapper.Map<List<BranchDto>>(branches);

        return new SuccessDataResult<List<BranchDto>>(branchDtos, Messages.BranchesListed);
    }

    //[RoleRequirement("admin")]
    public IResult RegisterBranch(BranchRegisterDto branchRegisterDto)
    {
        var schoolExists = _schoolRepository.Query().Any(s => s.Id == branchRegisterDto.SchoolId);
        if (!schoolExists)
        {
            return new ErrorResult(Messages.SchoolNotFound);
        }

        var existingBranch = _branchRepository.Get(b => b.BranchName == branchRegisterDto.BranchName && b.SchoolId == branchRegisterDto.SchoolId);
        if (existingBranch != null)
        {
            return new ErrorResult(Messages.BranchAlreadyExists);
        }

        var branchEntity = _mapper.Map<Branch>(branchRegisterDto);
        if (branchEntity == null)
        {
            return new ErrorResult(Messages.InvalidBranchData);
        }

        _branchRepository.Add(branchEntity);
        return new SuccessResult(Messages.BranchRegistered);
    }

    public IDataResult<BranchDto> GetBranchById(int branchId)
    {
        var branch = _branchRepository.Get(b => b.Id == branchId);
        if (branch == null)
        {
            return new ErrorDataResult<BranchDto>(Messages.BranchNotFound);
        }

        var mappedBranch = _mapper.Map<BranchDto>(branch);
        return new SuccessDataResult<BranchDto>(mappedBranch, Messages.BranchGetSuccess);
    }

    public IDataResult<List<BranchDto>> GetBranchesBySchoolId(int schoolId)
    {
        var branches = _branchRepository.GetList(b => b.SchoolId == schoolId);
        var mappedBranches = _mapper.Map<List<BranchDto>>(branches);
        return new SuccessDataResult<List<BranchDto>>(mappedBranches, Messages.SchoolBranchesListed);
    }

    [RoleRequirement("admin")]
    public IDataResult<BranchDto> UpdateBranch(BranchUpdateDto branchUpdateDto)
    {
        var branch = _branchRepository.Get(b => b.Id == branchUpdateDto.Id);
        if (branch == null)
        {
            return new ErrorDataResult<BranchDto>(Messages.BranchNotFound);
        }

        var mappedBranch = _mapper.Map(branchUpdateDto, branch);

        if (mappedBranch == null)
        {
            return new ErrorDataResult<BranchDto>(Messages.InvalidBranchData);
        }

        _branchRepository.Update(mappedBranch);

        return new SuccessDataResult<BranchDto>(_mapper.Map<BranchDto>(mappedBranch), Messages.BranchUpdated);
    }

    [RoleRequirement("admin")]
    public IResult DeleteBranch(int id)
    {
        try
        {
            var branch = _branchRepository.Get(b => b.Id == id);
            if (branch == null)
            {
                return new ErrorResult(Messages.BranchNotFound);
            }

            if (HasRelatedData(branch))
            {
                return new ErrorResult(Messages.BranchHasRelatedRecords);
            }

            _branchRepository.Delete(branch);
            return new SuccessResult(Messages.BranchDeleted);
        }
        catch (Exception ex)
        {
            return new ErrorResult(ex.Message);
        }
    }

    private bool HasRelatedData(Branch branch)
    {
        return (branch.Students != null && branch.Students.Any()) ||
               (branch.Sessions != null && branch.Sessions.Any());
    }

    public IDataResult<List<BranchDto>> GetBranchesByTrainerId(int trainerId)
    {
        var branches = _branchRepository.GetBranchesByTrainerId(trainerId);
        var mappedBranches = _mapper.Map<List<BranchDto>>(branches);
        return new SuccessDataResult<List<BranchDto>>(mappedBranches, Messages.TrainerBranchesListed);
    }
}