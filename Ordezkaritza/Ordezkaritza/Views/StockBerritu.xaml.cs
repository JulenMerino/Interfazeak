using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Ordezkaritza.Views;

public partial class StockBerritu : ContentPage
{
    public ObservableCollection<Produktua> Produktuak { get; set; }
    public StockBerritu()
	{
		InitializeComponent();
        ProduktuColection();

    }

    private void ProduktuColection()
    {
        Produktuak = new ObservableCollection<Produktua>
            {
                new Produktua { Irudia = "analogiko1.png", Izena = "Producto 1", Kantitatea = 0 },
                new Produktua { Irudia = "analogiko2.png", Izena = "Producto 2", Kantitatea = 0 },
                new Produktua { Irudia = "analogiko3.png", Izena = "Producto 3", Kantitatea = 0 },
                new Produktua { Irudia = "analogiko4.png", Izena = "Producto 4", Kantitatea = 0 },
                new Produktua { Irudia = "analogiko5.png", Izena = "Producto 5", Kantitatea = 0 },

            };

        ProduktuakColection.ItemsSource = Produktuak;
    }

    private void btnStocKBerritu_Clicked(object sender, EventArgs e)
    {

    }

    public class Produktua : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Irudia { get; set; }
        public string Izena { get; set; }

        private int _kantitatea;
        public int Kantitatea
        {
            get => _kantitatea;
            set
            {
                if (_kantitatea != value)
                {
                    _kantitatea = value;
                    kenduBotoiaDesgaitu(nameof(Kantitatea));
                    kenduBotoiaDesgaitu(nameof(EzinKendu)); 
                }
            }
        }

        public bool EzinKendu => Kantitatea > 0;


        protected void kenduBotoiaDesgaitu(string Desgaitu)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Desgaitu));
            
        }
    }


    private void btnGehitu_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Produktua produktua)
        {
            produktua.Kantitatea++;
        }
    }

    private void btnKendu_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Produktua produktua)
        {
            produktua.Kantitatea--;
        }
    }



    private void btnSortuXML_Clicked(object sender, EventArgs e)
    {

    }

    
}