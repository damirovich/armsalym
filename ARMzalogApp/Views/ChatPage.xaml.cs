namespace ARMzalogApp.Views;

public partial class ChatPage : ContentPage // использовать для чата
{
    private readonly HttpClient _httpClient;

    public ChatPage()
    {
        InitializeComponent();
        _httpClient = new HttpClient();
    }

    private async void OnSendButtonClicked(object sender, EventArgs e)
    {
        var userInput = UserInput.Text;

        if (!string.IsNullOrEmpty(userInput))
        {
            var response = ""; // FAQ_GPT.GetAnswer(userInput);
            ChatOutput.Text += $"\n\nВопрос: {userInput}\nОтвет: {response}";
            UserInput.Text = string.Empty; // Очистить поле ввода после отправки
        }
    }
}