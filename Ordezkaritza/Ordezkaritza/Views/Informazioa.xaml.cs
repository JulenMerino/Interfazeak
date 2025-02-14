using Ordezkaritza.Data;
using Ordezkaritza.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace Ordezkaritza.Views;

public partial class Informazioa : ContentPage
{
    private readonly Database _database;
    private string filePath;
    public ObservableCollection<Katalogoa> Katalogoa { get; set; }
    private ObservableCollection<Partner> Partners;

    public Informazioa()
    {
        InitializeComponent();
        _database = new Database("Komertzialak.db");
        Katalogoa = new ObservableCollection<Katalogoa>();
        Partners = new ObservableCollection<Partner>();


        ProduktuColection();
        LoadPartners();
    }


    // XML fitxategiak Irakurri

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







    //Eskaera zatia

    private void pkPartner_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pkPartner.SelectedIndex != -1)
        {
            var selectedPartner = (Partner)pkPartner.SelectedItem;
            etyHelbidea.Text = selectedPartner?.Helbidea ?? "";
        }
    }

    private async void LoadPartners()
    {
        var partners = await _database.GetAllPartnersAsync();
        Partners.Clear();
        foreach (var partner in partners)
        {
            Partners.Add(partner);
        }
        pkPartner.ItemsSource = Partners;
    }


    //  Botón para aumentar cantidad (Botón +)
    private decimal CalcularTotal()
    {
        return Katalogoa.Sum(p => p.PrezioTotala); //  Suma de (Cantidad * Precio) de todos los productos
    }

    // Botón para aumentar cantidad (Botón +)
    private void btnGehituKantitatea_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Katalogoa produktua)
        {
            if (produktua.Kantitatea < produktua.Stock) // No superar stock máximo
            {
                produktua.Kantitatea++;
                ActualizarTotal(); // Actualiza el precio total
            }
        }
    }

    // Botón para disminuir cantidad (Botón -)
    private void btnKenduaKantitatea_Clicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is Katalogoa produktua)
        {
            if (produktua.Kantitatea > 0) // No bajar de 0
            {
                produktua.Kantitatea--;
                ActualizarTotal(); // Actualiza el precio total
            }
        }
    }

    private async void ActualizarTotal()
    {
        etyPrezioTotala.Text = CalcularTotal().ToString("F2");
    }

    private async void btnFaktura_Clicked(object sender, EventArgs e)
    {
        // Obtenemos el Partner seleccionado
        var partnerSeleccionado = pkPartner.SelectedItem as Partner;
        if (partnerSeleccionado == null)
        {
            await DisplayAlert("Error", "Selecciona un partner antes de continuar.", "OK");
            return;
        }

        // Guardamos la información del formulario antes de recorrer los productos
        var eskaera = new Eskaera_Goiburua
        {
            Partner_ID = partnerSeleccionado.Partner_ID,      // Obtenemos el ID del partner
            Komertzial_ID = partnerSeleccionado.ID_komertzial, // Obtenemos el ID del comercial
            Egoera = "Pendiente",
            Data = dpData.Date.ToString("yyyy/MM/dd")
        };

        // Insertamos la información en la base de datos
        await _database.InsertEskaeraGoiburuaAsync(eskaera);

        // Recorremos todos los productos y guardamos los que tienen una cantidad mayor a 0
        foreach (var produktua in Katalogoa)
        {
            if (produktua.Kantitatea > 0)
            {
                var nuevaEskaera = new Eskaera_Xehetasuna
                {
                    Eskaera_kod = eskaera.Eskaera_kod,
                    Produktu_kod = produktua.Produktu_kod.ToString(),
                    Deskribapena = produktua.Izena,
                    Guztira = produktua.Prezioa * produktua.Kantitatea,
                    Prezioa = produktua.Prezioa,
                    Kantitatea = produktua.Kantitatea
                };

                Debug.WriteLine($"Eskaera: {nuevaEskaera.Produktu_kod} - {nuevaEskaera.Deskribapena} - {nuevaEskaera.Kantitatea} - {nuevaEskaera.Guztira} - {eskaera.Eskaera_kod}");

                // Guardamos el nuevo registro en la tabla
                await _database.InsertEskaeraXehetasunaAsync(nuevaEskaera);
            }
        }

        var bidalketa = new Bidalketa
        {
            Enpresa_izena = "Correos",
            Data = dpData.Date.ToString("yyyy/MM/dd"),
            Eskaera_kod = eskaera.Eskaera_kod
        };

        await _database.InsertBidalketaAsync(bidalketa);


        // Después de guardar, actualizamos el total
        ActualizarTotal();

        await Navigation.PushAsync(new Views.FakturaIkusi(partnerSeleccionado, eskaera.Eskaera_kod.ToString()));
    }




    //  Si el usuario edita manualmente el Entry
    private void etyKantitatea_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && entry.BindingContext is Katalogoa produktua)
        {
            if (int.TryParse(entry.Text, out int nuevaCantidad))
            {
                produktua.Kantitatea = nuevaCantidad; //  Se ajustará automáticamente al rango 0 - Stock
            }
            else
            {
                entry.Text = produktua.Kantitatea.ToString(); //  Evita valores no numéricos
            }
        }
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
        cvEskaera.ItemsSource = Katalogoa;
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




    //Informeak/Estadisticas zatia


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


