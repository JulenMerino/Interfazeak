using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Erosketa_orgatxoa
{
    public class Fruitua
    {
        public string Izena { get; set; } // Frutaren izena
        public int Kantitatea { get; set; } // Frutaren kantitatea

        public Fruitua(string izena, int kantitatea)
        {
            Izena = izena; // Frutaren izena ezarri
            Kantitatea = kantitatea; // Frutaren kantitatea ezarri
        }
    }

    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Fruitua> gehitu { get; set; } = new ObservableCollection<Fruitua>(); // Frutak gehitzeko bilduma
        private Stack<Fruitua> kendu = new Stack<Fruitua>(); // Frutak kentzeko pilatuta
        private const int fruituMax = 20; // Fruta maximoa

        public MainPage()
        {
            InitializeComponent(); // Osagaiak hasieratu
            lvFruituak.ItemsSource = gehitu; // Irudien iturria ezarri
        }

        private int GetFrutaTotal()
        {
            // Frutak guztira zenbatzea
            return gehitu.Sum(f => f.Kantitatea);
        }

        public void Hartu(object sender, DragStartingEventArgs e)
        {
            if (sender is DragGestureRecognizer dragGestureRecognizer)
            {
                var irudia = dragGestureRecognizer.Parent as Image;

                if (irudia != null)
                {
                    string fruituIzena = irudia.Source.ToString(); // Irudiaren iturria lortu

                    // Frutaren izena egokitu irudiaren izenaren arabera
                    if (fruituIzena.Contains("sandia"))
                        fruituIzena = "Sandia"; // Sandia
                    else if (fruituIzena.Contains("manzana"))
                        fruituIzena = "Sagarra"; // Sagarra
                    else if (fruituIzena.Contains("pera"))
                        fruituIzena = "Udarea"; // Udarea
                    else if (fruituIzena.Contains("naranja"))
                        fruituIzena = "Naranja"; // Naranja
                    else if (fruituIzena.Contains("pinia"))
                        fruituIzena = "Piña"; // Piña
                    else if (fruituIzena.Contains("melon"))
                        fruituIzena = "Meloia"; // Meloia

                    // Frutaren izena ez bada hutsik, irudian ezarri
                    if (!string.IsNullOrEmpty(fruituIzena))
                    {
                        e.Data.Properties.Add("FruituIzena", fruituIzena);
                    }
                }
            }
        }

        public async void Erosi(object sender, DropEventArgs e)
        {
            if (e.Data.Properties.ContainsKey("FruituIzena"))
            {
                var fruituIzena = (string)e.Data.Properties["FruituIzena"]; // Frutaren izena lortu
                e.Handled = true;

                if (!string.IsNullOrEmpty(fruituIzena))
                {
                    var fruituaBadago = gehitu.FirstOrDefault(f => f.Izena == fruituIzena); // Fruta dagoen ala ez
                    int fruituTotal = GetFrutaTotal(); // Frutak guztira zenbatzea

                    // Maximoa gainditzen bada, alerta erakutsi
                    if (fruituTotal >= fruituMax)
                    {
                        await DisplayAlert("Limitera iritsi zara", "Ezin dira 20 fruta baino gehiago sartu carritoan", "OK");
                        return;
                    }

                    // Fruta dagoen bada, kantitatea handitu
                    if (fruituaBadago != null)
                    {
                        if (fruituTotal + 1 <= fruituMax)
                        {
                            fruituaBadago.Kantitatea++;
                            gehitu.Remove(fruituaBadago); // Aurretik kendu
                            gehitu.Add(fruituaBadago); // Gehitu berriro
                        }
                    }
                    else
                    {
                        // Fruta berria gehitu
                        if (fruituTotal + 1 <= fruituMax)
                        {
                            var fruituBerria = new Fruitua(fruituIzena, 1); // Fruta berri bat sortu
                            gehitu.Add(fruituBerria); // Gehitu bildumara
                        }
                    }

                    // Azken fruta kendu pilan gorde
                    kendu.Push(new Fruitua(fruituIzena, 1));
                }
            }
        }

        private void Desegin(object sender, EventArgs e)
        {
            // Azken fruta kentzeko logika
            if (kendu.Any())
            {
                var azkenFruitua = kendu.Pop(); // Azken fruta atera
                var fruituaGehitu = gehitu.FirstOrDefault(f => f.Izena == azkenFruitua.Izena); // Fruta bilatu

                if (fruituaGehitu != null)
                {
                    if (fruituaGehitu.Kantitatea > 1)
                    {
                        fruituaGehitu.Kantitatea--; // Kantitatea murriztu
                        gehitu.Remove(fruituaGehitu); // Aurretik kendu
                        gehitu.Add(fruituaGehitu); // Gehitu berriro
                    }
                    else
                    {
                        gehitu.Remove(fruituaGehitu); // Fruta guztiz kendu
                    }
                }
            }
        }

        private void Itzuli(object sender, EventArgs e)
        {
            // Bilduma eta pilak garbitu
            gehitu.Clear();
            kendu.Clear();
        }

        private void Irten(object sender, EventArgs e)
        {
            // Aplikazioa itxi
            Application.Current.Quit();
        }
    }
}
