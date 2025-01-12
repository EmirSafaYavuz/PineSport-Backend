using System.Net;
using Entities.Dtos.BaseDto;
using Entities.Dtos.Register;
using Entities.Dtos.Update;
using FluentAssertions;
using NUnit.Framework;
using Tests.Helpers;
using Tests.Helpers.Token;

namespace Tests.WebAPI;

[TestFixture]
public class SchoolsControllerTests : BaseControllerTests<SchoolDto, SchoolRegisterDto, SchoolUpdateDto>
{
    protected override string BaseEndpoint => "/api/schools";

    protected override SchoolRegisterDto CreateTestDto()
    {
        return new SchoolRegisterDto
        {
            SchoolName = "Test School",
            Email = "test@school.com",
            Password = "Test123!",
            SchoolAddress = "Test Address",
            SchoolPhone = "1234567890",
            FullName = "Test User",
            MobilePhone = "0987654321",
            Gender = 0,
            BirthDate = DateTime.Now.AddYears(-20)
        };
    }

    protected override SchoolUpdateDto UpdateTestDto()
    {
        return new SchoolUpdateDto
        {
            Id = 1,
            SchoolName = "Updated School",
            Address = "Updated Address",
            MobilePhone = "0987654321",
            Notes = "Updated Notes",
            Gender = 1,
            BirthDate = DateTime.Now.AddYears(-20)
        };
    }

    [Test]
    public async Task GetSchools_AsAdmin_ShouldReturnAllSchools()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());

        // Act
        var response = await ExecuteGetRequest(token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertResponseHasData<List<SchoolDto>>(response);
    }

    [Test]
    public async Task GetSchools_AsSchool_ShouldReturnUnauthorized()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetSchoolClaims());

        // Act
        var response = await ExecuteGetRequest(token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Test]
    public async Task RegisterSchool_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());
        var schoolDto = CreateTestDto();

        // Act
        var response = await ExecutePostRequest(token, schoolDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task GetSchoolById_WithValidId_ShouldReturnSchool()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());

        // Act
        var response = await ExecuteGetRequest(token, $"{BaseEndpoint}/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertResponseHasData<SchoolDto>(response);
    }

    [Test]
    public async Task GetSchoolBranches_WithValidId_ShouldReturnBranches()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());

        // Act
        var response = await ExecuteGetRequest(token, $"{BaseEndpoint}/1/branches");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task UpdateSchool_WithValidData_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());
        var updateDto = UpdateTestDto();

        // Act
        var response = await ExecutePutRequest(token, updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        await AssertResponseHasData<SchoolDto>(response);
    }

    [Test]
    public async Task DeleteSchool_WithValidId_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());

        // Act
        var response = await ExecuteDeleteRequest(token, 1);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
