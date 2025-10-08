using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ARMzalogApp.Views;

public partial class Zalog : ContentPage
{
    private Zavkr _selectedZavkr;
    public ICommand FrameTappedCommand { get; }

    public ObservableCollection<Zalogi> ZalogiList { get; set; }
    public Zalog(Zavkr selectedZavkr)
    {
        InitializeComponent();
        _selectedZavkr = selectedZavkr;
        ZalogiList = new ObservableCollection<Zalogi>();
        BindingContext = this;
        LoadAllDataZalog();

    }
    private async void LoadAllDataZalog()
    {
        try
        {
            string DgPozn = _selectedZavkr.PositionalNumber.ToString();

            using var httpClient = new HttpClient();

            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetReferencesOfZalogi?dgPozn=" + DgPozn;

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();

                List<Zalogi> dataList = JsonConvert.DeserializeObject<List<Zalogi>>(responseData);
                //BindingContext = dataList;
                if(dataList.Count() == 0)
                {
                    emptyZalog.Text = "Залоги отсутствуют";
                }
                else
                {
                    emptyZalog.Text = "";
                }
                    foreach (var item in dataList)
                    {
                        ZalogiList.Add(item);
                    }
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }
    }

    private async void OnFrameTapped(object sender, EventArgs e)
    {
        var selectedZavkr = ((sender as StackLayout)?.BindingContext as Zalogi);
        if (selectedZavkr != null)
        {
            await Navigation.PushAsync(new ZalogPhoto(selectedZavkr));
        }
    }

    private async void OnCreateZalogClicked(object sender, EventArgs e)
    {
       await Navigation.PushAsync(new CreateZalogPage(_selectedZavkr));
    }
}