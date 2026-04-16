using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace finals.Models
{
    public class CurrencySymbolsResponse
    {
        public Dictionary<string, string> Symbols { get; set; }
    }
    public class CurrencyItem
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public string PickerDisplay => $"{Code} - {Name}";
    }
    public class LatestRatesResponse
    {
        public string Date { get; set; }
        public string Base {  get; set; }
        public Dictionary<string, double> Rates { get; set; }
    }
    public class DisplayRates
    {
        public string CurrencyCode { get; set; }
        public double Rate { get; set; }
    }
    public class ConverterResponse
    {
        public Query Query { get; set;}
        public string Date { get; set;}
        public double Result { get; set;}
    }
    public class Query
    {
        public string From { get; set; }
        public string To { get; set; }
        public double Amount { get; set; }
    }
}
