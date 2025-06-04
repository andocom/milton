//using System;
//using System.Collections.Generic;
//using System.Net;
//using System.Net.Http;
//using System.Text.Json;
//using System.Threading.Tasks;
//using HtmlAgilityPack;
//using milton.Models.CompetitorPrices;

//public class TrophiesDirect
//{
//    private readonly HttpClient _httpClient;
//    private readonly ProductSnapshotService _snapshotService;

//    public TrophiesDirect(ProductSnapshotService snapshotService)
//    {
//        _snapshotService = snapshotService;

//        _httpClient = new HttpClient();
//        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
//    }

//    public async Task<List<ProductSnapshot>> ScrapeProductsAsync(string url)
//    {
//        var products = new List<ProductSnapshot>();

//        var html = await _httpClient.GetStringAsync(url);
//        var doc = new HtmlDocument();
//        doc.LoadHtml(html);

//        var nodes = doc.DocumentNode.SelectNodes("//span[@class='gtm4wp_productdata']");
//        if (nodes != null)
//        {
//            foreach (var node in nodes)
//            {
//                string encodedJson = node.GetAttributeValue("data-gtm4wp_product_data", "");
//                string decodedJson = WebUtility.HtmlDecode(encodedJson);

//                var productData = JsonSerializer.Deserialize<TrophiesDirectProductData>(decodedJson);
//                if (productData != null) productData.Log = encodedJson;

//                if (productData != null)
//                {
//                    products.Add(new ProductSnapshot
//                    {
//                        Source = "TrophiesDirect",
//                        ProductId = productData.item_id.ToString(),
//                        Name = productData.item_name,
//                        SKU = productData.sku,
//                        Price = productData.price,
//                        Category = productData.item_category,
//                        SnapshotDate = DateTime.Now.Date
//                    });
//                }
//            }
//        }

//        // Save to DB
//        if (products.Count > 0)
//        {
//            await _snapshotService.SaveSnapshotsAsync(products);
//        }

//        return products;
//    }
//}

using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace milton.ScrapperEngines
{
    public class DirectTrophies
    {
        private static readonly HttpClient _http = new();
        public string Log { get; set; }

        public async Task<decimal> GetPriceAsync(string sku)
        {
            var url = $"https://directtrophies.com.au/wp-content/plugins/ajax-search-for-woocommerce-premium/includes/Engines/TNTSearchMySQL/Endpoints/search.php?s={Uri.EscapeDataString(sku)}";
            var json = await _http.GetStringAsync(url);
            Log += "Fetched JSON : " + HttpUtility.HtmlDecode(json);


            using var doc = JsonDocument.Parse(json);

            if (!doc.RootElement.TryGetProperty("suggestions", out var suggestions) || suggestions.ValueKind != JsonValueKind.Array)
                throw new Exception("Invalid response structure — 'suggestions' array missing.");

            foreach (var result in suggestions.EnumerateArray())
            {
                if (!result.TryGetProperty("sku", out var skuProp)) continue;

                var candidateSku = skuProp.GetString()?.Trim();

                if (string.Equals(candidateSku, sku, StringComparison.OrdinalIgnoreCase))
                {
                    if (!result.TryGetProperty("price", out var priceProp))
                        throw new Exception($"Price not found for SKU '{sku}'");

                    var rawPriceHtml = priceProp.GetString() ?? throw new Exception("Price value is null");

                    // Strip HTML tags from the price (e.g., <span> and <bdi>)
                    var stripped = StripHtml(rawPriceHtml);

                    // If there's a range like "21.95 – 30.95", take the first number
                    var firstPrice = stripped.Split('–', StringSplitOptions.TrimEntries)[0];

                    // Clean characters and parse
                    var cleaned = new string(firstPrice.Where(c => char.IsDigit(c) || c == '.' || c == ',').ToArray());

                    if (decimal.TryParse(cleaned, NumberStyles.Currency, CultureInfo.InvariantCulture, out var price))
                        return price;

                    throw new Exception($"Could not parse price: '{rawPriceHtml}'");
                }
            }

            throw new Exception($"SKU '{sku}' not found in suggestions.");
        }

        private static string StripHtml(string input)
        {
            var s = input.Replace("&#36;", "");
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(s);
            return doc.DocumentNode.InnerText;
        }
    }
}
