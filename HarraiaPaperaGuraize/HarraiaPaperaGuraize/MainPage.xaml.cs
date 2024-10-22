namespace HarraiaPaperaGuraize
{
    public partial class MainPage : ContentPage
    {
        private readonly string[] _aukera = { "piedra", "papel", "tijeras" }; // Jokoaren aukera posibleak
        private readonly Random _random = new(); // Random objektua
        private int jokalariPuntuak = 0; // Jokalariaren puntuazioa
        private int makinaPuntuak = 0; // Makina puntuazioa

        public MainPage()
        {
            InitializeComponent(); // Osagaiak hasieratu
            ResetPuntuak(); // Puntuazioak berrezarri
        }

        private void ResetPuntuak()
        {
            NirePuntuak.Text = "0"; // Jokalariaren puntuak
            MakinarenPuntuak.Text = "0"; // Makina puntuak
        }

        private void JokalariakAukeratutakoan(object sender, EventArgs e)
        {
            // Jokalariaren hautaketa irudiaren izena lortu
            var jokalariAukera = ((Image)sender).Source.ToString()
                .Replace("piedra.png", "piedra")
                .Replace("papel.png", "papel")
                .Replace("tijeras.png", "tijeras");

            var makinaAukera = LortuMakinaAukera(); // Makina aukeraketa
            MakinarenAukera.Source = makinaAukera + ".png"; // Makina aukeratutako irudia ezarri

            var emaitza = LortuJokoEmaitza(jokalariAukera, makinaAukera); // Emaitza lortu
            EguneratuPuntuak(jokalariAukera, makinaAukera); // Puntuak eguneratu
            IrabazleaAldizkatuta(); // Irabazlea egiaztatu
        }

        private string LortuMakinaAukera()
        {
            int index = _random.Next(_aukera.Length); // Aukera aleatorio bat lortu
            return _aukera[index];
        }

        private string LortuJokoEmaitza(string jokalari, string makina)
        {
            if (jokalari == makina)
                return "berdinketa"; // Berdinketa

            return (jokalari, makina) switch
            {
                ("piedra", "tijeras") => "jokalaria", // Jokalaria irabazten du
                ("piedra", "papel") => "makina",  // Makina irabazten du
                ("papel", "piedra") => "jokalaria", // Jokalaria irabazten du
                ("papel", "tijeras") => "makina", // Makina irabazten du
                ("tijeras", "papel") => "jokalaria", // Jokalaria irabazten du
                ("tijeras", "piedra") => "makina", // Makina irabazten du
                _ => "berdinketa" // Berdinketa kasu guztiak
            };
        }

        private void EguneratuPuntuak(string jokalariAukera, string makinaAukera)
        {
            // Jokalariak irabazten badu puntuazioa handitu
            if (jokalariAukera.Contains("piedra") && makinaAukera.Contains("tijeras") ||
                jokalariAukera.Contains("papel") && makinaAukera.Contains("piedra") ||
                jokalariAukera.Contains("tijeras") && makinaAukera.Contains("papel"))
            {
                jokalariPuntuak++;
            }
            // Makina irabazten badu puntuazioa handitu
            else if (makinaAukera.Contains("piedra") && jokalariAukera.Contains("tijeras") ||
                     makinaAukera.Contains("papel") && jokalariAukera.Contains("piedra") ||
                     makinaAukera.Contains("tijeras") && jokalariAukera.Contains("papel"))
            {
                makinaPuntuak++;
            }

            // Puntuak eguneratu UI-an
            NirePuntuak.Text = jokalariPuntuak.ToString();
            MakinarenPuntuak.Text = makinaPuntuak.ToString();
        }

        private void IrabazleaAldizkatuta()
        {
            // Jokalariak irabazi duen egiaztatu
            if (int.Parse(NirePuntuak.Text) >= 10)
            {
                DisplayAlert("Irabazlea!", "Jokalaria irabazi du.", "OK");
                ResetPuntuak(); // Mezua erakutsi ondoren puntuazioak berrezarri
                return;
            }
            // Makina irabazi duen egiaztatu
            else if (int.Parse(MakinarenPuntuak.Text) >= 10)
            {
                DisplayAlert("Irabazlea!", "Makina irabazi du.", "OK");
                ResetPuntuak(); // Mezua erakutsi ondoren puntuazioak berrezarri
                return;
            }
        }

        private void Irten(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}
