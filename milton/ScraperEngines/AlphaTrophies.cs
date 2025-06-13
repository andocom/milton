using System.Text.RegularExpressions;

namespace milton.ScraperEngines
{
    public class AlphaTrophies : ICompetitorScraper
    {
        public string CompetitorName => "Alpha Trophies";
        private static readonly HttpClient _http = new();
        public string Log { get; set; }

        public async Task<decimal?> GetPriceAsync(string sku)
        {
            try
            {
                using var http = new HttpClient();

                // You might want to build this from the sku if needed
                var url = $"https://alphatrophies.com.au/product/{sku}";
                var html = await http.GetStringAsync(url);

                // Look for the fbq ViewContent call
                var match = Regex.Match(html, @"fbq\('track',\s*'ViewContent',\s*\{.*?value:\s*(\d+\.\d{2})", RegexOptions.Singleline);

                return match.Success ? decimal.Parse(match.Groups[1].Value) : (decimal?)null;
            }
            catch
            {
                return null;
            }
        }
    }
}
