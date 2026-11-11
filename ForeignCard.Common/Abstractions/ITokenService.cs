namespace ForeignCard.Common.Abstractions
{
    public interface ITokenService
    {
        string CreateToken(int customerId, string fullName, string role);
    }
}
