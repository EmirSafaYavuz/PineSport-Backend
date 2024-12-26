using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Concrete;

public class BranchService : IBranchService
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;

    public BranchService(IBranchRepository branchRepository, IMapper mapper)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
    }

    public IResult CreateBranch(BranchCreateDto branchCreateDto)
    {
        // 1. SchoolId Kontrolü
        var schoolExists = _branchRepository.Query().Any(s => s.SchoolId == branchCreateDto.SchoolId);
        if (!schoolExists)
        {
            return new ErrorResult("Belirtilen okul mevcut değil.");
        }

        // 2. Dto'dan Entity'ye Mapleme
        var branchEntity = _mapper.Map<Branch>(branchCreateDto);

        // 3. Veri Doğrulama
        if (branchEntity == null)
        {
            return new ErrorResult("Branch bilgileri eksik veya hatalı.");
        }

        // Aynı isimde bir şube olup olmadığını kontrol etme
        var existingBranch = _branchRepository.Get(b => b.Name == branchCreateDto.Name && b.SchoolId == branchCreateDto.SchoolId);
        if (existingBranch != null)
        {
            return new ErrorResult("Bu isimde bir şube zaten mevcut.");
        }

        // 4. Şube Kaydı
        _branchRepository.Add(branchEntity);

        // 5. Veritabanı Kaydetme
        var saveResult = _branchRepository.SaveChanges();
        if (saveResult > 0)
        {
            return new SuccessResult("Şube başarıyla kaydedildi.");
        }

        return new ErrorResult("Şube kaydedilemedi, lütfen tekrar deneyin.");
    }
}