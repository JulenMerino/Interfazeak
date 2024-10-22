using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Landetxearen_erreserba
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> Irudiak { get; set; } // Irudien bilduma
        public ObservableCollection<string> ErreserbatutakoAsteak { get; set; } // Erreserbatutako asteen bilduma

        public MainPage()
        {
            InitializeComponent(); // Osagaiak hasieratu
            IrudiakHasieratu(); // Irudiak hasieratu
            ErreserbatutakoAsteak = new ObservableCollection<string>(); // Bilduma hasieratu
            BindingContext = this; // Kontextoa lotu
        }

        private void IrudiakHasieratu()
        {
            // Irudiak hasieratu
            Irudiak = new ObservableCollection<string>
            {
                "bat.jpg", // Lehen irudia
                "bi.jpg",  // Bigarren irudia
                "hiru.jpg", // Hirugarren irudia
                "lau.jpg"  // Laugarren irudia
            };
        }

        public void CheckBoxAldatzean(object sender, CheckedChangedEventArgs e)
        {
            // CheckBox bat aldatu denean, guztira eguneratu
            GuztiraEguneratu();
        }

        private void GuztiraEguneratu()
        {
            decimal guztira = 0; // Guztira aldagaia hasieratu

            // CheckBox bakoitzaren egoera egiaztatu eta prezioa gehitu
            if (ChAireportu.IsChecked)
                guztira += 25; // Aireportuko gehigarria

            if (ChBisita.IsChecked)
                guztira += 150; // Bisitaren gehigarria

            if (ChUme.IsChecked)
                guztira += 40; // Ume gehiagarria

            EtyGehigarri.Text = guztira.ToString(); // Gehigarria testu bezala ezarri

            // Estancia eta gehigarriak prezioak kalkulatu
            decimal estanciaPrezioa = 300; // Estancia prezioa
            decimal gehigarriakPrezioa = guztira; // Gehigarri prezioa
            decimal guztiraDut = estanciaPrezioa + gehigarriakPrezioa; // Guztira kalkulatu
            EtyGuztira.Text = guztiraDut.ToString(); // Guztira testu bezala ezarri
        }

        private void ErreserbaEgin()
        {
            // Aste hasiera eta amaiera kalkulatu
            var asteHasiera = BookingDatePicker.Date.StartOfWeek(DayOfWeek.Monday); // Aste hasiera
            var asteAmaiera = asteHasiera.AddDays(6); // Aste amaiera
            string hilabeteIzena = LortuEuskeraHilabeteIzena(asteHasiera.Month); // Hilabete izena lortu euskeraz
            string asteIrangoa = $"{asteHasiera:yyyy}: {hilabeteIzena} {asteHasiera.Day}-{asteAmaiera.Day}"; // Aste irangoa osatu

            // Astea erreserbatuta dagoen ala ez egiaztatu
            if (AsteaErreserbatutaDago(asteIrangoa))
            {
                DisplayAlert("Errorea", "Aste hau dagoeneko erreserbaturik dago.", "Onartu"); // Errorea jakinarazi
            }
            else
            {
                // Erreserbatutako astea bildumara gehitu
                ErreserbatutakoAsteak.Add(asteIrangoa);
                DisplayAlert("Arrakasta", "Erreserba ongi egin da.", "Onartu"); // Arrakasta jakinarazi
            }
        }

        private string LortuEuskeraHilabeteIzena(int hilabetea)
        {
            // Hilabete izenen hiztegia euskeraz
            string[] hilabeteakEuskeraz = {
                "Urtarrila", // Urtarrila
                "Otsaila",   // Otsaila
                "Martxoa",   // Martxoa
                "Apirila",   // Apirila
                "Maiatza",   // Maiatza
                "Ekaina",    // Ekaina
                "Uztaila",   // Uztaila
                "Abuztua",   // Abuztua
                "Iraila",    // Iraila
                "Urria",     // Urria
                "Azaroa",    // Azaroa
                "Abendua"    // Abendua
            };

            return hilabeteakEuskeraz[hilabetea - 1]; // Indizea egokitu hilabeteak 1etik hasten direlako
        }

        private bool AsteaErreserbatutaDago(string asteIrangoa)
        {
            // Astea erreserbatuta dagoen egiaztatu
            return ErreserbatutakoAsteak.Contains(asteIrangoa);
        }

        private void ErreserbaBotoia_Clicked(object sender, EventArgs e)
        {
            // Botoia sakatzean erreserba funtzioa deitzen du
            ErreserbaEgin();
        }
    }

    // Aste hasiera kalkulatzeko luzapen klasea
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            // Aste hasiera kalkulatzeko logika
            int desberdintasuna = (7 + (dt.DayOfWeek - startOfWeek)) % 7; // Aste hasierara arteko desberdintasuna
            return dt.AddDays(-1 * desberdintasuna).Date; // Aste hasiera itzuli
        }
    }
}
