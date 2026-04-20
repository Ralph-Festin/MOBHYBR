using finals.Models;
using finals.Services;

namespace finals
{
    public partial class MainPage : ContentPage
    {
        private readonly CurrencyApiService _currencyService = new CurrencyApiService();

        private List<CurrencyItem> AllCurrencies;
        private CurrencyItem SelectedCurrency;

        public MainPage()
        {
            InitializeComponent();
            LoadCurrencies();
            LoadRates();

            CurrencyDropdownFrame.IsVisible = false;
        }

        // Date changed
        private void OnDateChanged(object sender, EventArgs e)
        {
            LoadRates();
        }

        // Open + filter dropdown (DOES NOT HIDE)
        private void OnCurrencySearchChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;

            CurrencyDropdownFrame.IsVisible = true;

            if (AllCurrencies == null)
                return;

            var filtered = string.IsNullOrWhiteSpace(text)
                ? AllCurrencies
                : AllCurrencies
                    .Where(c => c.PickerDisplay
                    .ToLower()
                    .Contains(text.ToLower()))
                    .ToList();

            CurrencySearchCollection.ItemsSource = filtered;
        }

        // When entry is focused, show full list
        private void OnCurrencyFocused(object sender, FocusEventArgs e)
        {
            CurrencyDropdownFrame.IsVisible = true;
            CurrencySearchCollection.ItemsSource = AllCurrencies;
        }

        // Select currency
        private void OnCurrencySelected(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection.FirstOrDefault() as CurrencyItem;

            if (selected == null)
                return;

            SelectedCurrency = selected;

            CurrencySearchEntry.Text = selected.PickerDisplay;

            LoadRates();
        }

        // Navigation
        private async void OnConverterPageClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ConverterPage));
        }

        // Load currencies
        private async void LoadCurrencies()
        {
            try
            {
                Dictionary<string, string> currencies = await _currencyService.GetCurrenciesAsync();

                AllCurrencies = currencies
                    .Select(c => new CurrencyItem
                    {
                        Code = c.Key.ToUpper(),
                        Name = c.Value
                    })
                    .OrderBy(c => c.Code)
                    .ToList();

                SelectedCurrency = AllCurrencies
                    .FirstOrDefault(c => c.Code == "PHP")
                    ?? AllCurrencies.FirstOrDefault();

                CurrencySearchEntry.Text = SelectedCurrency?.PickerDisplay;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        // Load rates
        private async void LoadRates()
        {
            try
            {
                string baseCurrency = SelectedCurrency?.Code ?? "PHP";

                var date = DatePicker.Date.ToString("yyyy-MM-dd");

                var latest = await _currencyService.GetLatestRatesAsync(
                    baseCurrency,
                    "USD, EUR, JPY, AUD, CAD",
                    date);

                CurrenciesCollection.ItemsSource = latest.Rates
                    .Select(r => new DisplayRates
                    {
                        CurrencyCode = r.Key,
                        Rate = r.Value
                    })
                    .Take(5)
                    .ToList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}