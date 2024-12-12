using System.Diagnostics;
using System.Windows.Input;

namespace BankuKudeaketa.Views
{
    public partial class WindowsButton : ContentView
    {
        public static readonly BindableProperty TextProperty =
            BindableProperty.Create(nameof(Text), typeof(string), typeof(WindowsButton), default(string));

        public static readonly BindableProperty AkzioaProperty =
            BindableProperty.Create(nameof(Akzioa), typeof(ICommand), typeof(WindowsButton), default(ICommand));

        public static readonly BindableProperty LuzeraProperty =
            BindableProperty.Create(nameof(Luzera), typeof(int), typeof(WindowsButton), 100);

        public static readonly BindableProperty AktibatutaProperty =
            BindableProperty.Create(nameof(Aktibatuta), typeof(bool), typeof(WindowsButton), true);

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public ICommand Akzioa
        {
            get => (ICommand)GetValue(AkzioaProperty);
            set => SetValue(AkzioaProperty, value);
        }

        public int Luzera
        {
            get => (int)GetValue(LuzeraProperty);
            set => SetValue(LuzeraProperty, value);
        }

        public bool Aktibatuta
        {
            get => (bool)GetValue(AktibatutaProperty);
            set => SetValue(AktibatutaProperty, value);
        }

        public WindowsButton()
        {
            InitializeComponent();
            BindingContext = this;
        }
    }
}
