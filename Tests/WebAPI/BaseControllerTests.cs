using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.IoC;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Tests.Helpers;
using Tests.Helpers.Token;

namespace Tests.WebAPI;

public abstract class BaseControllerTests<TDto, TCreateDto, TUpdateDto> : BaseIntegrationTest
    where TDto : class
    where TCreateDto : class
    where TUpdateDto : class
{
    protected ICacheManager CacheManager;
    protected abstract string BaseEndpoint { get; }
    protected virtual string CacheKey => $"{CacheKeys.UserIdForClaim}=1";
    protected virtual List<string> CacheValues => new() { "GetQuery" };

    [SetUp]
    public virtual void Setup()
    {
        CacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
    }

    [TearDown]
    public virtual void TearDown()
    {
        if (CacheManager != null)
        {
            CacheManager.Remove(CacheKey);
        }
    }

    protected virtual async Task<HttpResponseMessage> ExecuteGetRequest(string token, string endpoint = null)
    {
        ConfigureHttpClient(token);
        return await HttpClient.GetAsync(endpoint ?? BaseEndpoint);
    }

    protected virtual async Task<HttpResponseMessage> ExecutePostRequest(string token, TCreateDto createDto, string endpoint = null)
    {
        ConfigureHttpClient(token);
        return await HttpClient.PostAsJsonAsync(endpoint ?? BaseEndpoint, createDto);
    }

    protected virtual async Task<HttpResponseMessage> ExecutePutRequest(string token, TUpdateDto updateDto, string endpoint = null)
    {
        ConfigureHttpClient(token);
        return await HttpClient.PutAsJsonAsync(endpoint ?? BaseEndpoint, updateDto);
    }

    protected virtual async Task<HttpResponseMessage> ExecuteDeleteRequest(string token, int id)
    {
        ConfigureHttpClient(token);
        return await HttpClient.DeleteAsync($"{BaseEndpoint}/{id}");
    }

    protected virtual void ConfigureHttpClient(string token)
    {
        HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        CacheManager?.Add(CacheKey, CacheValues);
    }

    [Test]
    public virtual async Task GetAll_AsAdmin_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());

        // Act
        var response = await ExecuteGetRequest(token);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public virtual async Task GetAll_AsUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetUnauthorizedClaims());

        // Act
        var response = await ExecuteGetRequest(token);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        content.Should().NotBeNull();
        content.Should().Contain("Unauthorized");
    }

    [Test]
    public virtual async Task GetById_AsAdmin_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());

        // Act
        var response = await ExecuteGetRequest(token, $"{BaseEndpoint}/1");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public virtual async Task GetById_AsUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetUnauthorizedClaims());

        // Act
        var response = await ExecuteGetRequest(token, $"{BaseEndpoint}/1");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        content.Should().NotBeNull();
        content.Should().Contain("Unauthorized");
    }

    [Test]
    public virtual async Task Create_AsAdmin_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());
        var createDto = CreateTestDto();

        // Act
        var response = await ExecutePostRequest(token, createDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public virtual async Task Create_AsUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetUnauthorizedClaims());
        var createDto = CreateTestDto();

        // Act
        var response = await ExecutePostRequest(token, createDto);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        content.Should().NotBeNull();
        content.Should().Contain("Unauthorized");
    }

    [Test]
    public virtual async Task Update_AsAdmin_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());
        var updateDto = UpdateTestDto();

        // Act
        var response = await ExecutePutRequest(token, updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public virtual async Task Update_AsUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetUnauthorizedClaims());
        var updateDto = UpdateTestDto();

        // Act
        var response = await ExecutePutRequest(token, updateDto);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        content.Should().NotBeNull();
        content.Should().Contain("Unauthorized");
    }

    [Test]
    public virtual async Task Delete_AsAdmin_ShouldReturnSuccess()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetAdminClaims());

        // Act
        var response = await ExecuteDeleteRequest(token, 1);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public virtual async Task Delete_AsUnauthorized_ShouldReturnUnauthorized()
    {
        // Arrange
        var token = MockJwtTokens.GenerateJwtToken(ClaimsData.GetUnauthorizedClaims());

        // Act
        var response = await ExecuteDeleteRequest(token, 1);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        content.Should().NotBeNull();
        content.Should().Contain("Unauthorized");
    }

    protected abstract TCreateDto CreateTestDto();
    protected abstract TUpdateDto UpdateTestDto();

    protected virtual async Task AssertResponseHasError(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();
        content.Should().Contain("error", "Error message should be present in response");
    }

    protected virtual async Task AssertResponseHasData<T>(HttpResponseMessage response) where T : class
    {
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNull();
        
        var result = System.Text.Json.JsonSerializer.Deserialize<T>(content);
        result.Should().NotBeNull("Response should contain valid data");
    }
}
