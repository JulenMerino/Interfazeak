using System;
using Microsoft.Maui.Controls;

namespace Ordezkaritza
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnMapTapped(object sender, EventArgs e)
        {
            var mapaUrl = "https://www.google.es/maps/place/Tolosaldea+Lanbide+Heziketako+Ikastetxe+Integratua/@43.1478842,-2.0737755,16z/data=!4m6!3m5!1s0xd504b6900588037:0xbaa343d5f58fb872!8m2!3d43.1489905!4d-2.0681961!16s%2Fg%2F1z44bdkm5?hl=es&entry=ttu&g_ep=EgoyMDI1MDEyMS4wIKXMDSoASAFQAw%3D%3D";
            await Launcher.OpenAsync(new Uri(mapaUrl));
        }

        private async void OnEmailTapped(object sender, EventArgs e)
        {
            var emailAddress = "Correoa@prmt.com";
            var mailtoUri = new Uri($"mailto:{emailAddress}");
            await Launcher.OpenAsync(mailtoUri);
        }

        private async void btnHasi_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Informazioa());
        }
    }
}
