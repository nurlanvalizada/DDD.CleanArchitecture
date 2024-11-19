using Application.Common.Interfaces;

namespace Infrastructure.Services;

public class TestCurrentUserService : ICurrentUserService
{
    public long? UserId { get; } = 1;
}