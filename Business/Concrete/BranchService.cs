using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.Register;

namespace Business.Concrete;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _branchRepository;
    private readonly ISchoolRepository _schoolRepository;
    private readonly IMapper _mapper;

    public BranchService(IBranchRepository branchRepository, IMapper mapper, ISchoolRepository schoolRepository)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _schoolRepository = schoolRepository;
    }

    public IDataResult<List<BranchDto>> GetBranches()
    {
        var branches = _branchRepository.GetList();

        var mappedBranches = _mapper.Map<List<BranchDto>>(branches);
        return new SuccessDataResult<List<BranchDto>>(mappedBranches, "Şubeler başarıyla listelendi.");
    }

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
}