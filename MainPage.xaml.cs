using finals.Models;
using finals.Services;

namespace finals
{
    public partial class MainPage : ContentPage
    {
        private readonly CurrencyApiService _currencyService = new CurrencyApiService();
        public MainPage()
        {
            InitializeComponent();
            LoadCurrencies();
            LoadRates();
        }
        private void OnDateChanged(object sender, EventArgs e)
        {
            LoadRates();
        }
        private void OnCurrencyChanged(object sender, EventArgs e)
        {
            LoadRates();
        }
        private async void OnConverterPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ConverterPage));
        }
        private async void LoadCurrencies()
        {
            try
            {
                Dictionary<string, string> currencies = await _currencyService.GetCurrenciesAsync();

                var currencyList = currencies
                    .Select(c => new CurrencyItem
                    {
                        Code = c.Key.ToUpper(),
                        Name = c.Value
                    })
                    .OrderBy(c => c.Code)
                    .ToList();

                CurrencyPicker.ItemsSource = currencyList;
                CurrencyPicker.ItemDisplayBinding = new Binding("PickerDisplay");

                var phpIndex = currencyList.FindIndex(c => c.Code == "PHP");
                if (phpIndex >= 0)
                    CurrencyPicker.SelectedIndex = phpIndex;
                else 
                    CurrencyPicker.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to fetch rates: " + ex.Message, "OK");
            }
        }
        private async void LoadRates()
        {
            try
            {
                string baseCurrency = (CurrencyPicker.SelectedItem as CurrencyItem)?.Code ?? "PHP";

                var selectedDate = DatePicker.Date;
                var date = selectedDate.ToString("yyyy-MM-dd");

                var latest = await _currencyService.GetLatestRatesAsync(baseCurrency, "USD, EUR, JPY, AUD, CAD", date);
                    
                var displayList = latest.Rates
                    .Select(r => new DisplayRates { CurrencyCode = r.Key, Rate = r.Value })
                    .Take(5)
                    .ToList();

                CurrenciesCollection.ItemsSource = displayList;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to fetch rates: " + ex.Message, "OK");
            }
        }
    }
}
