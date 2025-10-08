using ARMzalogApp.Models;
using Newtonsoft.Json;
using ARMzalogApp.Constants;
using System.Text;

namespace ARMzalogApp.Sevices
{
    public class LoginService : ILoginRepository
    {
        public async Task<User> Login(string username, string password)
        {
            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var client = new HttpClient();
                    string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/SignIn";
                    client.BaseAddress = new Uri(url);
                    var loginRequest = new ILoginRequest
                    {
                        KlLogin = username,
                        KlPassword = password,
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(ServerConstants.SERVER_ROOT_URL + "api/LoanReference/SignIn", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        //var user = System.Text.Json.JsonSerializer.Deserialize<User>(result);
                        var user = JsonConvert.DeserializeObject<User>(result);
                        //if (user.DepartmentId != 0 && user.DepartmentId != null && user.UserNumber != null) // не используется, вместо него Session
                        //{
                        //    UserData.SetUserData(user);
                        //    return user;
                        //}
                        //else
                        //{
                        //    return null;
                        //}
                        return user;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<string> Registration(string lastName, string firstName, string phoneNumber, DateTime birthDate)
        {
            try
            {
                if (Connectivity.Current.NetworkAccess == NetworkAccess.Internet)
                {
                    var client = new HttpClient();
                    string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/RegistrationUsers";
                    client.BaseAddress = new Uri(url);
                    var regRequest = new IRegistrRequest
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        PhoneNumber = phoneNumber,
                        BirthDate = birthDate.ToString(),
                    };

                    var content = new StringContent(JsonConvert.SerializeObject(regRequest), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(ServerConstants.SERVER_ROOT_URL + "api/LoanReference/RegistrationUsers", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = "OK";
                        return result;

                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<string> LoginLog(string login, string fio, string otdel, string uniq, string result1)
        {
            var client = new HttpClient();
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/UserSalymLog";
            client.BaseAddress = new Uri(url);
            var loginRequest = new UserSalymArm
            {
                Otdel = otdel,
                OtUid = login,
                Fio = fio,
                Uniq = uniq,
                Result = result1
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ServerConstants.SERVER_ROOT_URL + "api/LoanReference/UserSalymLog", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                result = "OK";
                return result;
            }
            else
            {
                //await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка при сохранении", "OK");
                return null;
            }
        }

    }
}
