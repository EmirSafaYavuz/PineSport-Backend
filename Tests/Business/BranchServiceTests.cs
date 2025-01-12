using System.Linq.Expressions;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Business.Constants;
using Core.Utilities.Helpers;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Tests.Business;

[TestFixture]
public class BranchServiceTests
{
    private Mock<IBranchRepository> _branchRepositoryMock;
    private Mock<ISchoolRepository> _schoolRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<IUserContextHelper> _userContextHelperMock;
    private IBranchService _branchService;

    [SetUp]
    public void Setup()
    {
        _branchRepositoryMock = new Mock<IBranchRepository>();
        _schoolRepositoryMock = new Mock<ISchoolRepository>();
        _mapperMock = new Mock<IMapper>();
        _userContextHelperMock = new Mock<IUserContextHelper>();
        
        _branchService = new BranchService(
            _branchRepositoryMock.Object,
            _mapperMock.Object,
            _schoolRepositoryMock.Object,
            _userContextHelperMock.Object
        );
    }

    [Test]
    public void GetBranches_WhenUserIsAdmin_ShouldReturnAllBranches()
    {
        // Arrange
        var branches = new List<Branch> 
        { 
            new() { Id = 1, BranchName = "Test Branch", SchoolId = 1 } 
        };
        var branchDtos = new List<BranchDto> 
        { 
            new() { Id = 1, Name = "Test Branch", SchoolId = 1 } 
        };

        _userContextHelperMock.Setup(x => x.IsInRole("admin")).Returns(true);
        _branchRepositoryMock.Setup(x => x.GetList(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns(branches);
        _mapperMock.Setup(x => x.Map<List<BranchDto>>(branches)).Returns(branchDtos);

        // Act
        var result = _branchService.GetBranches();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(branchDtos);
    }

    [Test]
    public void GetBranches_WhenUserIsUnauthorized_ShouldThrowException()
    {
        // Arrange
        _userContextHelperMock.Setup(x => x.IsInRole("admin")).Returns(false);
        _userContextHelperMock.Setup(x => x.IsInRole("school")).Returns(false);

        // Act & Assert
        FluentActions.Invoking(() => _branchService.GetBranches())
            .Should().Throw<UnauthorizedAccessException>();
    }

    [Test]
    public void RegisterBranch_WhenSchoolDoesNotExist_ShouldReturnError()
    {
        // Arrange
        var branchRegisterDto = new BranchRegisterDto 
        { 
            SchoolId = 1, 
            BranchName = "New Branch" 
        };

        _schoolRepositoryMock.Setup(x => x.Query())
            .Returns(new List<School>().AsQueryable());

        // Act
        var result = _branchService.RegisterBranch(branchRegisterDto);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be(Messages.SchoolNotFound);
    }

    [Test]
    public void RegisterBranch_WhenBranchNameExists_ShouldReturnError()
    {
        // Arrange
        var branchRegisterDto = new BranchRegisterDto 
        { 
            SchoolId = 1, 
            BranchName = "Existing Branch" 
        };

        _schoolRepositoryMock.Setup(x => x.Query())
            .Returns(new List<School> { new() { Id = 1 } }.AsQueryable());
        
        _branchRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns(new Branch { BranchName = "Existing Branch" });

        // Act
        var result = _branchService.RegisterBranch(branchRegisterDto);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be(Messages.BranchAlreadyExists);
    }

    [Test]
    public void UpdateBranch_WhenBranchNotFound_ShouldReturnError()
    {
        // Arrange
        var updateDto = new BranchUpdateDto { Id = 1 };
        _branchRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns((Branch)null);

        // Act
        var result = _branchService.UpdateBranch(updateDto);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be(Messages.BranchNotFound);
    }

    [Test]
    public void DeleteBranch_WhenBranchHasRelatedData_ShouldReturnError()
    {
        // Arrange
        const int branchId = 1;
        var branch = new Branch 
        { 
            Id = branchId,
            Students = new List<Student> { new Student() }
        };

        _branchRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns(branch);

        // Act
        var result = _branchService.DeleteBranch(branchId);

        // Assert
        result.Success.Should().BeFalse();
        result.Message.Should().Be(Messages.BranchHasRelatedRecords);
    }

    [Test]
    public void RegisterBranch_WhenValidData_ShouldSucceed()
    {
        // Arrange
        var branchRegisterDto = new BranchRegisterDto 
        { 
            SchoolId = 1, 
            BranchName = "New Branch",
            BranchAddress = "Test Address",
            BranchPhone = "1234567890"
        };
        var branch = new Branch 
        { 
            SchoolId = 1, 
            BranchName = "New Branch",
            BranchAddress = "Test Address",
            BranchPhone = "1234567890"
        };

        _schoolRepositoryMock.Setup(x => x.Query())
            .Returns(new List<School> { new() { Id = 1 } }.AsQueryable());
        _branchRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns((Branch)null);
        _mapperMock.Setup(x => x.Map<Branch>(branchRegisterDto)).Returns(branch);

        // Act
        var result = _branchService.RegisterBranch(branchRegisterDto);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be(Messages.BranchRegistered);
        _branchRepositoryMock.Verify(x => x.Add(It.IsAny<Branch>()), Times.Once);
    }

    [Test]
    public void UpdateBranch_WhenValidData_ShouldSucceed()
    {
        // Arrange
        var updateDto = new BranchUpdateDto 
        { 
            Id = 1,
            BranchName = "Updated Branch",
            BranchAddress = "Updated Address",
            BranchPhone = "9876543210",
            SchoolId = 1
        };
        var existingBranch = new Branch { Id = 1, BranchName = "Old Branch" };
        var updatedBranch = new Branch 
        { 
            Id = 1,
            BranchName = "Updated Branch",
            BranchAddress = "Updated Address",
            BranchPhone = "9876543210",
            SchoolId = 1
        };
        var branchDto = new BranchDto 
        { 
            Id = 1,
            Name = "Updated Branch",
            Address = "Updated Address",
            Phone = "9876543210",
            SchoolId = 1
        };

        _branchRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns(existingBranch);
        _mapperMock.Setup(x => x.Map(updateDto, existingBranch)).Returns(updatedBranch);
        _mapperMock.Setup(x => x.Map<BranchDto>(updatedBranch)).Returns(branchDto);

        // Act
        var result = _branchService.UpdateBranch(updateDto);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(branchDto);
        _branchRepositoryMock.Verify(x => x.Update(It.IsAny<Branch>()), Times.Once);
    }

    [Test]
    public void GetBranchesBySchoolId_WhenSchoolHasBranches_ShouldReturnBranches()
    {
        // Arrange
        const int schoolId = 1;
        var branches = new List<Branch> 
        { 
            new() 
            { 
                Id = 1, 
                BranchName = "Branch 1", 
                SchoolId = schoolId 
            },
            new() 
            { 
                Id = 2, 
                BranchName = "Branch 2", 
                SchoolId = schoolId 
            }
        };
        var branchDtos = new List<BranchDto> 
        { 
            new() 
            { 
                Id = 1, 
                Name = "Branch 1", 
                SchoolId = schoolId 
            },
            new() 
            { 
                Id = 2, 
                Name = "Branch 2", 
                SchoolId = schoolId 
            }
        };

        _branchRepositoryMock.Setup(x => x.GetList(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns(branches);
        _mapperMock.Setup(x => x.Map<List<BranchDto>>(branches)).Returns(branchDtos);

        // Act
        var result = _branchService.GetBranchesBySchoolId(schoolId);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().BeEquivalentTo(branchDtos);
    }

    [Test]
    public void GetBranchesByTrainerId_WhenTrainerHasBranches_ShouldReturnBranches()
    {
        // Arrange
        const int trainerId = 1;
        var branches = new List<Branch> 
        { 
            new() { Id = 1, BranchName = "Trainer Branch 1" },
            new() { Id = 2, BranchName = "Trainer Branch 2" }
        };
        var branchDtos = new List<BranchDto> 
        { 
            new() { Id = 1, Name = "Trainer Branch 1" },
            new() { Id = 2, Name = "Trainer Branch 2" }
        };

        _branchRepositoryMock.Setup(x => x.GetBranchesByTrainerId(trainerId))
            .Returns(branches);
        _mapperMock.Setup(x => x.Map<List<BranchDto>>(branches)).Returns(branchDtos);

        // Act
        var result = _branchService.GetBranchesByTrainerId(trainerId);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data.Should().BeEquivalentTo(branchDtos);
    }

    [Test]
    public void GetBranchById_WhenBranchExists_ShouldReturnBranch()
    {
        // Arrange
        const int branchId = 1;
        var branch = new Branch 
        { 
            Id = branchId, 
            BranchName = "Test Branch",
            BranchAddress = "Test Address",
            BranchPhone = "1234567890",
            SchoolId = 1
        };
        var branchDto = new BranchDto 
        { 
            Id = branchId, 
            Name = "Test Branch",
            Address = "Test Address",
            Phone = "1234567890",
            SchoolId = 1
        };

        _branchRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns(branch);
        _mapperMock.Setup(x => x.Map<BranchDto>(branch)).Returns(branchDto);

        // Act
        var result = _branchService.GetBranchById(branchId);

        // Assert
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(branchDto);
    }

    [Test]
    public void DeleteBranch_WhenNoRelatedData_ShouldSucceed()
    {
        // Arrange
        const int branchId = 1;
        var branch = new Branch 
        { 
            Id = branchId,
            BranchName = "Test Branch",
            Students = new List<Student>(),
            Sessions = new List<Session>()
        };

        _branchRepositoryMock.Setup(x => x.Get(It.IsAny<Expression<Func<Branch, bool>>>()))
            .Returns(branch);

        // Act
        var result = _branchService.DeleteBranch(branchId);

        // Assert
        result.Success.Should().BeTrue();
        result.Message.Should().Be(Messages.BranchDeleted);
        _branchRepositoryMock.Verify(x => x.Delete(It.IsAny<Branch>()), Times.Once);
    }
}
