using TO_DO.SERVİCE.Dtos;

namespace TO_DO.SERVİCE.Contracts
{
    public interface ITokenService
    {
        string GetToken(UserDto userDto);
    }
}