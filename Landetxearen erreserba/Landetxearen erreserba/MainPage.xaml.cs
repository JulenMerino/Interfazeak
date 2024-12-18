using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Landetxearen_erreserba
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> Irudiak { get; set; }
        public ObservableCollection<string> ErreserbatutakoAsteak { get; set; }

        public MainPage()
        {
            InitializeComponent();
            IrudiakHasieratu();
            ErreserbatutakoAsteak = new ObservableCollection<string>();
            BindingContext = this;
        }

        /// <summary>
        /// Irudien bilduma hasieratzen du.
        /// </summary>
        private void IrudiakHasieratu()
        {
            Irudiak = new ObservableCollection<string>
            {
                "bat.jpg",
                "bi.jpg",
                "hiru.jpg",
                "lau.jpg"
            };
        }

        /// <summary>
        /// CheckBox bat aldatu denean guztirako prezioa eguneratzen du.
        /// </summary>
        public void CheckBoxAldatzean(object sender, CheckedChangedEventArgs e)
        {
            GuztiraEguneratu();
        }

        /// <summary>
        /// Guztirako prezioa kalkulatzen du eta interfazeko testuak eguneratzen ditu.
        /// </summary>
        private void GuztiraEguneratu()
        {
            decimal guztira = 0;

            if (ChAireportu.IsChecked)
                guztira += 25;

            if (ChBisita.IsChecked)
                guztira += 150;

            if (ChUme.IsChecked)
                guztira += 40;

            EtyGehigarri.Text = guztira.ToString();

            decimal estanciaPrezioa = 300;
            decimal gehigarriakPrezioa = guztira;
            decimal guztiraDut = estanciaPrezioa + gehigarriakPrezioa;
            EtyGuztira.Text = guztiraDut.ToString();
        }

        /// <summary>
        /// Erreserba egiten du eta astea erreserbatuta dagoen egiaztatzen du.
        /// </summary>
        private void ErreserbaEgin()
        {
            var asteHasiera = BookingDatePicker.Date.StartOfWeek(DayOfWeek.Monday);
            var asteAmaiera = asteHasiera.AddDays(6);
            string hilabeteIzena = LortuEuskeraHilabeteIzena(asteHasiera.Month);
            string asteIrangoa = $"{asteHasiera:yyyy}: {hilabeteIzena} {asteHasiera.Day}-{asteAmaiera.Day}";

            if (AsteaErreserbatutaDago(asteIrangoa))
            {
                DisplayAlert("Errorea", "Aste hau dagoeneko erreserbaturik dago.", "Onartu");
            }
            else
            {
                ErreserbatutakoAsteak.Add(asteIrangoa);
                DisplayAlert("Arrakasta", "Erreserba ongi egin da.", "Onartu");
            }
        }

        /// <summary>
        /// Hilabete zenbakia jasotzen du eta euskerazko izena itzultzen du.
        /// </summary>
        private string LortuEuskeraHilabeteIzena(int hilabetea)
        {
            string[] hilabeteakEuskeraz = {
                "Urtarrila",
                "Otsaila",
                "Martxoa",
                "Apirila",
                "Maiatza",
                "Ekaina",
                "Uztaila",
                "Abuztua",
                "Iraila",
                "Urria",
                "Azaroa",
                "Abendua"
            };

            return hilabeteakEuskeraz[hilabetea - 1];
        }

        /// <summary>
        /// Aste irangoa erreserbatuta dagoen edo ez egiaztatzen du.
        /// </summary>
        private bool AsteaErreserbatutaDago(string asteIrangoa)
        {
            return ErreserbatutakoAsteak.Contains(asteIrangoa);
        }

        /// <summary>
        /// Erreserba botoia sakatzean erreserba funtzioa exekutatzen du.
        /// </summary>
        private void ErreserbaBotoia_Clicked(object sender, EventArgs e)
        {
            ErreserbaEgin();
        }
    }

    /// <summary>
    /// Datarekin erlazionatutako luzapen metodoak eskaintzen dituen klasea.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Emandako datarekin bat datozen astearen lehen eguna itzultzen du.
        /// </summary>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int desberdintasuna = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * desberdintasuna).Date;
        }
    }
}
