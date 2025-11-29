using ARMzalogApp.Models.Auth;

namespace ARMzalogApp.Sevices.Auth;
public interface IUserSessionService
{
    bool IsAuthenticated { get; }
    int UserId { get; }
    string Login { get; }
    string FullName { get; }

    void SetSession(UserInfoDto user);
    void Clear();
}