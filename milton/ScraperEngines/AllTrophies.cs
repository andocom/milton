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
    public class AllTrophies
    {
        private static readonly HttpClient _http = new();
        public string Log { get; set; }


        public async Task<decimal> GetPriceAsync(string sku)
        {
            var url = $"https://alltrophies.com.au/wp-admin/admin-ajax.php?action=flatsome_ajax_search_products&query={Uri.EscapeDataString(sku)}";
            var json = await _http.GetStringAsync(url);
            Log += "Fetched JSON : " + HttpUtility.HtmlDecode(json);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<SuggestionResponse>(json, options);

            if (data?.Suggestions != null)
            {
                foreach (var item in data.Suggestions)
                {
                    Console.WriteLine($"Product: {item.Value}, Price HTML: {item.Price}");
                }
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
