using AutoMapper;
using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;

namespace Business.Concrete;

public class ParentService : IParentService
{
    private readonly IParentRepository _parentRepository;
    private readonly IMapper _mapper;

    public ParentService(IParentRepository parentRepository, IMapper mapper)
    {
        _parentRepository = parentRepository;
        _mapper = mapper;
    }

    public IResult RegisterParent(ParentRegisterDto parentRegisterDto)
    {
        var parent = _mapper.Map<Parent>(parentRegisterDto);

        _parentRepository.Add(parent);
        return new SuccessResult("Parent registered successfully.");
    }

    public IDataResult<ParentDto> GetParentById(int parentId)
    {
        var parent = _parentRepository.Get(p => p.Id == parentId);

        if (parent == null)
        {
            return new ErrorDataResult<ParentDto>("Parent not found.");
        }

        // DTO'ya dönüştür
        var parentDto = _mapper.Map<ParentDto>(parent);
        return new SuccessDataResult<ParentDto>(parentDto);
    }

    public IDataResult<List<ParentDto>> GetParents()
    {
        var parents = _parentRepository.GetList().ToList();

        if (!parents.Any())
        {
            return new ErrorDataResult<List<ParentDto>>("No parents found.");
        }

        // DTO'ya dönüştür
        var parentDtos = _mapper.Map<List<ParentDto>>(parents);
        return new SuccessDataResult<List<ParentDto>>(parentDtos);
    }

    public IDataResult<ParentDto> UpdateParent(ParentUpdateDto parentUpdateDto)
    {
        var parent = _parentRepository.Get(p => p.Id == parentUpdateDto.Id);

        if (parent == null)
        {
            return new ErrorDataResult<ParentDto>("Parent not found.");
        }

        // DTO'dan gelen verileri entity'e ekle
        parent.FullName = parentUpdateDto.FullName;
        parent.Address = parentUpdateDto.Address;
        parent.Notes = parentUpdateDto.Notes;
        parent.Gender = parentUpdateDto.Gender;
        parent.BirthDate = parentUpdateDto.BirthDate;

        _parentRepository.Update(parent);
        return new SuccessDataResult<ParentDto>("Parent updated successfully.");
    }

    public IResult DeleteParent(int id)
    {
        var parent = _parentRepository.Get(p => p.Id == id);

        if (parent == null)
        {
            return new ErrorResult("Parent not found.");
        }

        _parentRepository.Delete(parent);
        return new SuccessResult("Parent deleted successfully.");
    }

    public IDataResult<List<ParentDto>> SearchParentsByName(string name)
    {
        var parents = _parentRepository.GetList(p => p.FullName.Contains(name)).ToList();

        if (!parents.Any())
        {
            return new ErrorDataResult<List<ParentDto>>("No parents found.");
        }

        // DTO'ya dönüştür
        var parentDtos = _mapper.Map<List<ParentDto>>(parents);
        return new SuccessDataResult<List<ParentDto>>(parentDtos);
    }
}
