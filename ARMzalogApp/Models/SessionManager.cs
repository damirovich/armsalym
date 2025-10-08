using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMzalogApp.Models
{
    public static class SessionManager
    {
        public static (string Username, string OtNom, string Fio, string Otdel, string Pin) GetSession()
        {
            return Task.Run(async () => await GetSessionAsync()).GetAwaiter().GetResult();
        }

        public static async Task<(string Username, string OtNom, string Fio, string Otdel, string Pin)> GetSessionAsync()
        {
            var username = await SecureStorage.GetAsync("username");
            var otNom = await SecureStorage.GetAsync("otNom");
            var fio = await SecureStorage.GetAsync("fio");
            var otdel = await SecureStorage.GetAsync("otdel");
            var pin = await SecureStorage.GetAsync("pin");
            return (username, otNom, fio, otdel, pin);
        }
        public static async Task ClearSessionAsync()
        {
            SecureStorage.Remove("username");
            SecureStorage.Remove("otNom");
            SecureStorage.Remove("fio");
            SecureStorage.Remove("otdel");
            SecureStorage.Remove("pin");
        }
// Асинхронный метод для получения otNom-кода
        public static async Task<string> GetOtNomAsync()
        {
            return await SecureStorage.Default.GetAsync("otNom");
        }

        // Синхронный метод для получения otNom-кода
        public static string GetoOtNom()
        {
            return Task.Run(async () => await GetOtNomAsync()).GetAwaiter().GetResult();
        }

  // Асинхронный метод для получения fio-кода
        public static async Task<string> GetFioAsync()
        {
            return await SecureStorage.Default.GetAsync("fio");
        }

        // Синхронный метод для получения fio-кода
        public static string GetoFio()
        {
            return Task.Run(async () => await GetFioAsync()).GetAwaiter().GetResult();
        }

    }

}
