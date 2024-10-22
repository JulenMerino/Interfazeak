using System.Collections.ObjectModel; // ObservableCollection erabiliko dugu izenak kudeatzeko
using System.Reflection; // Reflection erabiliko dugu fitxategiak irakurtzeko

namespace AusazTxandak // Nabarmentzen dugun namespace
{
    public partial class MainPage : ContentPage // MainPage klasea, ContentPage oinordetzen duena
    {
        public ObservableCollection<string> Izenak { get; set; } = new ObservableCollection<string>(); // Izenen bilduma, UI-ra lotzeko
        private List<string> erabilgarriIzenak; // Erabilgarri dauden izenak gordetzeko zerrenda

        public MainPage() // MainPage eraikitzailea
        {
            InitializeComponent(); // UI osagaiak inicializatzeko metodoa
            this.BindingContext = this; // Lotura testuingurua definitzen dugu
        }

        private async void OnLoadNamesClicked(object sender, EventArgs e) // Izenak kargatzeko klik egin denean exekutatuko den metodoa
        {
            if (erabilgarriIzenak == null) // Erabilgarri izenak ez badira definituta
            {
                await KargatuIzenak(); // Izenak kargatzen ditugu
            }

            if (erabilgarriIzenak != null && erabilgarriIzenak.Count > 0) // Izenak erabilgarri badaude
            {
                var Aukeratutakoizena = erabilgarriIzenak[0]; // Lehen izena hautatzen dugu
                Izenak.Add(Aukeratutakoizena); // Hautatutako izena Izenak bildumara gehitzen dugu

                erabilgarriIzenak.RemoveAt(0); // Hautatutako izena erabilgarri izenak zerrendatik ezabatu

                if (erabilgarriIzenak.Count == 0) // Izen erabilgarriak agortu badira
                {
                    await DisplayAlert("Bukatuta", "Izen guztiak gehitu dira.", "OK"); // Mezua erakutsi
                }
            }
            else // Izen erabilgarriak ez badira
            {
                await DisplayAlert("Error", "Ez daude izen gehiago erabilgarri.", "OK"); // Mezua erakutsi
            }
        }

        private async void OnSaveNamesClicked(object sender, EventArgs e) // Izenak gorde klik egin denean exekutatuko den metodoa
        {
            var fitxatizena = FItxategiIzenentry.Text; // Fitxategi izena erabiltzaileak sartutakoa
            var rutafitxat = RutaFitxategiEntry.Text; // Fitxategiaren ruta erabiltzaileak sartutakoa

            if (string.IsNullOrWhiteSpace(fitxatizena) || string.IsNullOrWhiteSpace(rutafitxat)) // Sartutako datuak baliozkoak ez badira
            {
                await DisplayAlert("Error", "Mesdez, sartu ruta eta fitxategi izen egokia.", "OK"); // Mezua erakutsi
                return; // Metodoa amaitu
            }

            var fitxateiakonpleto = Path.Combine(rutafitxat, fitxatizena); // Fitxategiaren bidea osatzen dugu
            await GordeIzenakAsync(fitxateiakonpleto); // Izenak gordetzeko metodoa deitzen dugu
        }

        private async Task GordeIzenakAsync(string fitxategiaa) // Izenak gordetzeko metodoa
        {
            try
            {
                using (var stream = new FileStream(fitxategiaa, FileMode.Create)) // Fitxategia sortzen dugu
                {
                    using (var writer = new StreamWriter(stream)) // StreamWriter erabiliz idazteko prest
                    {
                        foreach (var nombre in Izenak) // Izenak bilduman zehar ibiltzen gara
                        {
                            await writer.WriteLineAsync(nombre); // Izenak fitxategian idazten dugu
                        }
                    }
                }

                await DisplayAlert("Ondo", "Izenak gorde dira.", "OK"); // Izenak ondo gorde direla adierazten dugu
            }
            catch (Exception ex) // Salbuespen bat gertatuz gero
            {
                await DisplayAlert("Error", "Arazo bat egon da izenak gordetzerakoan.", "OK"); // Mezua erakutsi
            }
        }

        private async Task KargatuIzenak() // Izenak kargatzeko metodoa
        {
            var Fitxatizena = await IrakurriFitxategiakAsync("ikasleak.txt"); // "ikasleak.txt" fitxategia irakurtzen dugu

            if (Fitxatizena != null && Fitxatizena.Length > 0) // Izenak kargatu badira
            {
                Console.WriteLine("Izenak kargatzen......"); // Kontsolan mezu bat inprimatu
                erabilgarriIzenak = Fitxatizena.ToList(); // Izenak zerrenda bihurtzen dugu
                erabilgarriIzenak = erabilgarriIzenak.OrderBy(x => Guid.NewGuid()).ToList(); // Izenak nahasten ditugu
                Console.WriteLine("Izenak nahastuta eta akrgatuta."); // Kontsolan mezu bat inprimatu
            }
            else // Izenik ez badago
            {
                NombresLabel.Text = "Ez dira izenak aurkitu."; // Label batean mezu bat jarri
            }
        }

        private async Task<string[]> IrakurriFitxategiakAsync(string fitxategi) // Fitxategiak irakurtzeko metodoa
        {
            var assembly = Assembly.GetExecutingAssembly(); // Aktiboa den assembly-a lortzen dugu
            var resourcePath = $"AusazTxandak.Resources.Raw.{fitxategi}"; // Fitxategiaren bidea osatzen dugu
            Console.WriteLine(resourcePath); // Kontsolan mezu bat inprimatu

            using (var stream = assembly.GetManifestResourceStream(resourcePath)) // Fitxategia irakurtzen saiatzen gara
            {
                if (stream != null) // Fitxategia aurkitu bada
                {
                    using (var reader = new StreamReader(stream)) // StreamReader erabiliz irakurtzeko prest
                    {
                        var contenido = await reader.ReadToEndAsync(); // Fitxategiaren edukia irakurri
                        Console.WriteLine(contenido); // Kontsolan fitxategiaren edukia inprimatu
                        return contenido.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); // Edukia lerroz banatzen dugu
                    }
                }
                else // Fitxategia aurkitu ez bada
                {
                    Console.WriteLine("Fitxategia ez da aurkitu."); // Kontsolan mezu bat inprimatu
                    NombresLabel.Text = "Fitxategia ez da aurkitu"; // Label batean mezu bat jarri
                }
            }

            return new string[0]; // Emaitzarik ez bada, zerrenda huts bat itzultzen dugu
        }
    }
}
