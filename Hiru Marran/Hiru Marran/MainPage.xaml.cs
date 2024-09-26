namespace Hiru_Marran
{
    public partial class MainPage : ContentPage
    {
        private bool isXTurn = true; // Txanda norena den kontrolatzen du
        private int scoreX = 0;      // X-ek irabazitako partidak
        private int scoreO = 0;      // O-ek irabazitako partidak
        private Button[,] buttons;   // Taulako botoiak

        public MainPage()
        {
            InitializeComponent();
            buttons = new Button[,]
            {
                { Button00, Button01, Button02 },
                { Button10, Button11, Button12 },
                { Button20, Button21, Button22 }
            };

            // Inicializar la tabla y los marcadores al abrir la aplicación
            ResetBoard();
        }

        // Taulako botoi bat sakatzen denean
        private void OnButtonClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            // Botoia dagoeneko markatuta dagoen egiaztatzen du
            if (button.Text != "")
                return;

            // Botoia X edo O-rekin markatzen du txandaren arabera
            button.Text = isXTurn ? "X" : "O";

            if (isXTurn)
            {
                button.TextColor = Colors.Red;
            }
            else
            {
                button.TextColor = Colors.Blue;
            }

            // Irabazle bat dagoen egiaztatzen du
            if (CheckWinner())
            {
                if (isXTurn)
                {
                    scoreX++;
                    lblMarkagailuaX.Text = scoreX.ToString();
                    ShowEndGameAlert("X jokalariak irabazi du!");
                }
                else
                {
                    scoreO++;
                    lblMarkagailuaO.Text = scoreO.ToString();
                    ShowEndGameAlert("O jokalariak irabazi du!");
                }

                return;
            }

            // Txanda aldatzen du
            isXTurn = !isXTurn;
            lblTxanda.Text = "Txanda: " + (isXTurn ? "X" : "O");

            // Berdinketa dagoen egiaztatzen du
            if (IsBoardFull())
            {
                ShowEndGameAlert("Berdinketa da!");
            }
        }

        // Irabazle bat dagoen egiaztatzen du
        private bool CheckWinner()
        {
            // Irabazteko aukerak (lerroak, zutabeak eta diagonalak)
            string[,] board = new string[3, 3];
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    board[row, col] = buttons[row, col].Text;
                }
            }

            // Lerroak, zutabeak eta diagonalak
            for (int i = 0; i < 3; i++)
            {
                if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 0] != "")
                    return true;
                if (board[0, i] == board[1, i] && board[1, i] == board[2, i] && board[0, i] != "")
                    return true;
            }

            if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[0, 0] != "")
                return true;
            if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[0, 2] != "")
                return true;

            return false;
        }

        // Taula beteta dagoen egiaztatzen du
        private bool IsBoardFull()
        {
            foreach (var button in buttons)
            {
                if (button.Text == "")
                    return false;
            }
            return true;
        }

        // Taula berrezartzen du
        private void ResetBoard()
        {
            foreach (var button in buttons)
            {
                button.Text = "";
            }
            isXTurn = true;
            lblTxanda.Text = "Txanda: X";
        }

        // Muestra el alert final con opciones
        private async void ShowEndGameAlert(string message)
        {
            var result = await DisplayAlert("Joko amaitu da", message + "\nBeste partida bat jokatu nahi duzu?", "Bai", "Ez");
            if (result)
            {
                ResetBoard();
            }
            else
            {
                // Cierra la aplicación (o puedes llevar al usuario a una pantalla de inicio si lo prefieres)
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }

        // Joko osoa berrezartzen du (berrezarpen botoia)
        private void OnResetClicked(object sender, EventArgs e)
        {
            ResetBoard();
            scoreX = 0;
            scoreO = 0;
            lblMarkagailuaX.Text = scoreX.ToString();
            lblMarkagailuaO.Text = scoreO.ToString();
        }
    }
}
