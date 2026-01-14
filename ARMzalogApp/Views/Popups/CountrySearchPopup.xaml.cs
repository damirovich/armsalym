
using ARMzalogApp.Models.Responses;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace ARMzalogApp.Views.Popups;

public partial class CountrySearchPopup : Popup
{
    private readonly List<CountryCode> _allCountries;
    public ObservableCollection<CountryCode> FilteredCountries { get; set; }

    public CountrySearchPopup(IEnumerable<CountryCode> countries)
    {
        InitializeComponent();

        _allCountries = countries?.ToList() ?? new List<CountryCode>();
        FilteredCountries = new ObservableCollection<CountryCode>(_allCountries);

        // БЕЗ ЭТОЙ СТРОКИ СПИСОК БУДЕТ ПУСТЫМ:
        BindingContext = this;
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string searchTerm = e.NewTextValue?.ToLower() ?? "";

        // Фильтруем список
        var filtered = _allCountries
            .Where(c => c.Name.ToLower().Contains(searchTerm))
            .ToList();

        // Обновляем коллекцию для UI
        FilteredCountries.Clear();
        foreach (var country in filtered)
        {
            FilteredCountries.Add(country);
        }
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is CountryCode selectedCountry)
        {
            Close(selectedCountry); // Возвращаем выбор
        }
    }

    private void OnCloseClicked(object sender, EventArgs e) => Close(null);
}