// Modelo con Binding para actualizar automáticamente
using SQLite;
using System.ComponentModel;

public class Katalogoa : INotifyPropertyChanged
{
    private int _stock;
    private int _kantitatea;
    private decimal _prezioa;

    [PrimaryKey]
    public int Produktu_kod { get; set; }
    public string Izena { get; set; }

    public decimal Prezioa //  Precio unitario del producto
    {
        get => _prezioa;
        set
        {
            if (_prezioa != value)
            {
                _prezioa = value;
                OnPropertyChanged(nameof(Prezioa));
                OnPropertyChanged(nameof(PrezioTotala)); //  Se recalcula el total
            }
        }
    }

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
    public int Kantitatea // Cantidad seleccionada
    {
        get => _kantitatea;
        set
        {
            if (_kantitatea != value)
            {
                _kantitatea = Math.Max(0, Math.Min(value, Stock));
                OnPropertyChanged(nameof(Kantitatea));
                OnPropertyChanged(nameof(PrezioTotala)); //  Se recalcula el total

            }
        }
    }

    public decimal PrezioTotala => Kantitatea * Prezioa; // Precio total por producto

    [Ignore]
    public string Irudia { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
