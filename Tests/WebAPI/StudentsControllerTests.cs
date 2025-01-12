using System.Net;
using Entities.Dtos;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using FluentAssertions;
using NUnit.Framework;
using Tests.Helpers;
using Tests.Helpers.Token;

namespace Tests.WebAPI;

[TestFixture]
public class StudentsControllerTests : BaseControllerTests<StudentDto, StudentRegisterDto, StudentUpdateDto>
{
    protected override string BaseEndpoint => "/api/students";

    protected override StudentRegisterDto CreateTestDto()
    {
        return new StudentRegisterDto
        {
            FullName = "Test Student",
            Email = "test.student@example.com",
            Password = "Test123!",
            MobilePhone = "1234567890",
            BirthDate = DateTime.Now.AddYears(-15),
            Gender = 0,
            Address = "Test Address",
            Notes = "Test Notes",
            BranchId = 1,
            ParentId = 1
        };
    }

    protected override StudentUpdateDto UpdateTestDto()
    {
        return new StudentUpdateDto
        {
            Id = 1,
            FullName = "Updated Student",
            MobilePhone = "0987654321",
            BirthDate = DateTime.Now.AddYears(-16),
            Gender = 1,
            Address = "Updated Address",
            Notes = "Updated Notes"
        };
    }

    [Test]
    public async Task GetAll_AsBranch_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetBranchClaims());

        // Act
        var response = await ExecuteGetRequest(token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertResponseHasData<List<StudentDto>>(response);
    }

    [Test]
    public async Task SearchStudents_WithValidName_ShouldReturnMatchingStudents()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetBranchClaims());
        var searchName = "Test";

        // Act
        var response = await ExecuteGetRequest(token, $"{BaseEndpoint}/search?name={searchName}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertResponseHasData<List<StudentDto>>(response);
    }

    [Test]
    public async Task GetStudentSessions_WithValidId_ShouldReturnSessions()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetBranchClaims());
        var studentId = 1;

        // Act
        var response = await ExecuteGetRequest(token, $"{BaseEndpoint}/{studentId}/sessions");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertResponseHasData<List<SessionDto>>(response);
    }

    [Test]
    public async Task Create_AsBranch_ShouldReturnCreated()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetBranchClaims());
        var createDto = CreateTestDto();

        // Act
        var response = await ExecutePostRequest(token, createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Create_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetBranchClaims());
        var invalidDto = CreateTestDto();
        invalidDto.Email = "invalid-email";

        // Act
        var response = await ExecutePostRequest(token, invalidDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        await AssertResponseHasError(response);
    }
}
