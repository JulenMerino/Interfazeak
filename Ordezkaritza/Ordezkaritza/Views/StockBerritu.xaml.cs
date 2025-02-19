using Ordezkaritza.Data;
using Ordezkaritza.Models;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Ordezkaritza.Views;

public partial class StockBerritu : ContentPage
{
    private readonly Database _database;
    public ObservableCollection<Katalogoa> Katalogoa { get; set; }
    public StockBerritu()
	{
		InitializeComponent();

        _database = new Database("Komertzialak.db");
        Katalogoa = new ObservableCollection<Katalogoa>();
        

        ProduktuaKargatu();
    }

    private async void ProduktuaKargatu()
    {
        var produktuak = await _database.GetAllKatalogoasAsync();

        Katalogoa.Clear();

        foreach (var produktuenInformazioa in produktuak)
        {
            Katalogoa.Add(new Katalogoa
            {
                Produktu_kod = produktuenInformazioa.Produktu_kod,
                Izena = produktuenInformazioa.Izena,
                Prezioa = produktuenInformazioa.Prezioa,
                Stock = 0,
                Irudia = LortuIrudia(produktuenInformazioa.Produktu_kod)
            });
        }

        ProduktuakColection.ItemsSource = Katalogoa;
    }

    private string LortuIrudia(int produktuKodea)
    {

        switch (produktuKodea)
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


    private void btnGehitu_Clicked(object sender, EventArgs e)
    {

        var botoia = (Button)sender;
        var produktua = (Katalogoa)botoia.BindingContext;

        produktua.Stock++;


    }

    private void btnKendu_Clicked(object sender, EventArgs e)
    {

        var botoia = (Button)sender;
        var produktua = (Katalogoa)botoia.BindingContext;


        if (produktua.Stock > 0)
        {
            produktua.Stock--;
        }

    }



    private async void btnSortuXML_Clicked(object sender, EventArgs e)
    {
        var kantitateaDutenProduktuak = Katalogoa.Where(p => p.Stock > 0).ToList();

        if (kantitateaDutenProduktuak.Count == 0)
        {
            DisplayAlert("Arazoa", "Ez duzu kantitaterik jarri.", "OK");
            return;
        }

        var xml = new XDocument(
            new XElement("productos",
                kantitateaDutenProduktuak.Select(p => new XElement("producto",
                    new XElement("codigo", p.Produktu_kod),
                    new XElement("nombre", p.Izena),
                    new XElement("precio", p.Prezioa),
                    new XElement("stock", p.Stock)
                ))
            )
        );

        string data = DateTime.Now.ToString("yyyyMMdd_HHmm");

        string fitxategiIzena = $"Eskaera_{data}.xml";

        string fitxategiHelbidea = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fitxategiIzena);

        xml.Save(fitxategiHelbidea);

        DisplayAlert("Lortuta", $"XML zuzenki sortu da helbide honetan: {Path.GetFullPath(fitxategiHelbidea)}", "OK");

        int eskaeraKodea = _database.LortuHurregoEskaeraKod();

        foreach (var produktuInfrmazioa in kantitateaDutenProduktuak)
        {
            var EgoitzaNagusia = new EgoitzaNagusia
            {
                Eskaera_kod = eskaeraKodea,
                Izena = produktuInfrmazioa.Izena,
                Kantitatea = produktuInfrmazioa.Stock,  
                Produktu_kod = produktuInfrmazioa.Produktu_kod
            };

            await _database.InsertEgoitzaNagusiaAsync(EgoitzaNagusia);
        }

    }


}