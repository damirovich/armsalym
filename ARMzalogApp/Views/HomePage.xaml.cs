using ARMzalogApp.Models;
using Newtonsoft.Json;
using ARMzalogApp.Views;
using System.Windows.Input;
using ARMzalogApp.Constants;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ARMzalogApp.Views;

public partial class HomePage : ContentPage, INotifyPropertyChanged
{
    public ICommand FrameTappedCommand { get; }
    private ObservableCollection<Zavkr> _zavkrList;
    private bool _isPasswordVisible = true;
    public ObservableCollection<Zavkr> ZavkrList
    {
        get => _zavkrList;
        set
        {
            _zavkrList = value;
            OnPropertyChanged();
        }
    }

    private long _savedNumber;
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

        // Загружаем данные только если не загружаем сейчас
        if (!_isLoading)
        {
            LoadData();
        }
    }

    protected override void OnDisappearing()
    {
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = null;
        base.OnDisappearing();
    }

    public HomePage(User user)
    {
        InitializeComponent();
        ZavkrList = new ObservableCollection<Zavkr>();
        BindingContext = this;
    }

    private async void LoadData()
    {
        if (_isLoading || _cancellationTokenSource == null || _cancellationTokenSource.Token.IsCancellationRequested)
            return;

        _isLoading = true;
        try
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            activityIndicator.IsEnabled = true;
            string departmentId = await SecureStorage.Default.GetAsync("otdel");
            string ot_nom = "1049";
            //string departmentId = "6";
            using var httpClient = new HttpClient();
            //string ot_nom = UserData.CurrentUser.UserNumber.ToString();
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetReferencesOfDepartment?departmentId=" + departmentId;
            //string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetReferencesOfEmployee?otNom=" + ot_nom;
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            HttpResponseMessage response = await httpClient.GetAsync(url, _cancellationTokenSource.Token);
            _cancellationTokenSource.Token.ThrowIfCancellationRequested(); // Проверяем, не была ли отменена операция

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                ZavkrList.Clear();
                var dataList = JsonConvert.DeserializeObject<List<Zavkr>>(responseData);
                foreach (var item in dataList.Take(10).OrderByDescending(s => s.PositionalNumber))
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        break;
                    ZavkrList.Add(item);
                }

                OnPropertyChanged(nameof(ZavkrList));
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                await DisplayAlert("Ошибка", $"Ошибка сервера: {response.StatusCode}", "OK");
            }
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            activityIndicator.IsEnabled = false;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            await DisplayAlert("Ошибка", $"Exception: {ex.Message}", "OK");
        }
    }

    private async void OnFrameTapped(object sender, EventArgs e)
    {
        var selectedZavkr = ((sender as StackLayout)?.BindingContext as Zavkr);
        if (selectedZavkr != null)
        {
            await Navigation.PushAsync(new ZavkrPage(selectedZavkr));
        }
    }

    private void OnSaveNumberClicked(object sender, EventArgs e)
    {
        if (long.TryParse(numberEntry.Text, out long number))
        {
            _savedNumber = number;
            GetDataFromFilter(_savedNumber);
        }
        else
        {
            DisplayAlert("Ошибка", "Введите числа!", "OK");
        }
    }

    private async void GetDataFromFilter(long pozn) // 1126321645
    {
        try
        {
            string departmentId = await SecureStorage.Default.GetAsync("otdel");
            // string departmentId = UserData.CurrentUser.DepartmentId.ToString();
            using var httpClient = new HttpClient();
            /*"http://localhost:5145/"*/
            string url = ServerConstants.SERVER_ROOT_URL + "api/LoanReference/GetOneZalog?pozn=" + pozn+"/"+departmentId;

            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                ZavkrList.Clear();

                var dataList = JsonConvert.DeserializeObject<List<Zavkr>>(responseData);
                //BindingContext = dataList;

                //var top3Data = dataList.Take(2).ToList();
                //BindingContext = top3Data;
                foreach (var item in dataList)
                {
                    ZavkrList.Add(item);

                }
                if (dataList.Count > 0)
                {
                    DisplayAlert("Успешно", "Заявка найдена", "OK");
                }
                OnPropertyChanged(nameof(ZavkrList));

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
    private void ClearFilter(object sender, EventArgs e)
    {
        LoadData();
        numberEntry.Text = string.Empty;
        ZavkrList.Clear();
    }
    private void HideProperty(object sender, EventArgs e)
    {
        if (ZavkrList.Count > 0)
        {
            bool hide = ZavkrList.All(z => !z.IsSecretHidden);
            foreach (var zavkr in ZavkrList)
            {
                zavkr.IsSecretHidden = hide;
            }
            _isPasswordVisible = !_isPasswordVisible;
            UpdatePasswordVisibility();
        }
    }
    private void UpdatePasswordVisibility()
    {
        if (_isPasswordVisible)
        {
            showPasswordIcon.Source = "hide_icon.png";
        }
        else
        {
            showPasswordIcon.Source = "show_icon.png";
        }
    }

    #region INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion

    //protected async override void OnNavigatedTo(NavigatedToEventArgs args) // используется после полной загрузки UI
    //{
    //    base.OnNavigatedTo(args);

    //}

}