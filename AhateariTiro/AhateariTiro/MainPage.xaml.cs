namespace AhateariTiro
{
    public partial class MainPage : ContentPage
    {
        private Random random = new Random();
        private int score = 0;
        private List<Duck> activeDucks = new List<Duck>();
        private IDispatcherTimer gameTimer;

        public int Score
        {
            get => score;
            set
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            InitializeGame();
        }

        private void InitializeGame()
        {
            gameTimer = Application.Current.Dispatcher.CreateTimer();
            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            // Inicializar el audio

            // Configurar el evento de tap para toda la página
            GameArea.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command<Point>(HandleShot)
            });

            // Comenzar a generar patos
            StartDuckGeneration();
        }

        private async void StartDuckGeneration()
        {
            while (true)
            {
                await Task.Delay(random.Next(2000, 5000));
                CreateDuck();
            }
        }

        private void CreateDuck()
        {
            var duck = new Duck
            {
                IsSpeedyDuck = random.Next(2) == 0,
                Position = new Point(-50, random.Next(50, (int)GameArea.Height - 50))
            };

            var duckImage = new Image
            {
                Source = duck.IsSpeedyDuck ? "patorapido.png" : "pato.png",
                WidthRequest = 100,
                HeightRequest = 100
            };

            duck.Visual = duckImage;
            activeDucks.Add(duck);
            GameArea.Children.Add(duckImage);

            AbsoluteLayout.SetLayoutBounds(duckImage,
                new Rect(duck.Position.X, duck.Position.Y, 50, 50));
        }

        private void GameLoop(object sender, EventArgs e)
        {
            for (int i = activeDucks.Count - 1; i >= 0; i--)
            {
                var duck = activeDucks[i];
                duck.Position = new Point(
                    duck.Position.X + (duck.IsSpeedyDuck ? 8 : 4),
                    duck.Position.Y);

                AbsoluteLayout.SetLayoutBounds(duck.Visual,
                    new Rect(duck.Position.X, duck.Position.Y, 50, 50));

                // Eliminar patos que salen de la pantalla
                if (duck.Position.X > GameArea.Width)
                {
                    GameArea.Children.Remove(duck.Visual);
                    activeDucks.RemoveAt(i);
                }
            }
        }

        private async void HandleShot(Point tapPosition)
        {
            // Reproducir sonido de disparo

            for (int i = activeDucks.Count - 1; i >= 0; i--)
            {
                var duck = activeDucks[i];
                var duckBounds = new Rect(
                    duck.Position.X,
                    duck.Position.Y,
                    50, 50);

                if (duckBounds.Contains(tapPosition))
                {
                    // Sumar 1 punto al marcador
                    Score += 1;

                    // Animación de eliminación
                    await duck.Visual.RotateTo(360, 500);
                    await duck.Visual.FadeTo(0, 200);

                    GameArea.Children.Remove(duck.Visual);
                    activeDucks.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class Duck
    {
        public bool IsSpeedyDuck { get; set; }
        public Point Position { get; set; }
        public Image Visual { get; set; }
    }
}
