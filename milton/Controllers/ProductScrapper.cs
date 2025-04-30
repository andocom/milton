using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

public class ProductInfo
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public string PriceRange { get; set; }

    public override string ToString() => $"{Name}, {SKU}, {PriceRange}";
}

public class ProductScraper
{
    private readonly HttpClient _httpClient;

    public ProductScraper()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    }

    public async Task<List<ProductInfo>> ScrapeProductsAsync(string url)
    {
        var products = new List<ProductInfo>();

        var html = await _httpClient.GetStringAsync(url);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        // Select all product elements
        var productNodes = doc.DocumentNode.SelectNodes("//li[contains(@class, 'type-product')]");

        if (productNodes != null)
        {
            foreach (var node in productNodes)
            {
                // Extract product name
                var nameNode = node.SelectSingleNode(".//h2");
                var name = nameNode?.InnerText.Trim() ?? "N/A";

                // Extract SKU from the product link
                var linkNode = node.SelectSingleNode(".//a[@href]");
                var href = linkNode?.GetAttributeValue("href", "");
                var sku = ExtractSKUFromUrl(href);

                // Extract price range
                var priceNode = node.SelectSingleNode(".//span[contains(@class, 'price')]");
                var priceText = priceNode?.InnerText.Trim().Replace("\n", "").Replace("\r", "") ?? "N/A";
                var priceRange = NormalizePrice(priceText);

                products.Add(new ProductInfo
                {
                    Name = name,
                    SKU = sku,
                    PriceRange = priceRange
                });
            }
        }

        return products;
    }

    private string ExtractSKUFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return "N/A";

        // Assuming the SKU is the last segment of the URL
        var segments = url.TrimEnd('/').Split('/');
        return segments.Length > 0 ? segments[^1].ToUpper() : "N/A";
    }

    private string NormalizePrice(string priceText)
    {
        if (string.IsNullOrEmpty(priceText))
            return "N/A";

        // Remove currency symbols and whitespace
        var cleaned = priceText.Replace("$", "").Replace("–", "-").Replace("–", "-").Trim();

        // Split the price range
        var parts = cleaned.Split('-');
        if (parts.Length == 2)
        {
            return $"${parts[0].Trim()} - ${parts[1].Trim()}";
        }
        else
        {
            return $"${cleaned}";
        }
    }
}

