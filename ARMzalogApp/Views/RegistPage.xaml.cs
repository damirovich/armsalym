using ARMzalogApp.Views;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using ARMzalogApp.Sevices;

namespace ARMzalogApp.Views;

public partial class RegistPage : ContentPage
{
	public RegistPage()
	{
		InitializeComponent();
	}
    private async void OnButtonSendClicked(object sender, EventArgs e)
    {
        string lastName = LastNameEntry.Text;
        string firstName = FirstNameEntry.Text;
        string phoneNumber = PhoneNumberEntry.Text;
        DateTime birthDate = BirthDatePicker.Date;
        if (string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(phoneNumber))
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Заполните все поля ", "Ok");
            return;
        }
        var service = new LoginService();
        string result = await service.Registration(lastName, firstName, phoneNumber.Trim(), birthDate);
        if (result == "OK")
        {
            await Application.Current.MainPage.DisplayAlert("Успешно", "Заявка на регистрацию отправлена. Подойти в офис Компании Салым Финанс для получения логина и пароля", "OK");
        }
        else if (result == "not found")
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка", "Неправильный номер телефона. Подойти в офис Компании Салым Финанс для получения дополнительной информации", "OK");
        }
    }
}