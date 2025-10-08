using ARMzalogApp.Models.Responses;
using Newtonsoft.Json;
using System.Windows.Input;
using ARMzalogApp.Constants;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ARMzalogApp.Views.DogkrPages;

public partial class ListDogkr : ContentPage, INotifyPropertyChanged
{
    public ICommand FrameTappedCommand { get; }
    private ObservableCollection<DogkrResponse> _zavkrList;
    private bool _isPasswordVisible = true;
    public ObservableCollection<DogkrResponse> DogkrList
    {
        get => _zavkrList;
        set
        {
            _zavkrList = value;
            OnPropertyChanged();
        }
    }

    private string _savedNumber;
    private CancellationTokenSource _cancellationTokenSource;
    private bool _isLoading = false;

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (_cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();
        }
    }

    protected override void OnDisappearing()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        base.OnDisappearing();
    }

    public ListDogkr()
    {
        InitializeComponent();
        DogkrList = new ObservableCollection<DogkrResponse>();
        BindingContext = this;
    }

    private async void OnFrameTapped(object sender, EventArgs e)
    {
        var selectedZavkr = ((sender as StackLayout)?.BindingContext as DogkrResponse);
        if (selectedZavkr != null)
        {
            //await Shell.Current.GoToAsync("ViewKredDog");
            await Navigation.PushAsync(new ViewKredDog(selectedZavkr));
        }
    }

    private void OnSaveNumberClicked(object sender, EventArgs e)
    {
        //if (long.TryParse(numberEntry.Text, out long number))
        //{
        //    _savedNumber = number;
        //}
        //else
        //{
        //    DisplayAlert("Ошибка", "Введите числа!", "OK");
        //}
        _savedNumber = numberEntry.Text;
        GetDataFromFilter(_savedNumber.ToString());
    }

    private async void GetDataFromFilter(string pozn) // 1126321645
    {
        try
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            activityIndicator.IsEnabled = true;

            using var httpClient = new HttpClient();
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetLoanByCN?contractName=" + pozn ;

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var dataList = JsonConvert.DeserializeObject<List<DogkrResponse>>(responseData);

                foreach (var item in dataList)
                {
                    DogkrList.Add(item);
                }
                if (dataList.Count > 0)
                {
                    DisplayAlert("Успешно", "Кредитный договор найден", "OK");
                }
                OnPropertyChanged(nameof(DogkrList));

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
        activityIndicator.IsVisible = false;
        activityIndicator.IsRunning = false;
        activityIndicator.IsEnabled = false;
    }
    private void ClearFilter(object sender, EventArgs e)
    {
        numberEntry.Text = string.Empty;
        DogkrList.Clear();
    }
    

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}