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
        try
        {
            Expression<Func<Branch, bool>> filter = null;

            if (_userContextHelper.IsInRole("school"))
            {
                var schoolId = _userContextHelper.GetCurrentSchoolId();
                filter = b => b.SchoolId == schoolId;
            }
            else if (!_userContextHelper.IsInRole("admin"))
            {
                throw new UnauthorizedAccessException("Şubeleri listelemek için yetkiniz yok.");
            }

            var branches = _branchRepository.GetList(filter);
            var branchDtos = _mapper.Map<List<BranchDto>>(branches);
            return new SuccessDataResult<List<BranchDto>>(branchDtos, "Şubeler başarıyla listelendi.");
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<List<BranchDto>>(ex.Message);
        }
    }

    [RoleRequirement("admin")]
    public IResult RegisterBranch(BranchRegisterDto branchRegisterDto)
    {
        // 1. SchoolId Kontrolü
        var schoolExists = _schoolRepository.Query().Any(s => s.Id == branchRegisterDto.SchoolId);
        if (!schoolExists)
        {
            return new ErrorResult("Belirtilen okul mevcut değil.");
        }

        // 2. Dto'dan Entity'ye Mapleme
        var branchEntity = _mapper.Map<Branch>(branchRegisterDto);

        // 3. Veri Doğrulama
        if (branchEntity == null)
        {
            return new ErrorResult("Branch bilgileri eksik veya hatalı.");
        }

        // Aynı isimde bir şube olup olmadığını kontrol etme
        var existingBranch = _branchRepository.Get(b => b.BranchName == branchRegisterDto.BranchName && b.SchoolId == branchRegisterDto.SchoolId);
        if (existingBranch != null)
        {
            return new ErrorResult("Bu isimde bir şube zaten mevcut.");
        }

        // 4. Şube Kaydı
        _branchRepository.Add(branchEntity);
        
        return new SuccessResult("Şube başarıyla kaydedildi.");
    }

    [RoleRequirement("admin")]
    public IDataResult<BranchDto> GetBranchById(int branchId)
    {
        var branch = _branchRepository.Get(b => b.Id == branchId);
        if (branch == null)
        {
            return new ErrorDataResult<BranchDto>("Şube bulunamadı.");
        }

        var mappedBranch = _mapper.Map<BranchDto>(branch);
        return new SuccessDataResult<BranchDto>(mappedBranch, "Şube başarıyla getirildi.");
    }

    public IDataResult<List<BranchDto>> GetBranchesBySchoolId(int schoolId)
    {
        var branches = _branchRepository.GetList(b => b.SchoolId == schoolId);
        var mappedBranches = _mapper.Map<List<BranchDto>>(branches);
        return new SuccessDataResult<List<BranchDto>>(mappedBranches, "Okula ait şubeler başarıyla getirildi.");
    }

    [RoleRequirement("admin")]
    public IDataResult<BranchDto> UpdateBranch(BranchUpdateDto branchUpdateDto)
    {
        var branch = _branchRepository.Get(b => b.Id == branchUpdateDto.Id);
        if (branch == null)
        {
            return new ErrorDataResult<BranchDto>("Şube bulunamadı.");
        }

        var mappedBranch = _mapper.Map(branchUpdateDto, branch);

        if (mappedBranch == null)
        {
            return new ErrorDataResult<BranchDto>("Branch bilgileri eksik veya hatalı.");
        }

        // 4. Şube Güncelleme
        _branchRepository.Update(mappedBranch);

        return new SuccessDataResult<BranchDto>(_mapper.Map<BranchDto>(mappedBranch), "Şube başarıyla güncellendi.");
    }

    [RoleRequirement("admin")]
    public IResult DeleteBranch(int id)
    {
        try
        {
            var branch = _branchRepository.Get(b => b.Id == id);
            if (branch == null)
            {
                return new ErrorResult("Şube bulunamadı.");
            }

            if (HasRelatedData(branch))
            {
                return new ErrorResult("İlişkili kayıtlar olduğu için şube silinemez.");
            }

            _branchRepository.Delete(branch);
            return new SuccessResult("Şube başarıyla silindi.");
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
        return new SuccessDataResult<List<BranchDto>>(mappedBranches, "Eğitmenin bağlı olduğu şubeler başarıyla getirildi.");
    }
}