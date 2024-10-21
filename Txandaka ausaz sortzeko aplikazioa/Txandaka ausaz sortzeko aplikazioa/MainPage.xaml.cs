namespace Txandaka_ausaz_sortzeko_aplikazioa
{
    public partial class MainPage : ContentPage
    {
        private List<string> ikasleak;
        private List<string> ateratakoIzenak;
        private Random random;

        public MainPage()
        {
            InitializeComponent();
            random = new Random();
            ikasleak = new List<string>();
            ateratakoIzenak = new List<string>();
        }
        
        private async void KargatuFitxategia(string fitxategiBidea)
        {
            if (File.Exists(fitxategiBidea))
            {
                var lines = await File.ReadAllLinesAsync(fitxategiBidea);
                ikasleak = lines.ToList();
                AteratakoakLista.Text = "Izenak kargatuta!";
            }
            else
            {
                await DisplayAlert("Errorea", "Fitxategia ez da aurkitu.", "Ados");
            }
        }

        
        private void AteraIzenBat()
        {
            if (ikasleak.Count == 0)
            {
                AteratakoakLista.Text = "Izen guztiak atera dira.";
                return;
            }

            int index = random.Next(ikasleak.Count);
            string aukeratua = ikasleak[index];
            ikasleak.RemoveAt(index); 
            ateratakoIzenak.Add(aukeratua);

            
            AteratakoIzena.Text = $"Aukeratutako ikaslea: {aukeratua}";
            AteratakoakLista.Text = string.Join("\n", ateratakoIzenak);
        }

        
        private async void GordeFitxategia(string fitxategiBidea)
        {
            if (ateratakoIzenak.Count == 0)
            {
                await DisplayAlert("Errorea", "Ez dago gordetzeko izenik.", "Ados");
                return;
            }

            await File.WriteAllLinesAsync(fitxategiBidea, ateratakoIzenak);
            await DisplayAlert("Arrakasta", "Izenak gorde dira!", "Ados");
        }

        
        private void KargatuBotoia_Clicked(object sender, EventArgs e)
        {
            KargatuFitxategia("/bidea/ikasleak.txt"); 
        }

        private void AteraBotoia_Clicked(object sender, EventArgs e)
        {
            AteraIzenBat();
        }

        private void GordeBotoia_Clicked(object sender, EventArgs e)
        {
            GordeFitxategia("/bidea/ateratako_izenak.txt"); 
        }
    }

}


