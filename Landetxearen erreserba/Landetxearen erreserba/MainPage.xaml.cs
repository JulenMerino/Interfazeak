using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Landetxearen_erreserba
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> Images { get; set; }
        public ObservableCollection<string> ReservedWeeks { get; set; } // Colección para las semanas reservadas

        public MainPage()
        {
            InitializeComponent();
            InitializeImages();
            ReservedWeeks = new ObservableCollection<string>(); // Inicializa la colección
            BindingContext = this;
        }

        private void InitializeImages()
        {
            Images = new ObservableCollection<string>
            {
                "bat.jpg",
                "bi.jpg",
                "hiru.jpg",
                "lau.jpg"
            };
        }

        public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            decimal total = 0;

            if (ChAireportu.IsChecked)
                total += 25;

            if (ChBisita.IsChecked)
                total += 150;

            if (ChUme.IsChecked)
                total += 40;

            EtyGehigarri.Text = total.ToString();

            decimal estanciaPrecio = 300;
            decimal gehigarriakPrecio = total;
            decimal totalGuztira = estanciaPrecio + gehigarriakPrecio;
            EtyGuztira.Text = totalGuztira.ToString();
        }

        private void MakeReservation()
        {
            // Calcular el inicio de la semana y el rango
            var startOfWeek = BookingDatePicker.Date.StartOfWeek(DayOfWeek.Monday);
            var endOfWeek = startOfWeek.AddDays(6);
            string monthName = GetEuskeraMonthName(startOfWeek.Month); // Obtener el nombre del mes en euskera
            string weekRange = $"{startOfWeek:yyyy}: {monthName} {startOfWeek.Day}-{endOfWeek.Day}";

            if (IsWeekReserved(weekRange))
            {
                DisplayAlert("Errorea", "Aste hau dagoeneko erreserbaturik dago.", "Onartu");
            }
            else
            {
                // Agregar la semana reservada a la colección
                ReservedWeeks.Add(weekRange);
                DisplayAlert("Arrakasta", "Erreserba ongi egin da.", "Onartu");
            }
        }

        private string GetEuskeraMonthName(int month)
        {
            // Diccionario para los nombres de los meses en euskera
            string[] monthsInEuskera = {
                "Urtarrila", // Enero
                "Otsaila",   // Febrero
                "Martxoa",   // Marzo
                "Apirila",   // Abril
                "Maiatza",   // Mayo
                "Ekaina",    // Junio
                "Uztaila",   // Julio
                "Abuztua",   // Agosto
                "Iraila",    // Septiembre
                "Urria",     // Octubre
                "Azaroa",    // Noviembre
                "Abendua"    // Diciembre
            };

            return monthsInEuskera[month - 1]; // Ajustar índice porque los meses empiezan desde 1
        }

        private bool IsWeekReserved(string weekRange)
        {
            return ReservedWeeks.Contains(weekRange);
        }

        private void BookingButton_Clicked(object sender, EventArgs e)
        {
            MakeReservation(); // Llama a la función de reserva cuando se hace clic en el botón
        }
    }

    // Clase de extensión para calcular el inicio de la semana
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
