namespace ProdBase.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<Dictionary<string, object>> VerifyIdTokenAsync(string idToken);
    }

    public class AuthError : Exception
    {
        public AuthError(string message) : base(message)
        {
        }
    }
}
