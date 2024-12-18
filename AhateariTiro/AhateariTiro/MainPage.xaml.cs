﻿namespace AhateariTiro
{

    public partial class MainPage : ContentPage
    {
        private Random random = new Random(); 
        private List<Duck> aktiboPatuak = new List<Duck>(); 
        private IDispatcherTimer jokoTimerra; 
        private IDispatcherTimer kontagailuTimerra;
        private bool jokoaHasita = false; 
        private int puntuazioa = 0; 
        private int kontagailuDenbora = 60; 

        public MainPage()
        {
            InitializeComponent(); 
            InitializeGame(); 

            
            var hasieratuBotoia = this.FindByName<Button>("btnHasi");
            hasieratuBotoia.Clicked += (s, e) => JokoHasiera();

            
            var amaituBotoia = this.FindByName<Button>("btnAmaitu");
            amaituBotoia.Clicked += (s, e) => JokoaAmaitu();
        }

        /// <summary>
        /// Jokoaren osagaiak eta timer-ak inicializatzen ditu.
        /// </summary>
        private void InitializeGame()
        {
            jokoTimerra = Application.Current.Dispatcher.CreateTimer(); 
            jokoTimerra.Interval = TimeSpan.FromMilliseconds(50); 
            jokoTimerra.Tick += JokoBueltatu; 
            jokoTimerra.Start(); 

            kontagailuTimerra = Application.Current.Dispatcher.CreateTimer(); 
            kontagailuTimerra.Interval = TimeSpan.FromSeconds(1); 
            kontagailuTimerra.Tick += Kontagailua; 
        }

        /// <summary>
        /// Jokoa hasten du eta hasierako baldintzak ezartzen ditu.
        /// </summary>
        private void JokoHasiera()
        {
            if (!jokoaHasita) 
            {
                jokoaHasita = true; 
                puntuazioa = 0; 
                kontagailuDenbora = 60; 
                lblDenabora.Text = kontagailuDenbora.ToString(); 
                lblPuntuak.Text = "Puntuak: 0"; 
                AhateaSortu(); 
                kontagailuTimerra.Start(); 
            }
        }

        /// <summary>
        /// Ahate berriak periodikoki sortzen ditu.
        /// </summary>
        private async void AhateaSortu()
        {
            while (jokoaHasita)
            {
                await Task.Delay(random.Next(2000, 5000));
                PatuaSortu(); 
            }
        }

        /// <summary>
        /// Denbora kontagailua kudeatzen du eta jokoa amaitzen du zero denean.
        /// </summary>
        private void Kontagailua(object sender, EventArgs e)
        {
            kontagailuDenbora--; 
            lblDenabora.Text = kontagailuDenbora.ToString(); 

            if (kontagailuDenbora <= 0) 
            {
                JokoaAmaitu(); 
            }
        }

        /// <summary>
        /// Jokoa amaitzen du eta erabiltzaileari puntuazioa erakusten dio.
        /// </summary>
        private void JokoaAmaitu()
        {
            jokoaHasita = false; 
            kontagailuTimerra.Stop();

            DisplayAlert("Partida Amaituta", $"zure puntuazioa: {puntuazioa}", "Berriro jolastu", "Itxi").ContinueWith(t =>
            {
                if (t.Result) 
                {
                    JokoaBerresetu(); 
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.Quit(); 
                    });
                }
            });
        }

        /// <summary>
        /// Jokoa berrabiarazten du hasierako baldintzetara itzultzeko.
        /// </summary>
        private void JokoaBerresetu()
        {
            aktiboPatuak.Clear(); 
            GameArea.Children.Clear(); 
            lblDenabora.Text = "60"; 
            lblPuntuak.Text = "Puntuak: 0"; 
            puntuazioa = 0;
        }

        /// <summary>
        /// Patua jokoan sartzen du eta interfazean erakusten du.
        /// </summary>
        private void PatuaSortu()
        {
            var patua = new Duck
            {
                IsSpeedyDuck = random.Next(2) == 0, 
                Position = new Point(-50, random.Next(50, (int)GameArea.Height - 50)) 
            };

            var patuIrudia = new Image
            {
                Source = patua.IsSpeedyDuck ? "patorapido.png" : "pato.png",
                WidthRequest = 100,
                HeightRequest = 100
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                PatuEzabatu(patua);
                puntuazioa++;
                lblPuntuak.Text = $"Puntuak: {puntuazioa}";
            };
            patuIrudia.GestureRecognizers.Add(tapGestureRecognizer);

            patua.Visual = patuIrudia;
            aktiboPatuak.Add(patua);
            GameArea.Children.Add(patuIrudia);

            AbsoluteLayout.SetLayoutBounds(patuIrudia,
                new Rect(patua.Position.X, patua.Position.Y, 50, 50));
        }

        /// <summary>
        /// Ahateen posizioa eguneratzen du eta pantailatik ateratzen direnak ezabatzen ditu.
        /// </summary>
        private void JokoBueltatu(object sender, EventArgs e)
        {
            for (int i = aktiboPatuak.Count - 1; i >= 0; i--)
            {
                var patua = aktiboPatuak[i];
                patua.Position = new Point(
                    patua.Position.X + (patua.IsSpeedyDuck ? 8 : 4),
                    patua.Position.Y);

                AbsoluteLayout.SetLayoutBounds(patua.Visual,
                    new Rect(patua.Position.X, patua.Position.Y, 50, 50));

                if (patua.Position.X > GameArea.Width)
                {
                    GameArea.Children.Remove(patua.Visual);
                    aktiboPatuak.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Ahate bat joko eremutik kentzen du.
        /// </summary>
        private void PatuEzabatu(Duck patua)
        {
            GameArea.Children.Remove(patua.Visual);
            aktiboPatuak.Remove(patua);
        }
    }

    /// <summary>
    /// Ahatearen propietateak eta egoera kudeatzeko klasea.
    /// </summary>
    public class Duck
    {
        public bool IsSpeedyDuck { get; set; } 
        public Point Position { get; set; } 
        public Image Visual { get; set; } 
    }
}
