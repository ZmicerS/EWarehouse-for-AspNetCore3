using EWarehouse.Services.Entities.AccountModels;
using EWarehouse.Services.Entities.TokenModels;

namespace EWarehouse.Services.Contracts
{
    public interface IJwtTokenService
    {
        JwtTokenModel CreateJwtToken(LoginServiceModel model);
    }
}
