using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace milton.ScraperEngines
{
    public class AllTrophies : ICompetitorScraper
    {
        public string CompetitorName => "All Trophies";
        private static readonly HttpClient _http = new();
        public string Log { get; set; }

        public async Task<decimal?> GetPriceAsync(string sku)
        {
            try
            {
                using var http = new HttpClient();
                var url = $"https://alltrophies.com.au/wp-admin/admin-ajax.php?action=flatsome_ajax_search_products&query={Uri.EscapeDataString(sku)}";
                var json = await http.GetStringAsync(url);

                using var doc = JsonDocument.Parse(json);
                var suggestions = doc.RootElement.GetProperty("suggestions");
                if (suggestions.GetArrayLength() == 0) return null;

                var priceHtml = suggestions[0].GetProperty("price").GetString();
                if (string.IsNullOrEmpty(priceHtml)) return null;

                var match = Regex.Matches(priceHtml, @"\d+\.\d{2}")
                                 .Cast<Match>()
                                 .LastOrDefault();

                return match != null ? decimal.Parse(match.Value) : (decimal?)null;
            }
            catch (Exception ex)
            {
                // Optional: log ex.Message or write to a failure table
                return null; // return null so app doesn't crash on scraper failures
            }
        }


        private static string StripHtml(string input)
        {
            var s = input.Replace("&#36;", "");
            var doc = new HtmlDocument();
            doc.LoadHtml(s);
            return doc.DocumentNode.InnerText;
        }

    }



    public class SuggestionResponse
    {
        [JsonPropertyName("suggestions")]
        public List<Suggestion> Suggestions { get; set; } = new();
    }

    public class Suggestion
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("img")]
        public string Img { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }
    }

}
