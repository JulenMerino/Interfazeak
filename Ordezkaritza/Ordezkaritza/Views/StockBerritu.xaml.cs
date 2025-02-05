using Ordezkaritza.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        ProduktuColection();

    }

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
                Stock = 0,
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


    private void btnGehitu_Clicked(object sender, EventArgs e)
    {

        var button = (Button)sender;
        var product = (Katalogoa)button.BindingContext;
        
        product.Stock++;


    }

    private void btnKendu_Clicked(object sender, EventArgs e)
    {

        var button = (Button)sender;
        var product = (Katalogoa)button.BindingContext;


        if (product.Stock > 0)
        {
            product.Stock--;
        }

    }



    private void btnSortuXML_Clicked(object sender, EventArgs e)
    {
        // Filtramos los productos cuyo stock es mayor que 0
        var productosConStock = Katalogoa.Where(p => p.Stock > 0).ToList();

        if (productosConStock.Count == 0)
        {
            // Si no hay productos con stock, mostramos un mensaje
            DisplayAlert("Error", "No hay productos con cantidad para generar el XML.", "OK");
            return;
        }

        // Crear el documento XML
        var xml = new XDocument(
            new XElement("productos",
                productosConStock.Select(p => new XElement("producto",
                    new XElement("codigo", p.Produktu_kod),
                    new XElement("nombre", p.Izena),
                    new XElement("precio", p.Prezioa),
                    new XElement("stock", p.Stock)
                ))
            )
        );

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

        string fileName = $"Eskaera_{timestamp}.xml";

        
        string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

        // Guardar el XML en el archivo
        xml.Save(filePath);

        // Informar al usuario que el archivo se ha creado correctamente
        DisplayAlert("Éxito", $"El archivo XML se ha creado correctamente en: {Path.GetFullPath(filePath)}", "OK");
    }


}