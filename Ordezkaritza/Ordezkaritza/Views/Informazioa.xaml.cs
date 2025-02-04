using Ordezkaritza.Data;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Ordezkaritza.Views;

public partial class Informazioa : ContentPage
{
    private readonly Database _database;
    private string filePath;
    public ObservableCollection<Katalogoa> Katalogoa { get; set; }

    public Informazioa()
    {
        InitializeComponent();
        _database = new Database("Komertzialak.db");
        Katalogoa = new ObservableCollection<Katalogoa>();

        ProduktuColection();
    }

    // Método para seleccionar el XML
    private async void btnAukeratuXMLa_Clicked(object sender, EventArgs e)
    {
        filePath = await _database.PickXmlFileAsync(); 

        if (!string.IsNullOrEmpty(filePath))
        {
            lblEmaitzaAukeratu.Text = Path.GetFileName(filePath) + " aukeratu duzu";
        }
    }

    // Método para cargar datos del XML y guardarlos en la base de datos
    private async void btnKargatuDatuak_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            await DisplayAlert("Errorea", "Mesedez, hautatu XML fitxategia lehenik.", "OK");
            return;
        }

        await _database.SaveDataFromXmlAsync(filePath); // Guardar datos en BD

        // Actualizar la lista en la UI
        var katalogoaList = await _database.GetAllKatalogoasAsync();
        Katalogoa.Clear();
        foreach (var item in katalogoaList)
        {
            Katalogoa.Add(item);
        }

        await DisplayAlert("Arrakasta", "Datuak ongi kargatu dira!", "OK");
    }


    





    //Stock zatia


    private async void ProduktuColection()
    {
        var katalogoak = await _database.GetAllKatalogoasAsync();

        Katalogoa.Clear();

        foreach (var katalogoa in katalogoak)
        {
            Katalogoa.Add(new Katalogoa
            {
                Produktu_kod = katalogoa.Produktu_kod,
                Izena = katalogoa.Izena,
                Prezioa = katalogoa.Prezioa,
                Stock = katalogoa.Stock,
                Irudia = GetImageForProduct(katalogoa.Produktu_kod) 
            });
        }

        ProduktuakColection.ItemsSource = Katalogoa;
    }

    private string GetImageForProduct(int productCode)
    {

        switch (productCode)
        {
            case 1: 
                return "analogiko1.png";
            case 2: 
                return "analogiko2.png";
            case 3: 
                return "analogiko3.png";
            default:
                return "default_image.png";  
        }
    }

    private async void btnStocKBerritu_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new Views.StockBerritu());
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


