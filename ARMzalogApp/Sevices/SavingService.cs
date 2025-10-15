using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using Newtonsoft.Json;
using System.Text;

namespace ARMzalogApp.Sevices
{
    public class SavingService : ISavingRepository
    {
        public async Task<string> SaveAbsFile(string clientInn, string ZvPozn, string latitude, string longitude, byte[] photoData, string type, string IdZalog, int otNom, string token)
        {
            //try
            //{
                var client = new HttpClient();
                string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/SavePhoto";
                //string url = "http://localhost:5145/" + "api/LoanReference/SavePhoto";
                client.BaseAddress = new Uri(url);
                var loginRequest = new SavingRequest
                {
                    clientInn = clientInn,
                    ZvPozn = ZvPozn,
                    latitude = latitude,
                    longitude = longitude,
                    PhotoData = photoData,
                    Type = type,
                    IdZalog = IdZalog,
                    otNom = otNom,
                    token = token
                };

                var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(ServerConstants.SERVER_ROOT_URL + "api/LoanReference/SavePhoto", content);
                //var response = await client.PostAsync("http://localhost:5145/" + "api/LoanReference/SavePhoto", content);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    result = "OK";
                    //await Application.Current.MainPage.DisplayAlert("Успех", "Успешно сохранено", "OK");
                    return result;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка при сохранении", "OK");
                    return null;
                }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return null;
            //}
        }

        public async Task<string> SaveNewZalog(int vidZal, string pozn,string otNom)
        {
            var client = new HttpClient();
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/SaveNewVidZalog";
            client.BaseAddress = new Uri(url);
            var loginRequest = new NewZalogRequest
            {
                VidZalog = vidZal,
                Pozn = pozn,
                OtNom = otNom
            };

            var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(ServerConstants.SERVER_ROOT_URL + "api/LoanReference/SaveNewVidZalog", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                result = "OK";
                return result;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка при сохранении", "OK");
                return null;
            }
        }

        public async Task<string> SaveNewLoan(string otNom, ZmainView newLoan)
        {
            var client = new HttpClient();
            string url = ServerConstants.SERVER_ROOT_URL + $"api/LoanReference/CreateLoanApplication?employeeNum={otNom}";
            client.BaseAddress = new Uri(url);


            var content = new StringContent(JsonConvert.SerializeObject(newLoan), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                //result = "OK";
                return result;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Ошибка", "Ошибка при сохранении", "OK");
                return null;
            }
        }


    }
}
