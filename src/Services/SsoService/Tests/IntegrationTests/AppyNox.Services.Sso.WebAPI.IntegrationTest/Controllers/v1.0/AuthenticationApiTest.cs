using AppyNox.Services.Base.API.Constants;
using AppyNox.Services.Base.API.Wrappers;
using AppyNox.Services.Base.Application.Exceptions.Base;
using AppyNox.Services.Base.IntegrationTests.URIs;
using AppyNox.Services.Base.IntegrationTests.Wrapper;
using AppyNox.Services.Base.IntegrationTests.Wrapper.Helpers;
using AppyNox.Services.Sso.Application.DTOs.AccountDtos.Models;
using AppyNox.Services.Sso.Application.DTOs.RefreshTokenDtos.Models;
using AppyNox.Services.Sso.Infrastructure.Exceptions.Base;
using AppyNox.Services.Sso.WebAPI.Exceptions.Base;
using AppyNox.Services.Sso.WebAPI.IntegrationTest.Fixtures;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit.Extensions.Ordering;

namespace AppyNox.Services.Sso.WebAPI.IntegrationTest.Controllers.v1._0;

[Collection("SsoService Collection")]
public class AuthenticationApiTest(SsoServiceFixture ssoApiTestFixture)
{
    #region [ Fields ]

    private readonly JsonSerializerOptions _jsonSerializerOptions = ssoApiTestFixture.JsonSerializerOptions;

    private readonly SsoServiceFixture _ssoApiTestFixture = ssoApiTestFixture;

    private readonly HttpClient _client = ssoApiTestFixture.Client;

    private readonly ServiceURIs _serviceURIs = ssoApiTestFixture.ServiceURIs;

    #endregion

    #region [ Public Methods ]

    [Theory]
    [InlineData("admin", "Admin@123", "AppyNox")]
    [InlineData("admin", "Admin@123", "AppyFleet")]
    [Order(1)]
    public async Task ConnectToken_ShouldReturnToken(string username, string password, string audience)
    {
        #region [ Get Token ]

        // Act
        var content = new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        username,
                        password,
                        audience
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );
        string uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/connect/token";
        HttpResponseMessage response = await _client.PostAsync(uri, content);

        NoxApiResponse<LoginResultDto> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<LoginResultDto>(response, jsonSerializerOptions: _jsonSerializerOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(wrappedResponse.Result);
        Assert.False(wrappedResponse.HasError);

        Assert.NotNull(wrappedResponse.Result.Data);
        Assert.False(string.IsNullOrEmpty(wrappedResponse.Result.Data.Token));

        #endregion

        #region [ Verify Token ]

        // Act
        uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/verify-token/{wrappedResponse.Result.Data.Token}?audience={audience}";
        response = await _client.GetAsync(uri);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        #endregion
    }

    [Theory]
    [InlineData("wrong", "Admin@123", "AppyNox", (int)NoxSsoInfrastructureExceptionCode.WrongCredentials, HttpStatusCode.BadRequest)]
    [InlineData("admin", "wrong", "AppyNox", (int)NoxSsoInfrastructureExceptionCode.WrongCredentials, HttpStatusCode.BadRequest)]
    [InlineData("admin", "Admin@123", "wrong", (int)NoxSsoInfrastructureExceptionCode.InvalidAudience, HttpStatusCode.BadRequest)]
    [InlineData("admin", "wrong", "wrong", (int)NoxSsoInfrastructureExceptionCode.WrongCredentials, HttpStatusCode.BadRequest)]
    [Order(2)]
    public async Task ConnectToken_ShouldReturnCorrectErrorResponse(string username, string password, string audience, int exceptionCode, HttpStatusCode statusCode)
    {
        #region [ Get Token ]

        // Act
        var content = new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        username,
                        password,
                        audience
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );
        string uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/connect/token";
        HttpResponseMessage response = await _client.PostAsync(uri, content);

        NoxApiResponse<NoxApiExceptionWrapObjectPOCO> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<NoxApiExceptionWrapObjectPOCO>(response, jsonSerializerOptions: _jsonSerializerOptions);
        NoxApiExceptionWrapObjectPOCO? errorObject = (wrappedResponse.Result.Error as NoxApiExceptionWrapObjectPOCO);

        // Assert
        Assert.Equal(statusCode, response.StatusCode);
        Assert.True(wrappedResponse.HasError);
        Assert.NotNull(errorObject);
        Assert.Null(errorObject.InnerException);
        Assert.Equal(exceptionCode, errorObject.ExceptionCode);

