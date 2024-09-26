namespace Opari_lista
{
    using Microsoft.Maui.Controls;

    public partial class MainPage : ContentPage
    {

        int kontagailua;
        private string lehenOpariaSeleccionado;
        private string bigarrenOpariaSeleccionado;


        public MainPage()
        {
            InitializeComponent();

            var opariList = new List<Opari>
            {
                new Opari { Izena = "Erlojua", Irudiak = "erlojua.png"},
                new Opari { Izena = "Aurikularrak", Irudiak = "aurikularrak.png"},
                new Opari { Izena = "Bozgoragailuak", Irudiak = "bozgoragailua.png"},
                new Opari { Izena = "Eramangarria", Irudiak = "eramangarria.png"},
                new Opari { Izena = "Bizikleta elektrikoa", Irudiak = "bizikleta.png"}
            };

            
            opariLista.ItemsSource = opariList;
            opariLista.ItemTemplate = new DataTemplate(() =>
            {
                var label = new Label
                {
                    VerticalOptions = LayoutOptions.Center
                };
                label.SetBinding(Label.TextProperty, "Izena");
                return new ViewCell { View = label };
            });

            
            opariLista.IsVisible = true;
        }
        private void aukeratuIrudia(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as Opari;

            if (selectedItem != null)
            {
                irudiak.Source = selectedItem.Irudiak;
            }
        }

        private void gehitu(object sender, EventArgs e)
        {
            var selectedItem = opariLista.SelectedItem as Opari;

            if (selectedItem == null)
            {
                DisplayAlert("Aukeratu oparia", "Mesedez, aukeratu oparia bat.", "OK");
                return;
            }

            if (kontagailua == 0)
            {
                lehenOpariaSeleccionado = selectedItem.Izena;
                lehenOparia.Text = $"Lehen oparia: {lehenOpariaSeleccionado}";
                kontagailua += 1;
            }
            else if (kontagailua == 1)
            {
                bigarrenOpariaSeleccionado = selectedItem.Izena;
                bigarrenOparia.Text = $"Bigarren oparia: {bigarrenOpariaSeleccionado}";
                kontagailua += 1;
            }
            else
            {
                DisplayAlert("Opari gehiegi", "Ezin dira bi oparai baino gehiago eskatu", "OK");
            }
           
        }

        private void kendu(object sender, EventArgs e)
        {
            if (kontagailua == 0)
            {
                DisplayAlert("Ez dago oparia", "Ez dago lehen oparia ezabatzeko.", "OK");
                return;
            }

            if (kontagailua > 0)
            {
                
                lehenOpariaSeleccionado = null;
                bigarrenOpariaSeleccionado = null;
                lehenOparia.Text = "Lehen oparia: ";
                bigarrenOparia.Text = "Bigarren oparia: ";
                kontagailua = 0;
            }
          
        }

        private void irten(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }

    }

}

public class Opari
{
    public string Izena { get; set; }
    public string Irudiak { get; set; }
}
