using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Erosketa_orgatxoa
{
    public class Fruitua
    {
        public string Izena { get; set; } 
        public int Kantitatea { get; set; } 

        public Fruitua(string izena, int kantitatea)
        {
            Izena = izena; 
            Kantitatea = kantitatea; 
        }
    }

    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Fruitua> gehitu { get; set; } = new ObservableCollection<Fruitua>();
        private Stack<Fruitua> kendu = new Stack<Fruitua>(); 
        private const int fruituMax = 20; 

        public MainPage()
        {
            InitializeComponent(); 
            lvFruituak.ItemsSource = gehitu; 
        }

        /// <summary>
        /// Frutak guztira zenbatzen dituen metodoa
        /// </summary>
        private int GetFrutaTotal()
        {
            return gehitu.Sum(f => f.Kantitatea); 
        }

        /// <summary>
        /// Drag-and-drop hasierako metodoa, frutaren izena zehazteko
        /// </summary>
        public void Hartu(object sender, DragStartingEventArgs e)
        {
            if (sender is DragGestureRecognizer dragGestureRecognizer)
            {
                var irudia = dragGestureRecognizer.Parent as Image;

                if (irudia != null)
                {
                    string fruituIzena = irudia.Source.ToString(); 


                    if (fruituIzena.Contains("sandia"))
                        fruituIzena = "Sandia"; 
                    else if (fruituIzena.Contains("manzana"))
                        fruituIzena = "Sagarra";
                    else if (fruituIzena.Contains("pera"))
                        fruituIzena = "Udarea"; 
                    else if (fruituIzena.Contains("naranja"))
                        fruituIzena = "Naranja"; 
                    else if (fruituIzena.Contains("pinia"))
                        fruituIzena = "Piña"; 
                    else if (fruituIzena.Contains("melon"))
                        fruituIzena = "Meloia"; 

                    if (!string.IsNullOrEmpty(fruituIzena))
                    {
                        e.Data.Properties.Add("FruituIzena", fruituIzena); 
                    }
                }
            }
        }

        /// <summary>
        /// Fruta erosi (drop) eta gehitu metodoa
        /// </summary>
        public async void Erosi(object sender, DropEventArgs e)
        {
            if (e.Data.Properties.ContainsKey("FruituIzena"))
            {
                var fruituIzena = (string)e.Data.Properties["FruituIzena"]; 
                e.Handled = true;

                if (!string.IsNullOrEmpty(fruituIzena))
                {
                    var fruituaBadago = gehitu.FirstOrDefault(f => f.Izena == fruituIzena); 
                    int fruituTotal = GetFrutaTotal(); 

                    if (fruituTotal >= fruituMax)
                    {
                        await DisplayAlert("Limitera iritsi zara", "Ezin dira 20 fruta baino gehiago sartu carritoan", "OK");
                        return;
                    }

                    if (fruituaBadago != null)
                    {
                        if (fruituTotal + 1 <= fruituMax)
                        {
                            fruituaBadago.Kantitatea++;
                            gehitu.Remove(fruituaBadago); 
                            gehitu.Add(fruituaBadago); 
                        }
                    }
                    else
                    {
                        if (fruituTotal + 1 <= fruituMax)
                        {
                            var fruituBerria = new Fruitua(fruituIzena, 1); 
                            gehitu.Add(fruituBerria); 
                        }
                    }

                    kendu.Push(new Fruitua(fruituIzena, 1));
                }
            }
        }

        /// <summary>
        /// Azken fruta kentzeko logika
        /// </summary>
        private void Desegin(object sender, EventArgs e)
        {
            if (kendu.Any())
            {
                var azkenFruitua = kendu.Pop(); 
                var fruituaGehitu = gehitu.FirstOrDefault(f => f.Izena == azkenFruitua.Izena); 

                if (fruituaGehitu != null)
                {
                    if (fruituaGehitu.Kantitatea > 1)
                    {
                        fruituaGehitu.Kantitatea--; 
                        gehitu.Remove(fruituaGehitu); 
                        gehitu.Add(fruituaGehitu); 
                    }
                    else
                    {
                        gehitu.Remove(fruituaGehitu); 
                    }
                }
            }
        }

        /// <summary>
        /// Bilduma eta pilak garbitu
        /// </summary>
        private void Itzuli(object sender, EventArgs e)
        {
            gehitu.Clear(); 
            kendu.Clear(); 
        }

        /// <summary>
        /// Aplikazioa itxi
        /// </summary>
        private void Irten(object sender, EventArgs e)
        {
            Application.Current.Quit(); 
        }
    }
}
