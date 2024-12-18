namespace HarraiaPaperaGuraize
{
    public partial class MainPage : ContentPage
    {
        private readonly string[] _aukera = { "piedra", "papel", "tijeras" };
        private readonly Random _random = new();
        private int jokalariPuntuak = 0;
        private int makinaPuntuak = 0;

        public MainPage()
        {
            InitializeComponent();
            ResetPuntuak();
        }

        /// <summary>
        /// Puntuazioak berrezartzen ditu.
        /// </summary>
        private void ResetPuntuak()
        {
            NirePuntuak.Text = "0";
            MakinarenPuntuak.Text = "0";
        }

        /// <summary>
        /// Jokalariak irudi bat aukeratzen duenean ekintza hau burutzen da.
        /// </summary>
        private void JokalariakAukeratutakoan(object sender, EventArgs e)
        {
            var jokalariAukera = ((Image)sender).Source.ToString()
                .Replace("piedra.png", "piedra")
                .Replace("papel.png", "papel")
                .Replace("tijeras.png", "tijeras");

            var makinaAukera = LortuMakinaAukera();
            MakinarenAukera.Source = makinaAukera + ".png";

            var emaitza = LortuJokoEmaitza(jokalariAukera, makinaAukera);
            EguneratuPuntuak(jokalariAukera, makinaAukera);
            IrabazleaAldizkatuta();
        }

        /// <summary>
        /// Makina hautaketa aleatorioa egiten du.
        /// </summary>
        private string LortuMakinaAukera()
        {
            int index = _random.Next(_aukera.Length);
            return _aukera[index];
        }

        /// <summary>
        /// Jokoaren emaitza kalkulatzen du jokalariaren eta makinaren aukeren arabera.
        /// </summary>
        private string LortuJokoEmaitza(string jokalari, string makina)
        {
            if (jokalari == makina)
                return "berdinketa";

            return (jokalari, makina) switch
            {
                ("piedra", "tijeras") => "jokalaria",
                ("piedra", "papel") => "makina",
                ("papel", "piedra") => "jokalaria",
                ("papel", "tijeras") => "makina",
                ("tijeras", "papel") => "jokalaria",
                ("tijeras", "piedra") => "makina",
                _ => "berdinketa"
            };
        }

        /// <summary>
        /// Puntuazioa eguneratzen du jokoaren emaitzaren arabera.
        /// </summary>
        private void EguneratuPuntuak(string jokalariAukera, string makinaAukera)
        {
            if (jokalariAukera.Contains("piedra") && makinaAukera.Contains("tijeras") ||
                jokalariAukera.Contains("papel") && makinaAukera.Contains("piedra") ||
                jokalariAukera.Contains("tijeras") && makinaAukera.Contains("papel"))
            {
                jokalariPuntuak++;
            }
            else if (makinaAukera.Contains("piedra") && jokalariAukera.Contains("tijeras") ||
                     makinaAukera.Contains("papel") && jokalariAukera.Contains("piedra") ||
                     makinaAukera.Contains("tijeras") && jokalariAukera.Contains("papel"))
            {
                makinaPuntuak++;
            }

            NirePuntuak.Text = jokalariPuntuak.ToString();
            MakinarenPuntuak.Text = makinaPuntuak.ToString();
        }

        /// <summary>
        /// Irabazlea egiaztatzen du eta behar izanez gero puntuazioa berrezartzen du.
        /// </summary>
        private void IrabazleaAldizkatuta()
        {
            if (int.Parse(NirePuntuak.Text) >= 10)
            {
                DisplayAlert("Irabazlea!", "Jokalaria irabazi du.", "OK");
                ResetPuntuak();
                return;
            }
            else if (int.Parse(MakinarenPuntuak.Text) >= 10)
            {
                DisplayAlert("Irabazlea!", "Makina irabazi du.", "OK");
                ResetPuntuak();
                return;
            }
        }

        /// <summary>
        /// Botoia sakatzean aplikazioa ixten du.
        /// </summary>
        private void Irten(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}
