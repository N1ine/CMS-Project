using Application.DTOs.Responses;
using Domain.Entities;

namespace Application.Common.Security;
public interface IJwtTokenGenerator
{
    AuthResultDto GenerateToken(User user);
}