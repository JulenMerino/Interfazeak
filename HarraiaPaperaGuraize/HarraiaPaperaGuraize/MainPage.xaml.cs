namespace HarraiaPaperaGuraize
{
    public partial class MainPage : ContentPage
    {
        private readonly string[] _opciones = { "piedra", "papel", "tijeras" };
        private readonly Random _random = new();
        private int playerScore = 0;
        private int computerScore = 0;

        public MainPage()
        {
            InitializeComponent();
            ResetScores();
        }

        private void ResetScores()
        {
            NirePuntuak.Text = "0";  // Puntos del jugador
            MakinarenPuntuak.Text = "0"; // Puntos de la máquina
        }

        private void OnPlayerChoiceTapped(object sender, EventArgs e)
        {
            var playerChoice = ((Image)sender).Source.ToString()
                .Replace("piedra.png", "piedra")
                .Replace("papel.png", "papel")
                .Replace("tijeras.png", "tijeras");

            var machineChoice = GetMachineChoice();
            MakinarenAukera.Source = machineChoice + ".png";

            var result = GetGameResult(playerChoice, machineChoice);
            UpdateScores(playerChoice, machineChoice);
        }

        private string GetMachineChoice()
        {
            int index = _random.Next(_opciones.Length);
            return _opciones[index];
        }

        private string GetGameResult(string player, string machine)
        {
            if (player == machine)
                return "draw"; // Empate

            return (player, machine) switch
            {
                ("piedra", "tijeras") => "player", // Jugador gana
                ("piedra", "papel") => "machine",  // Máquina gana
                ("papel", "piedra") => "player",    // Jugador gana
                ("papel", "tijeras") => "machine",  // Máquina gana
                ("tijeras", "papel") => "player",   // Jugador gana
                ("tijeras", "piedra") => "machine", // Máquina gana
                _ => "draw" // Para todos los empates
            };
        }

        private void UpdateScores(string playerChoice, string computerChoice)
        {
            if (playerChoice.Contains("piedra") && computerChoice.Contains("tijera") ||
                playerChoice.Contains("papel") && computerChoice.Contains("piedra") ||
                playerChoice.Contains("tijera") && computerChoice.Contains("papel"))
            {
                playerScore++;
            }
            else if (computerChoice.Contains("piedra") && playerChoice.Contains("tijera") ||
                     computerChoice.Contains("papel") && playerChoice.Contains("piedra") ||
                     computerChoice.Contains("tijera") && playerChoice.Contains("papel"))
            {
                computerScore++;
            }

            NirePuntuak.Text = playerScore.ToString();
            MakinarenPuntuak.Text = computerScore.ToString();
        }

        private void CheckWinner()
        {
            // Comprobar si el jugador ha ganado
            if (int.Parse(NirePuntuak.Text) >= 10)
            {
                DisplayAlert("¡Ganador!", "El jugador ha ganado el juego.", "OK");
                ResetScores(); // Reiniciar puntuaciones después de mostrar el mensaje
            }
            // Comprobar si la máquina ha ganado
            else if (int.Parse(MakinarenPuntuak.Text) >= 10)
            {
                DisplayAlert("¡Ganador!", "La máquina ha ganado el juego.", "OK");
                ResetScores(); // Reiniciar puntuaciones después de mostrar el mensaje
            }
        }
    }
}