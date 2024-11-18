using Microsoft.AspNetCore.Http;
using Web.Services;
using Xunit;

namespace IntegrationTests;

public class CurrentUserServiceTests
{
    [Fact]
    public void CurrentUserService_Should_Set_UserId_From_Header()
    {
        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };

        var userId = 1998;
        httpContextAccessor.HttpContext.Request.Headers["X-UserId"] = userId.ToString();

        var currentUserService = new CurrentUserService(httpContextAccessor);

        Assert.NotNull(currentUserService.UserId);
        Assert.Equal(userId, currentUserService.UserId);
    }

    [Fact]
    public void CurrentUserService_Should_Not_Set_UserId_If_Header_Missing()
    {
        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };

        var currentUserService = new CurrentUserService(httpContextAccessor);

        Assert.Null(currentUserService.UserId);
    }

    [Fact]
    public void CurrentUserService_Should_Not_Set_UserId_If_Header_Invalid()
    {
        var httpContextAccessor = new HttpContextAccessor
        {
            HttpContext = new DefaultHttpContext()
        };

        httpContextAccessor.HttpContext.Request.Headers["X-UserId"] = "invalid";

        var currentUserService = new CurrentUserService(httpContextAccessor);

        Assert.Null(currentUserService.UserId);
    }
}