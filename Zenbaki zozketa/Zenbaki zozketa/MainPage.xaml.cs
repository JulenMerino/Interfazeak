namespace Zenbaki_zozketa
{
    public partial class MainPage : ContentPage
    {
        private List<CheckBox> checkBoxes = new List<CheckBox>();
        private List<int> selectedNumbers = new List<int>();
        private List<int> irazbazitakoZenbakiakEty = new List<int>();
        private const int totalNumbers = 49;
        private const int maxSelection = 6;

        // Saria hitzordutegia irabazien kopuruaren arabera
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
            GenerateCheckBoxes(); // CheckBox-ak dinamikoak sortu
        }

        // 49 CheckBox sortzen ditu 7x7 koadernoan
        private void GenerateCheckBoxes()
        {
            int columns = 7;  // Koadernoan zutabe kopurua
            int rows = 7;     // Koadernoan errenkada kopurua

            for (int i = 1; i <= totalNumbers; i++)
            {
                // CheckBox eta Label-a izango dituen StackLayout horizontala sortu
                var stackLayout = new HorizontalStackLayout();

                CheckBox checkBox = new CheckBox { IsEnabled = true };

                // CheckBox-aren BindingContext-ean zenbakia gorde
                checkBox.BindingContext = i;

                checkBox.CheckedChanged += CheckboxAldaketak;

                Label numberLabel = new Label
                {
                    Text = i.ToString(),
                    VerticalOptions = LayoutOptions.Center
                };

                // CheckBox eta Label-a StackLayout-ean gehitu
                stackLayout.Children.Add(checkBox);
                stackLayout.Children.Add(numberLabel);

                checkBoxes.Add(checkBox);  // CheckBox-a kudeatzeko zerrendan gehitu

                // StackLayout-a koadernoan gehitu dagokion posizioan
                int row = (i - 1) / columns;
                int column = (i - 1) % columns;

                PnlCheckBox.Add(stackLayout, column, row);
            }
        }

        // CheckBox-ak hautatzen edo deshautatzen direnean kontrolatzen duen ekitaldia
        private void CheckboxAldaketak(object sender, CheckedChangedEventArgs e)
        {
            // Ekitaldia aktibatu duen CheckBox-a lortu
            var checkBox = (CheckBox)sender;

            // Verifikatu BindingContext ez dela null eta motak zuzena dela (zenbaki osoa)
            if (checkBox.BindingContext is int number)
            {
                if (e.Value) // CheckBox-a hautatzen bada
                {
                    selectedNumbers.Add(number);
                }
                else // CheckBox-a deshautatzen bada
                {
                    selectedNumbers.Remove(number);
                }

                // 6 zenbaki zehatz hautatuta egotean Sorteo botoia aktibatu edo desaktibatu
                BtnZozketa.IsEnabled = selectedNumbers.Count == maxSelection;

                // Hautatutako zenbakiak Entry-n eguneratu
                EtyZenbakiak.Text = string.Join(", ", selectedNumbers);
            }
            else
            {
                // Errorea kudeatu edo BindingContext baliogabea denean portaera
                Console.WriteLine("Errorea: CheckBox-aren BindingContext-a baliogabea da.");
            }
        }

        // Sorteo botoirako ekitaldia
        private async void Zozketa(object sender, EventArgs e)
        {
            // CheckBox guztiak desgaitzea
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsEnabled = false;
            }

            // Irabazle 6 zenbaki aleatorio sortu
            Random random = new Random();
            irazbazitakoZenbakiakEty = Enumerable.Range(1, totalNumbers)
                                           .OrderBy(x => random.Next())
                                           .Take(maxSelection)
                                           .ToList();

            // Irabazle zenbakiak interfazean garbitu
            EtyIrabazitakoZenbakiak.Children.Clear();

            // Irabazle zenbakiak labelaren eskuinean dauden Entry desberdinetan erakutsi
            foreach (var number in irazbazitakoZenbakiakEty)
            {
                Entry irazbazitakoZenbakiakEty = new Entry
                {
                    Text = number.ToString(),
                    IsReadOnly = true,
                    WidthRequest = 40 // Tamaina egokitu
                };
                EtyIrabazitakoZenbakiak.Children.Add(irazbazitakoZenbakiakEty);
            }

            // Asmatutako kopurua kalkulatu
            int asmatuakCount = selectedNumbers.Intersect(irazbazitakoZenbakiakEty).Count();
            Asmatuak.Text = asmatuakCount.ToString();

            // Asmatutako kopuruaren arabera sariak erakutsi
            string sariMessage = "Ez da irabazirik"; // Default message
            if (sariak.ContainsKey(asmatuakCount))
            {
                sariMessage = sariak[asmatuakCount];
            }

            // Alert with the prize information
            await DisplayAlert("Saria", sariMessage, "OK");

            // Nuevo botoia aktibatu
            BtnBerria.IsEnabled = true;
        }

        // Nuevo botoirako ekitaldia
        private void Berria(object sender, EventArgs e)
        {
            // Datu guztiak garbitu eta interfazea berreskuratu
            foreach (var checkBox in checkBoxes)
            {
                checkBox.IsEnabled = true;
                checkBox.IsChecked = false;
            }

            selectedNumbers.Clear();
            irazbazitakoZenbakiakEty.Clear();
            EtyZenbakiak.Text = "";
            Asmatuak.Text = "";

            // Irabazle zenbakiak garbitu
            EtyIrabazitakoZenbakiak.Children.Clear();

            // "Nuevo" eta "Sorteo" botoiak desgaitzea
            BtnBerria.IsEnabled = false;
            BtnZozketa.IsEnabled = false;
        }

        // Irten botoirako ekitaldia
        private void Irten(object sender, EventArgs e)
        {
            // Aplikaziotik irten
            Application.Current.Quit();
        }
    }
}
