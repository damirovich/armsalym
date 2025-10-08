using ARMzalogApp.Constants;
using ARMzalogApp.Models;
using ARMzalogApp.Sevices;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
namespace ARMzalogApp.Views;

public partial class ListPoruch : ContentPage
{
    private Zavkr _selectedZavkr;
    public ObservableCollection<PoruchZavkr> allPoruch { get; set; }
    public ListPoruch(Zavkr selectedZavkr)
	{
		InitializeComponent();
        _selectedZavkr = selectedZavkr;
        allPoruch = new ObservableCollection<PoruchZavkr>();
        BindingContext = this;
        LoadPochData();
    }

	private async Task LoadPochData()
	{
		string pozn = _selectedZavkr.PositionalNumber.ToString();
        using var httpClient = new HttpClient();
        string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetReferencesOfPoruch?pozn=" + pozn;
        HttpResponseMessage response = await httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            string responseData = await response.Content.ReadAsStringAsync();
            var dataList = JsonConvert.DeserializeObject<List<PoruchZavkr>>(responseData);
            //BindingContext = dataList;
            allPoruch.Clear();
            foreach (var item in dataList)
            {
                allPoruch.Add(item);
            }

        }
    }

    private async void OnFrameTapped(object sender, EventArgs e)
    {
        var selectedZavkr = ((sender as StackLayout)?.BindingContext as PoruchZavkr);
        if (selectedZavkr != null)
        {
            await Navigation.PushAsync(new PoruchPhoto(selectedZavkr));
        }
    }

}