        #endregion
    }

    [Theory]
    [InlineData("", "Admin@123", "AppyNox", "NotEmptyValidator", HttpStatusCode.UnprocessableContent)]
    [InlineData("admin", "", "AppyNox", "NotEmptyValidator", HttpStatusCode.UnprocessableContent)]
    [InlineData("admin", "Admin@123", "", "NotEmptyValidator", HttpStatusCode.UnprocessableContent)]
    [Order(2)]
    public async Task ConnectToken_ShouldReturnCorrectValidationResponse(string username, string password, string audience, string validationCode, HttpStatusCode statusCode)
    {
        #region [ Get Token ]

        // Act
        var content = new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        username,
                        password,
                        audience
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );
        string uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/connect/token";
        HttpResponseMessage response = await _client.PostAsync(uri, content);

        NoxApiResponse<NoxApiValidationExceptionWrapObjectPOCO> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<NoxApiValidationExceptionWrapObjectPOCO>(response, jsonSerializerOptions: _jsonSerializerOptions);
        NoxApiValidationExceptionWrapObjectPOCO? errorObject = (wrappedResponse.Result.Error as NoxApiValidationExceptionWrapObjectPOCO);

        // Assert
        Assert.Equal(statusCode, response.StatusCode);
        Assert.True(wrappedResponse.HasError);
        Assert.NotNull(errorObject);
        Assert.Null(errorObject.InnerException);
        Assert.Equal((int)NoxApplicationExceptionCode.FluentValidationError, errorObject.ExceptionCode);
        Assert.NotNull(errorObject.ValidationErrors);
        Assert.Single(errorObject.ValidationErrors);
        Assert.Equal(validationCode, errorObject.ValidationErrors.First().ErrorCode);

        #endregion
    }

    [Theory]
    [InlineData("admin", "Admin@123", "AppyNox")]
    [InlineData("admin", "Admin@123", "AppyFleet")]
    [Order(3)]
    public async Task RefreshToken_ShouldReturnToken(string username, string password, string audience)
    {
        #region [ Get Token ]

        // Act
        var content = new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        username,
                        password,
                        audience
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );
        string uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/connect/token";
        HttpResponseMessage response = await _client.PostAsync(uri, content);

        NoxApiResponse<LoginResultDto> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<LoginResultDto>(response, jsonSerializerOptions: _jsonSerializerOptions);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(wrappedResponse.Result);
        Assert.False(wrappedResponse.HasError);

        Assert.NotNull(wrappedResponse.Result.Data);
        string? token = wrappedResponse.Result.Data.Token;
        string? refreshToken = wrappedResponse.Result.Data.RefreshToken;
        Assert.False(string.IsNullOrEmpty(token));
        Assert.False(string.IsNullOrEmpty(refreshToken));

        #endregion

        Thread.Sleep(1000); // waiting to change exp of the token

        #region [ Refresh Token ]

        // Act
        uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/refresh";

        content = new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        token,
                        refreshToken,
                        audience
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );

        HttpResponseMessage refreshResponse = await _client.PostAsync(uri, content);
        NoxApiResponse<LoginResultDto> refreshWrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<LoginResultDto>(refreshResponse, jsonSerializerOptions: _jsonSerializerOptions);

        // Assert
        refreshResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, refreshResponse.StatusCode);
        Assert.NotNull(refreshWrappedResponse.Result);
        Assert.False(refreshWrappedResponse.HasError);

        Assert.NotNull(refreshWrappedResponse.Result.Data);
        string? newToken = refreshWrappedResponse.Result.Data.Token;
        string? newRefreshToken = refreshWrappedResponse.Result.Data.RefreshToken;

        Assert.False(string.IsNullOrEmpty(newToken));
        Assert.False(string.IsNullOrEmpty(newRefreshToken));
        Assert.NotEqual(token, newToken);
        Assert.NotEqual(refreshToken, newRefreshToken);

        #endregion
    }

    [Theory]
    [InlineData(true, false, "AppyNox", (int)NoxSsoApiExceptionCode.RefreshTokenInvalid, HttpStatusCode.Unauthorized)]
    [InlineData(false, false, "AppyNox", (int)NoxSsoInfrastructureExceptionCode.AuthenticationInvalidToken, HttpStatusCode.Unauthorized)]
    [InlineData(false, true, "AppyNox", (int)NoxSsoInfrastructureExceptionCode.AuthenticationInvalidToken, HttpStatusCode.Unauthorized)]
    [InlineData(true, false, "AppyFleet", (int)NoxSsoApiExceptionCode.RefreshTokenInvalid, HttpStatusCode.Unauthorized)]
    [InlineData(false, false, "AppyFleet", (int)NoxSsoInfrastructureExceptionCode.AuthenticationInvalidToken, HttpStatusCode.Unauthorized)]
    [InlineData(false, true, "AppyFleet", (int)NoxSsoInfrastructureExceptionCode.AuthenticationInvalidToken, HttpStatusCode.Unauthorized)]
    [InlineData(false, false, "NoAudience", (int)NoxSsoInfrastructureExceptionCode.InvalidAudience, HttpStatusCode.BadRequest)] // no need to use verified tokens, audience is confirmed first
    [Order(4)]
    public async Task RefreshToken_ShouldReturnCorrectErrorResponse(bool useVerifiedToken, bool useVerifiedRefreshToken, string audience, int exceptionCode, HttpStatusCode httpStatusCode)
    {
        #region [ Get Token ]

        string token = "dummyToken";
        string refreshToken = "dummyRefreshToken";

        if (useVerifiedToken || useVerifiedRefreshToken)
        {
            var loginContent = new StringContent(
               JsonSerializer.Serialize(
                   new
                   {
                       username = "admin",
                       password = "Admin@123",
                       audience
                   }
               ),
               Encoding.UTF8,
               "application/json"
           );
            string loginUri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/connect/token";
            HttpResponseMessage response = await _client.PostAsync(loginUri, loginContent);

            NoxApiResponse<LoginResultDto> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<LoginResultDto>(response, jsonSerializerOptions: _jsonSerializerOptions);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(wrappedResponse.Result);
            Assert.False(wrappedResponse.HasError);

            Assert.NotNull(wrappedResponse.Result.Data);

            token = useVerifiedToken ? wrappedResponse.Result.Data.Token ?? throw new Exception("token was null") : "dummyToken";
            refreshToken = useVerifiedRefreshToken ? wrappedResponse.Result.Data.RefreshToken ?? throw new Exception("refresh token was null") : "dummyRefreshToken";

            Assert.NotNull(token);
            Assert.NotNull(refreshToken);
        }

        #endregion

        #region [ Refresh Token ]

        // Act
        string uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/refresh";

        var content = new StringContent(
                JsonSerializer.Serialize(
                    new
                    {
                        token,
                        refreshToken,
                        audience
                    }
                ),
                Encoding.UTF8,
                "application/json"
            );

        HttpResponseMessage refreshResponse = await _client.PostAsync(uri, content);
        NoxApiResponse<LoginResultDto> refreshWrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<LoginResultDto>(refreshResponse, jsonSerializerOptions: _jsonSerializerOptions);
        NoxApiExceptionWrapObjectPOCO? wrapObjectPOCO = refreshWrappedResponse.Result.Error as NoxApiExceptionWrapObjectPOCO;

        // Assert
        Assert.NotNull(wrapObjectPOCO);
        Assert.Equal(httpStatusCode, refreshResponse.StatusCode);
        Assert.NotNull(refreshWrappedResponse.Result);
        Assert.True(refreshWrappedResponse.HasError);
        Assert.Equal(wrapObjectPOCO.ExceptionCode, exceptionCode);

        #endregion
    }

    [Theory]
    [InlineData("", "test", "test", "NotEmptyValidator", HttpStatusCode.UnprocessableContent)]
    [InlineData("test", "", "test", "NotEmptyValidator", HttpStatusCode.UnprocessableContent)]
    [InlineData("test", "test", "", "NotEmptyValidator", HttpStatusCode.UnprocessableContent)]
    [Order(2)]
    public async Task RefreshToken_ShouldReturnCorrectValidationResponse(string token, string refreshToken, string audience, string validationCode, HttpStatusCode statusCode)
    {
        #region [ Refresh Token ]

        RefreshTokenDto refreshTokenDto = new() { Token = token, RefreshToken = refreshToken, Audience = audience };

        // Act
        var content = new StringContent(
                JsonSerializer.Serialize(refreshTokenDto),
                Encoding.UTF8,
                "application/json"
            );
        string uri = $"{_serviceURIs.SsoServiceURI}/v{NoxVersions.v1_0}/authentication/refresh";
        HttpResponseMessage response = await _client.PostAsync(uri, content);

        NoxApiResponse<NoxApiValidationExceptionWrapObjectPOCO> wrappedResponse = await NoxResponseUnwrapper.UnwrapResponse<NoxApiValidationExceptionWrapObjectPOCO>(response, jsonSerializerOptions: _jsonSerializerOptions);
        NoxApiValidationExceptionWrapObjectPOCO? errorObject = (wrappedResponse.Result.Error as NoxApiValidationExceptionWrapObjectPOCO);

        // Assert
        Assert.Equal(statusCode, response.StatusCode);
        Assert.True(wrappedResponse.HasError);
        Assert.NotNull(errorObject);
        Assert.Null(errorObject.InnerException);
        Assert.Equal((int)NoxApplicationExceptionCode.FluentValidationError, errorObject.ExceptionCode);
        Assert.NotNull(errorObject.ValidationErrors);
        Assert.Single(errorObject.ValidationErrors);
        Assert.Equal(validationCode, errorObject.ValidationErrors.First().ErrorCode);

        #endregion
    }

    #endregion
}