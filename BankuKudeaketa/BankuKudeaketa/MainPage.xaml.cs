//Nork sortua: Hegoi Ruiz
//Noiz sortua:
//
//1.Aldaketa
//Nork aldatua: Julen Merino
//Noiz aldatua: 11/12/2024

//Leiho berri bat ireki beharrean, leiho berean beste leiho baterako trantsizioa egin dut, eta itzultzeko aukera du.
//Mugikorrean ongi ikusteko aldaketak 

using System.Diagnostics;
using System.Windows.Input;
using BankuKudeaketa.Modeloak;
using BankuKudeaketa.Views;

namespace BankuKudeaketa
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BotoienTmaina();

        }

        /// <summary>
        /// Bai mugikorrean bai ordenagailuan ongi ikusteko metodoa
        /// </summary>
        private void BotoienTmaina()
        {
            if (DeviceInfo.Platform == DevicePlatform.iOS || DeviceInfo.Platform == DevicePlatform.Android)
            {
                ButtonBezeroak.HeightRequest = 60; 
                ButtonKontuak.HeightRequest = 60;  
                ButtonIrten.HeightRequest = 60;    
            }
            else
            {
                ButtonBezeroak.HeightRequest = 80; 
                ButtonKontuak.HeightRequest = 80;  
                ButtonIrten.HeightRequest = 80;    
            }
        }

        /// <summary>
        /// Bezeroen pantaila irekitzeko metodoa
        /// </summary>
        private async void ButtonBezeroak_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BezeroakView());
        }

        /// <summary>
        /// Kontuen pantaila irekitzeko metodoa
        /// </summary>
        private async void ButtonKontuak_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new KontuakView());
        }

        /// <summary>
        /// Aplikazioatik irteteko metodoa
        /// </summary>
        private void ButtonIrten_Clicked(object sender, EventArgs e)
        {
            Application.Current?.Quit();
        }
    }

}
