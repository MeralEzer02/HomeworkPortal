namespace HomeworkPortal.UI.Services
{
    public interface ITokenParserService
    {
        string UserId { get; }
        string Email { get; }
        string Role { get; }
        bool IsAuthenticated { get; }
    }
}