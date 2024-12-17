using System.Collections.ObjectModel;
using System.Reflection;

namespace AusazTxandak
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> Izenak { get; set; } = new ObservableCollection<string>();
        private List<string> erabilgarriIzenak;

        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = this;
        }

        /// <summary>
        /// Izenak kargatzeko klik egin denean exekutatuko den metodoa
        /// </summary>
        private async void OnLoadNamesClicked(object sender, EventArgs e)
        {
            if (erabilgarriIzenak == null)
            {
                await KargatuIzenak(); // Izenak kargatzeko metodoa deitzen dugu
            }

            if (erabilgarriIzenak != null && erabilgarriIzenak.Count > 0)
            {
                var Aukeratutakoizena = erabilgarriIzenak[0];
                Izenak.Add(Aukeratutakoizena);

                erabilgarriIzenak.RemoveAt(0);

                if (erabilgarriIzenak.Count == 0)
                {
                    await DisplayAlert("Bukatuta", "Izen guztiak gehitu dira.", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Ez daude izen gehiago erabilgarri.", "OK");
            }
        }

        /// <summary>
        /// Izenak gorde klik egin denean exekutatuko den metodoa
        /// </summary>
        private async void OnSaveNamesClicked(object sender, EventArgs e)
        {
            var fitxatizena = etyFitxategiarenIzena.Text;
            var rutafitxat = etyfitxategiHelbidea.Text;

            if (string.IsNullOrWhiteSpace(fitxatizena) || string.IsNullOrWhiteSpace(rutafitxat))
            {
                await DisplayAlert("Error", "Mesdez, sartu ruta eta fitxategi izen egokia.", "OK");
                return;
            }

            var fitxateiakonpleto = Path.Combine(rutafitxat, fitxatizena);
            await GordeIzenakAsync(fitxateiakonpleto); // Izenak gordetzeko metodoa deitzen dugu
        }

        /// <summary>
        /// Izenak gordetzeko metodoa
        /// </summary>
        private async Task GordeIzenakAsync(string fitxategiaa)
        {
            try
            {
                using (var stream = new FileStream(fitxategiaa, FileMode.Create))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        foreach (var nombre in Izenak)
                        {
                            await writer.WriteLineAsync(nombre); // Izenak fitxategian idazten ditugu
                        }
                    }
                }

                await DisplayAlert("Ondo", "Izenak gorde dira.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Arazo bat egon da izenak gordetzerakoan.", "OK");
            }
        }

        /// <summary>
        /// Izenak kargatzeko metodoa
        /// </summary>
        private async Task KargatuIzenak()
        {
            var Fitxatizena = await IrakurriFitxategiakAsync("ikasleak.txt"); // Izenak kargatzeko fitxategia irakurtzen dugu

            if (Fitxatizena != null && Fitxatizena.Length > 0)
            {
                Console.WriteLine("Izenak kargatzen......");
                erabilgarriIzenak = Fitxatizena.ToList();
                erabilgarriIzenak = erabilgarriIzenak.OrderBy(x => Guid.NewGuid()).ToList();
                Console.WriteLine("Izenak nahastuta eta akrgatuta.");
            }
            else
            {
                lblIzenak.Text = "Ez dira izenak aurkitu.";
            }
        }

        /// <summary>
        /// Fitxategiak irakurtzeko metodoa
        /// </summary>
        private async Task<string[]> IrakurriFitxategiakAsync(string fitxategi)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = $"AusazTxandak.Resources.Raw.{fitxategi}";
            Console.WriteLine(resourcePath);

            using (var stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var contenido = await reader.ReadToEndAsync(); // Fitxategiaren edukia irakurri
                        Console.WriteLine(contenido);
                        return contenido.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries); // Edukia lerroz banatzen dugu
                    }
                }
                else
                {
                    Console.WriteLine("Fitxategia ez da aurkitu.");
                    lblIzenak.Text = "Fitxategia ez da aurkitu";
                }
            }

            return new string[0];
        }
    }
}
