using ARMzalogApp.Constants;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using ARMzalogApp.Models;

namespace ARMzalogApp.Views;

public partial class TotalNumberOfPhoto : ContentPage // показывает количество фотографий на новой странице
{
    private Zavkr _selectedZavkr;
    public TotalNumberOfPhoto(Zavkr selectedZavkr)
	{
		InitializeComponent();
        totalNum.Text = "Загрузка общего количества фотографий ";
        _selectedZavkr = selectedZavkr;
        LoadDataFromService();
    }
    private async void LoadDataFromService()
    {
        string inn = _selectedZavkr.Inn;
        string ZvPozn = _selectedZavkr.PositionalNumber.ToString();
        
        string path = $"{inn}\\ZAVKR\\{ZvPozn}\\ResidencePhoto"; // передеать через сессию ! до открытия страницы
        string typePhoto = await SecureStorage.GetAsync("typePhoto") ?? "";
        
        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetCountOfImage?path=" + path + "&typePhoto=" + typePhoto;

        using var httpClient = new HttpClient();
        HttpResponseMessage response = await httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<int>(responseData);
            if (data >= 0)
            {
                totalNum.Text = "Всего " + data.ToString() + " фотографий";
            }
            else
            {
                totalNum.Text = "не известно";
            }
        }
    }
}