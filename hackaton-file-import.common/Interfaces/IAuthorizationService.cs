namespace hackaton_file_import.common.Interfaces
{
    public interface IAuthorizationService
    {
        Task<bool> VerifyToken(string token, string[] roles);
    }
}
