using finals.Models;
using finals.Services;

namespace finals;

public partial class ConverterPage : ContentPage
{
    private readonly CurrencyApiService _currencyService = new CurrencyApiService();

    private List<CurrencyItem> AllCurrencies;
    private CurrencyItem SelectedFrom;
    private CurrencyItem SelectedTo;

    public ConverterPage()
    {
        InitializeComponent();
        LoadCurrencies();
    }

    // back button
    private async void OnBackToHomeClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    // load currencies
    private async void LoadCurrencies()
    {
        try
        {
            var currencies = await _currencyService.GetCurrenciesAsync();

            AllCurrencies = currencies.Select(c => new CurrencyItem
            {
                Code = c.Key.ToUpper(),
                Name = c.Value
            }).ToList();

            SelectedFrom = AllCurrencies.FirstOrDefault(c => c.Code == "USD");
            SelectedTo = AllCurrencies.FirstOrDefault(c => c.Code == "PHP");

            FromSearchEntry.Text = SelectedFrom?.PickerDisplay;
            ToSearchEntry.Text = SelectedTo?.PickerDisplay;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    // FROM search
    private void OnFromFocused(object sender, FocusEventArgs e)
    {
        FromDropdown.IsVisible = true;
        FromCollection.ItemsSource = AllCurrencies;
    }

    private void OnFromSearchChanged(object sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue;

        FromDropdown.IsVisible = true;

        FromCollection.ItemsSource = string.IsNullOrWhiteSpace(text)
            ? AllCurrencies
            : AllCurrencies.Where(c => c.PickerDisplay.ToLower().Contains(text.ToLower()));
    }

    private void OnFromSelected(object sender, SelectionChangedEventArgs e)
    {
        SelectedFrom = e.CurrentSelection.FirstOrDefault() as CurrencyItem;

        if (SelectedFrom == null) return;

        FromSearchEntry.Text = SelectedFrom.PickerDisplay;
        FromDropdown.IsVisible = false;
    }

    // TO search
    private void OnToFocused(object sender, FocusEventArgs e)
    {
        ToDropdown.IsVisible = true;
        ToCollection.ItemsSource = AllCurrencies;
    }

    private void OnToSearchChanged(object sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue;

        ToDropdown.IsVisible = true;

        ToCollection.ItemsSource = string.IsNullOrWhiteSpace(text)
            ? AllCurrencies
            : AllCurrencies.Where(c => c.PickerDisplay.ToLower().Contains(text.ToLower()));
    }

    private void OnToSelected(object sender, SelectionChangedEventArgs e)
    {
        SelectedTo = e.CurrentSelection.FirstOrDefault() as CurrencyItem;

        if (SelectedTo == null) return;

        ToSearchEntry.Text = SelectedTo.PickerDisplay;
        ToDropdown.IsVisible = false;
    }

    // convert
    private async void OnConvertButtonClicked(object sender, EventArgs e)
    {
        if (SelectedFrom == null || SelectedTo == null)
        {
            await DisplayAlert("Error", "Select currencies first", "OK");
            return;
        }

        if (!double.TryParse(AmountEntry.Text, out double amount))
        {
            await DisplayAlert("Error", "Invalid amount", "OK");
            return;
        }

        try
        {
            var date = DatePicker.Date.ToString("yyyy-MM-dd");

            var result = await _currencyService.ConvertAsync(
                SelectedFrom.Code,
                SelectedTo.Code,
                amount,
                date);

            ResultAmount.Text = result.Result.ToString();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }
}