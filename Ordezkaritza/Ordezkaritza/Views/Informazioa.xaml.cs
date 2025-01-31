
using Syncfusion.Maui.Core.Carousel;
using System.Collections.ObjectModel;

namespace Ordezkaritza.Views;

public partial class Informazioa : ContentPage
{
    public ObservableCollection<Produktua> Produktuak { get; set; }
    public Informazioa()
    {
        InitializeComponent();
        ProduktuColection();

    }

    //XML-ak irakurtzeko zatia


    private void btnAukeratuXMLa_Clicked(object sender, EventArgs e)
    {

    }

    private void btnKargatuDatuak_Clicked(object sender, EventArgs e)
    {
    }





    //Stock zatia


    private void ProduktuColection()
    {
        Produktuak = new ObservableCollection<Produktua>
            {
                new Produktua { Irudia = "analogiko1.png", Izena = "Producto 1", Kantitatea = 5, Prezioa = "10.00€" },
                new Produktua { Irudia = "analogiko2.png", Izena = "Producto 2", Kantitatea = 113, Prezioa = "15.50€" },
                new Produktua { Irudia = "analogiko3.png", Izena = "Producto 3", Kantitatea = 10, Prezioa = "7.75€" },
                new Produktua { Irudia = "analogiko4.png", Izena = "Producto 4", Kantitatea = 7, Prezioa = "21.75€" },
                new Produktua { Irudia = "analogiko5.png", Izena = "Producto 5", Kantitatea = 11, Prezioa = "18.75€" },
               
            };

        ProduktuakColection.ItemsSource = Produktuak;
    }

    private async void btnStocKBerritu_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.StockBerritu());
    }

    public class Produktua
    {
        public string Irudia { get; set; }
        public string Izena { get; set; }
        public int Kantitatea { get; set; }
        public string Prezioa { get; set; }
    }






    private void btnHilabetekoEskaerak_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Eskaera 1", "Eskaera 2", "Eskaera 3" };
        ActualizarInformeakTableView(datos);
    }

    private void btnEskaerakEgoitzaNagusira_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Egoitza Nagusira 1", "Egoitza Nagusira 2" };
        ActualizarInformeakTableView(datos);
    }

    private void btnGehienEskatutakoProduktua_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Produktua A", "Produktua B", "Produktua C" };
        ActualizarInformeakTableView(datos);
    }

    private void btnGehienSaltzenDuenBazkidea_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Bazkidea 1", "Bazkidea 2" };
        ActualizarInformeakTableView(datos);
    }

    private void btnIrabaziHandienaDuenBazkidea_Clicked(object sender, EventArgs e)
    {
        var datos = new List<string> { "Bazkidea X - 1000€", "Bazkidea Y - 900€" };
        ActualizarInformeakTableView(datos);
    }

    private void ActualizarInformeakTableView(List<string> datos)
    {
        InfoSection.Clear(); 

        foreach (var dato in datos)
        {
            InfoSection.Add(new TextCell { Text = dato });
        }
    }

   
}


