using finals.Models;
using finals.Services;

namespace finals;

public partial class ConverterPage : ContentPage
{
    private readonly CurrencyApiService _currencyService = new CurrencyApiService();
    public ConverterPage()
	{
		InitializeComponent();
        LoadCurrencies();
    }
    private async void OnBackToHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
    private async void LoadCurrencies()
    {
        try
        {
            Dictionary<string, string> currencies = await _currencyService.GetCurrenciesAsync();

            var currency = currencies
                .Select(c => new CurrencyItem
                {
                    Code = c.Key.ToUpper(),
                    Name = c.Value,
                })
                .OrderBy(c => c.Code)
                .ToList();

            ToCurrency.ItemsSource = currency;
            ToCurrency.ItemDisplayBinding = new Binding("PickerDisplay");

            FromCurrency.ItemsSource = currency;
            FromCurrency.ItemDisplayBinding = new Binding("PickerDisplay");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to fetch rates: " + ex.Message, "OK");
        }
    }
    private async void OnConvertButtonClicked(object sender, EventArgs e)
    {
        var fromItem = FromCurrency.SelectedItem as CurrencyItem;
        var toItem = ToCurrency.SelectedItem as CurrencyItem;
        var selectedDate = DatePicker.Date;
        var date = selectedDate.ToString("yyyy-MM-dd");

        if (fromItem == null || toItem == null || string.IsNullOrWhiteSpace(AmountEntry.Text))
            return;

        if (!double.TryParse(AmountEntry.Text, out double amount))
        {
            await DisplayAlert("Error", "Invalid amount", "OK");
            return;
        }

        try
        {
            var result = await _currencyService.ConvertAsync(fromItem.Code, toItem.Code, amount, date);

            ResultAmount.Text = $"{result.Result:F2}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to convert: " + ex.Message, "OK");
        }
    }
}