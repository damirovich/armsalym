using ARMzalogApp.Models;
using ARMzalogApp.Models.Auth;

namespace ARMzalogApp.Sevices.Auth;

public sealed class UserSessionService : IUserSessionService
{
    private User? _user;

    public bool IsAuthenticated => _user != null;
    public int UserId => _user?.UserNumber ?? 0;
    public string UserName => _user?.UserName ?? string.Empty;
    public string FullName => _user?.otFio ?? string.Empty;

    public void SetSession(User user)
    {
        _user = user;
    }

    public void Clear()
    {
        _user = null;
    }
}