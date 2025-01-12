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
public class BranchesControllerTests : BaseControllerTests<BranchDto, BranchRegisterDto, BranchUpdateDto>
{
    protected override string BaseEndpoint => "/api/branches";

    protected override BranchRegisterDto CreateTestDto()
    {
        return new BranchRegisterDto
        {
            BranchName = "Test Branch",
            BranchAddress = "Test Address",
            BranchPhone = "1234567890",
            SchoolId = 1
        };
    }

    protected override BranchUpdateDto UpdateTestDto()
    {
        return new BranchUpdateDto
        {
            Id = 1,
            BranchName = "Updated Branch",
            BranchAddress = "Updated Address",
            BranchPhone = "0987654321",
            SchoolId = 1
        };
    }

    [Test]
    public async Task GetBranches_AsSchool_ShouldReturnOwnBranches()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetSchoolClaims());

        // Act
        var response = await ExecuteGetRequest(token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task GetBranches_AsBranch_ShouldReturnUnauthorized()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetBranchClaims());

        // Act
        var response = await ExecuteGetRequest(token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}