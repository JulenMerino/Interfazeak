using Ordezkaritza.Data;
using Ordezkaritza.Models;
using System.Globalization;
namespace Ordezkaritza.Views;

public partial class FakturaIkusi : ContentPage
{
    private Partner _partnerSeleccionado;
    private readonly Database _database;
    private Bidalketa _bidalketa;
    private string _eskaeraKod;

    public FakturaIkusi(Partner partner, string eskaeraKod)
	{
		InitializeComponent();
        _database = new Database("Komertzialak.db");
        _partnerSeleccionado = partner;
        _eskaeraKod = eskaeraKod;
        ErakutsiPartnerInformazioa();
        KargatuFakturaProduktuak();
        KargatuBidalketa();

    }



    private async void KargatuBidalketa()
    {
        if (!string.IsNullOrEmpty(_eskaeraKod))
        {
            // Buscar la entrega asociada a este Eskaera_kod
            _bidalketa = await _database.GetBidalketaByEskaeraKodAsync(int.Parse(_eskaeraKod));

            if (_bidalketa != null)
            {
                lblFakturaData.Text = _bidalketa.Data;
                lblFakturaKodea.Text = _bidalketa.Eskaera_kod.ToString();
            }
        }
    }
    private void ErakutsiPartnerInformazioa()
    {
        if (_partnerSeleccionado != null)
        {
            lblPartnerIzena.Text = _partnerSeleccionado.Izena;
            lblPartnerHelbidea.Text = _partnerSeleccionado.Helbidea;
            lblPartnerTelefonoa.Text = _partnerSeleccionado.Telefonoa;
        }
    }


    private async void KargatuFakturaProduktuak()
    {
        // Filtra las 'Eskaera_Xehetasuna' según el 'Eskaera_kod'
        List<Eskaera_Xehetasuna> eskaerak = await _database.GetEskaeraByEskaeraKodAsync(int.Parse(_eskaeraKod));

        tbProduktuak.Clear(); // Limpiar datos previos

        foreach (var eskaera in eskaerak)
        {
            var cell = new ViewCell
            {
                View = new HorizontalStackLayout
                {
                    Children =
                {
                    new Label { Text = eskaera.Deskribapena, Margin = new Thickness(200,0,100,0), WidthRequest = 200, VerticalTextAlignment = TextAlignment.Center },
                    new Label { Text = eskaera.Kantitatea.ToString(), Margin = new Thickness(0,0,100,0), WidthRequest = 200, VerticalTextAlignment = TextAlignment.Center },
                    new Label { Text = eskaera.Prezioa.ToString("C", CultureInfo.CurrentCulture), Margin = new Thickness(-50,0,100,0), WidthRequest = 150, VerticalTextAlignment = TextAlignment.Center },
                    new Label { Text = eskaera.Guztira.ToString("C", CultureInfo.CurrentCulture), Margin = new Thickness(25,0,100,0), WidthRequest = 150, VerticalTextAlignment = TextAlignment.Center }
                }
                }
            };

            tbProduktuak.Add(cell);
        }
    }



}
