using Microsoft.Maui.Controls;

namespace ARMzalogApp.Views.ZavkrPages;

public partial class AnketaPage : ContentPage
{
    private bool _isInitialized = false;

    public AnketaPage()
	{
		InitializeComponent();
        SetupEventHandlers();
        BindingContext = this;
    }
    private void SetupEventHandlers()
    {
        Q1Yes.CheckedChanged += OnQ1Changed;
        Q1No.CheckedChanged += OnQ1Changed;
    }

    private void OnQ1Changed(object sender, CheckedChangedEventArgs e)
    {
        bool isYesSelected = Q1Yes?.IsChecked == true;
        bool isNoSelected = Q1No?.IsChecked == true;

        // Вопросы 2-5: ПОКАЗЫВАТЬ только когда выбран "Нет"
        if (FrameQ2 != null)
            FrameQ2.IsVisible = isNoSelected;
        if (FrameQ3 != null)
            FrameQ3.IsVisible = isNoSelected;
        if (FrameQ4 != null)
            FrameQ4.IsVisible = isNoSelected;
        if (FrameQ5 != null)
            FrameQ5.IsVisible = isNoSelected;

        // Вопросы 7–12: ПОКАЗЫВАТЬ только когда выбран "Да"
        if (FrameQ7 != null)
            FrameQ7.IsVisible = isYesSelected;
        if (FrameQ8 != null)
            FrameQ8.IsVisible = isYesSelected;
        if (FrameQ9 != null)
            FrameQ9.IsVisible = isYesSelected;
        if (FrameQ10 != null)
            FrameQ10.IsVisible = isYesSelected;
        if (FrameQ11 != null)
            FrameQ11.IsVisible = isYesSelected;
        if (FrameQ12 != null)
            FrameQ12.IsVisible = isYesSelected;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("Подтверждение", "Вы уверены, что хотите вернуться? Несохранённые данные будут потеряны.", "Да", "Нет");
        if (result)
        {
            await Navigation.PopAsync();
        }
    }

}