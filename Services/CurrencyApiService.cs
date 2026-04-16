using finals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace finals.Services
{
    public class CurrencyApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();

        private const string ApiHost = "currency-conversion-and-exchange-rates.p.rapidapi.com";
        private const string ApiKey = "c90e116b84mshdf253e9e4986417p17e2c7jsn499969846595";
        private const string BaseUrl = "https://currency-conversion-and-exchange-rates.p.rapidapi.com";

        public CurrencyApiService()
        {
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", ApiKey);
            _httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", ApiHost);
        }

        public async Task<Dictionary<string, string>> GetCurrenciesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<CurrencySymbolsResponse>($"{BaseUrl}/symbols");

            return response?.Symbols ?? new Dictionary<string, string>();
        }
        public async Task<LatestRatesResponse> GetLatestRatesAsync(string @base, string symbols = "", string? date = null)
        {
            string endpoint;
            if (string.IsNullOrWhiteSpace(date))
                endpoint = $"{BaseUrl}/latest?base={@base}&symbols={symbols}";
            else
                endpoint = $"{BaseUrl}/{date}?base={@base}&symbols={symbols}";
                
            var response = await _httpClient.GetFromJsonAsync<LatestRatesResponse>(endpoint);

            return response;
        }
        public async Task<ConverterResponse> ConvertAsync(string from, string to, double amount, string date)
        {
            var response = await _httpClient.GetFromJsonAsync<ConverterResponse>($"{BaseUrl}/convert?from={from}&to={to}&amount={amount}&date={date}");

            return response ?? new ConverterResponse
            {
                Result = 0,
                Query = new Query { From = from, To = to, Amount = amount }
            };
        }
    }
}
