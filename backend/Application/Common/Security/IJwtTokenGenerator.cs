using Shared.Responses;
using Shared.DTOs;
using Domain.Entities;

namespace Application.Common.Security;

public interface IJwtTokenGenerator
{
    AuthResultDto GenerateToken(User user);
}
