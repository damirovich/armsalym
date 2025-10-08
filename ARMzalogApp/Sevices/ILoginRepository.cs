using ARMzalogApp.Models;


namespace ARMzalogApp.Sevices
{
    public interface ILoginRepository
    {
        Task<User> Login(string username, string password);
        Task<string> LoginLog(string login, string fio, string otdel, string uniq, string result);
    }
}
