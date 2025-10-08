using ARMzalogApp.Constants;
using Azure.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Sevices
{
    internal class ContentService
    {

        private static ContentService _instance;

        public static ContentService Instance(string token)
        {
            return _instance;
        }

        public async static Task<IEnumerable<TResponse>> GetItemsAsync<TResponse>(string requestUrl)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                
                httpClient.BaseAddress = new Uri(ServerConstants.SERVER_ROOT_URL);
                httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                //httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _accessToken);

                try
                {
                    HttpResponseMessage response = await httpClient.GetAsync(httpClient.BaseAddress + requestUrl);
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        //await Shell.Current.GoToAsync("PinPage");
                        return null;
                    }
                    var content = await response.Content.ReadAsStringAsync();
                    if (content == null || string.IsNullOrEmpty(content))
                    {

                    }
                    IEnumerable<TResponse> result = JsonConvert.DeserializeObject<IEnumerable<TResponse>>(content);
                    return result;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
