namespace Zenbaki_zozketa
{
    public partial class MainPage : ContentPage
    {
        private List<CheckBox> checkBoxes = new List<CheckBox>();
        private List<int> selectedNumbers = new List<int>();
        private List<int> irazbazitakoZenbakiakEty = new List<int>();
        private const int totalNumbers = 49;
        private const int maxSelection = 6;

        private Dictionary<int, string> sariak = new Dictionary<int, string>
        {
            { 0, "Ez da irabazirik" },
            { 1, "Kontrako saria" },
            { 2, "20 €" },
            { 3, "50 €" },
            { 4, "100 €" },
            { 5, "500 €" },
            { 6, "5000 € Jackpot" }
        };

        public MainPage()
        {
            InitializeComponent();
            GenerateCheckBoxes();
        }

        /// <summary>
        /// 49 CheckBox-ak sortzen ditu 7x7 koadernoan.
        /// </summary>
        private void GenerateCheckBoxes()
        {
            int columns = 7;
            int rows = 7;

            for (int i = 1; i <= totalNumbers; i++)
            {
                var stackLayout = new HorizontalStackLayout();

                CheckBox checkBox = new CheckBox { IsEnabled = true };
                checkBox.BindingContext = i;
                checkBox.CheckedChanged += CheckboxAldaketak;

                Label numberLabel = new Label
                {
                    Text = i.ToString(),
                    VerticalOptions = LayoutOptions.Center
                };

                stackLayout.Children.Add(checkBox);
                stackLayout.Children.Add(numberLabel);

                checkBoxes.Add(checkBox);

                int row = (i - 1) / columns;
                int column = (i - 1) % columns;

                PnlCheckBox.Add(stackLayout, column, row);
            }
        }

        /// <summary>
        /// CheckBox-ak hautatzen edo deshautatzen direnean kudeatzen du.
        /// </summary>
        private void CheckboxAldaketak(object sender, CheckedChangedEventArgs e)
        {
            var checkBox = (CheckBox)sender;

            if (checkBox.BindingContext is int number)
            {
                if (e.Value)
                {
                    selectedNumbers.Add(number);
                }
                else
                {
                    selectedNumbers.Remove(number);
                }

                BtnZozketa.IsEnabled = selectedNumbers.Count == maxSelection;
                EtyZenbakiak.Text = string.Join(", ", selectedNumbers);
            }
            else
            {
                Console.WriteLine("Errorea: CheckBox-aren BindingContext-a baliogabea da.");
            }
        }

        /// <summary>
        /// Botoia sakatzean zozketa egiten du.
        /// </summary>
        private async void Zozketa(object sender, EventArgs e)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsEnabled = false;
            }

            Random random = new Random();
            irazbazitakoZenbakiakEty = Enumerable.Range(1, totalNumbers)
                                           .OrderBy(x => random.Next())
                                           .Take(maxSelection)
                                           .ToList();

            EtyIrabazitakoZenbakiak.Children.Clear();

            foreach (var number in irazbazitakoZenbakiakEty)
            {
                Entry irazbazitakoZenbakiakEty = new Entry
                {
                    Text = number.ToString(),
                    IsReadOnly = true,
                    WidthRequest = 40
                };
                EtyIrabazitakoZenbakiak.Children.Add(irazbazitakoZenbakiakEty);
            }

            int asmatuakCount = selectedNumbers.Intersect(irazbazitakoZenbakiakEty).Count();
            Asmatuak.Text = asmatuakCount.ToString();

            string sariMessage = "Ez da irabazirik";
            if (sariak.ContainsKey(asmatuakCount))
            {
                sariMessage = sariak[asmatuakCount];
            }

            await DisplayAlert("Saria", sariMessage, "OK");

            BtnBerria.IsEnabled = true;
        }

        /// <summary>
        /// Botoia sakatzean interfazea garbitzen du.
        /// </summary>
        private void Berria(object sender, EventArgs e)
        {
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsEnabled = true;
                checkBox.IsChecked = false;
            }

            selectedNumbers.Clear();
            irazbazitakoZenbakiakEty.Clear();
            EtyZenbakiak.Text = "";
            Asmatuak.Text = "";
            EtyIrabazitakoZenbakiak.Children.Clear();

            BtnBerria.IsEnabled = false;
            BtnZozketa.IsEnabled = false;
        }

        /// <summary>
        /// Botoia sakatzean aplikaziotik irteten da.
        /// </summary>
        private void Irten(object sender, EventArgs e)
        {
            Application.Current.Quit();
        }
    }
}
