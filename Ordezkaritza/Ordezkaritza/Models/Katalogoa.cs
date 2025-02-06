using SQLite;
using System.ComponentModel;

public class Katalogoa : INotifyPropertyChanged
{
    private int _stock;

    [PrimaryKey]
    public int Produktu_kod { get; set; }

    public string Izena { get; set; }

    public decimal Prezioa { get; set; }

    public int Stock
    {
        get => _stock;
        set
        {
            if (_stock != value)
            {
                _stock = value;
                OnPropertyChanged(nameof(Stock));  
            }
        }
    }

    [Ignore]
    public string Irudia { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
