using ARMzalogApp.Constants;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARMzalogApp.Models;

namespace ARMzalogApp.Sevices
{
    public class AccountRepository : IAccountRepository
    {
        public async Task<string> UpdateAccountStatus(string otNom, int status, string ot_uid)
        {
            //try
            //{
            var client = new HttpClient();
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/UpdateUserStatus";
            client.BaseAddress = new Uri(url);
            var statusRequest = new StatusUser
            {
                OtNom = otNom,
                Status = status,
                Login = ot_uid
            };

            var content = new StringContent(JsonConvert.SerializeObject(statusRequest), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ServerConstants.SERVER_ROOT_URL + "api/LoanReference/UpdateUserStatus", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                result = "OK";
                return result;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Произошла непредвиденная ошибка ", "OK");
                return null;
            }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return null;
            //}
        }

        public async Task<int> GetAccountStatus(string userName)
        {
            int result = 0;
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetUserStatus?userName=" + userName;

            using var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<int>(responseData);
                result = data;
            }
            return result;
        }

    }
}
