using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ARMzalogApp.Sevices
{
    public static class PlayStoreVersionChecker
    {
        // Rapid - Google Play Store API - Scrape Apps Data
        public static async Task<string> GetLatestVersionFromPlayStore() 
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://google-play-store-api-scrape-apps-data.p.rapidapi.com/googleplay/details"),
                Headers =
                {
                    { "x-rapidapi-key", "fe52bfc1f4msh49b803a61b3b8b3p15035bjsna8c5a0db5f51" },
                    { "x-rapidapi-host", "google-play-store-api-scrape-apps-data.p.rapidapi.com" },
                },
                Content = new StringContent("{\"package_name\":\"com.salymf.pamobile\"}")
                {
                    Headers =
                    {
                        ContentType = new MediaTypeHeaderValue("application/json")
                    }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(body);
                Console.WriteLine(body);
                var dataArray = (JArray)json["data"]; // Получение массива data

                if (dataArray != null && dataArray.Count > 0)
                {
                    var data = dataArray[0]; // Получение первого элемента массива
                    return data["version"].ToString(); // Извлечение версии из объекта data
                }
                else
                {
                    throw new Exception("No data found in the response.");
                }
            }

        }

        public static bool IsNewVersionAvailable(string currentVersion, string latestVersion)
        {
            var currentParts = currentVersion.Split('.').Select(int.Parse).ToArray();
            var latestParts = latestVersion.Split('.').Select(int.Parse).ToArray();

            int maxLength = Math.Max(currentParts.Length, latestParts.Length);

            for (int i = 0; i < maxLength; i++)
            {
                int currentPart = i < currentParts.Length ? currentParts[i] : 0;
                int latestPart = i < latestParts.Length ? latestParts[i] : 0;

                if (currentPart < latestPart)
                {
                    return true; // Найдена новая версия
                }
                else if (currentPart > latestPart)
                {
                    return false; // Текущая версия новее
                }
            }

            return false; // Версии равны
        }

        public static async Task<string> GetLatestVersionFromDatabase()
        {
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetMobileAppVersion?Id=1";
            string version = "";
            using var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync(); //"[{\"version\":\"1.0.3\",\"opSystem\":\"Android Version\"}]"
                var data = JsonConvert.DeserializeObject<List<ArmAppVersion>>(responseData);
                if (data != null && data.Count > 0)
                {
                    version = data[0].Version.Trim();
                    return version;
                }
                else
                {
                    throw new Exception("No data found in the response.");
                }
            }
            return version;
        }
    }
}
