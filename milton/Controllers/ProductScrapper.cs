using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;

public class ProductInfo
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public decimal FromPrice { get; set; }
    public decimal ToPrice { get; set; }

    public override string ToString() => $"{Name}, {SKU}, {FromPrice}, {ToPrice}";
}

public class GtmProductData
{
    public int internal_id { get; set; }
    public int item_id { get; set; }
    public string item_name { get; set; }
    public string sku { get; set; }
    public decimal price { get; set; }
    public object stocklevel { get; set; }  // could be null
    public string stockstatus { get; set; }
    public string google_business_vertical { get; set; }
    public string item_category { get; set; }
    public int id { get; set; }
    public string productlink { get; set; }
    public string item_list_name { get; set; }
    public int index { get; set; }
    public string product_type { get; set; }
    public string item_brand { get; set; }
}

public class ProductScraper
{
    private readonly HttpClient _httpClient;

    public ProductScraper()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    }

    public async Task<List<GtmProductData>> ScrapeProductsAsync(string url)
    {
        var products = new List<ProductInfo>();
        var productDatas = new List<GtmProductData>();

        var html = await _httpClient.GetStringAsync(url);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        //
        var nodes = doc.DocumentNode.SelectNodes("//span[@class='gtm4wp_productdata']");
        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                string encodedJson = node.GetAttributeValue("data-gtm4wp_product_data", "");
                string decodedJson = WebUtility.HtmlDecode(encodedJson);

                var productData = JsonSerializer.Deserialize<GtmProductData>(decodedJson);
                if (productData != null) productDatas.Add(productData);
            }
        }

        return productDatas;

        //// Select all product elements
        //var productNodes = doc.DocumentNode.SelectNodes("//li[contains(@class, 'type-product')]");

        //if (productNodes != null)
        //{
        //    foreach (var node in productNodes)
        //    {
        //        // Extract product name
        //        var nameNode = node.SelectSingleNode(".//h3");
        //        var name = nameNode?.InnerText.Trim() ?? "N/A";

        //        // Extract SKU from the product link
        //        var skuNode = node.SelectSingleNode(".//span[@class='n-product-sku']");
        //        var sku = skuNode?.InnerText.Trim() ?? "N/A";
        //        //var linkNode = node.SelectSingleNode(".//a[@href]");
        //        //var href = linkNode?.GetAttributeValue("href", "");
        //        //var sku = ExtractSKUFromUrl(href);

        //        // Extract price range
        //        var priceNode = node.SelectSingleNode(".//span[contains(@class, 'price')]");
        //        var priceText = priceNode?.InnerText.Trim().Replace("\n", "").Replace("\r", "") ?? "N/A";
        //        var (fromPrice, toPrice) = ParsePriceRange(priceNode?.InnerText ?? "");

        //        products.Add(new ProductInfo
        //        {
        //            Name = name,
        //            SKU = sku,
        //            FromPrice = fromPrice,
        //            ToPrice = toPrice
        //        });
        //    }
        //}

        //return products;
    }

    private string ExtractSKUFromUrl(string url)
    {
        if (string.IsNullOrEmpty(url))
            return "N/A";

        // Assuming the SKU is the last segment of the URL
        var segments = url.TrimEnd('/').Split('/');
        return segments.Length > 0 ? segments[^1].ToUpper() : "N/A";
    }

    private (decimal fromPrice, decimal toPrice) ParsePriceRange(string priceText)
    {
        if (string.IsNullOrWhiteSpace(priceText))
            return (0, 0);

        var cleaned = priceText.Replace("$", "").Replace("–", "-").Trim();
        var parts = cleaned.Split('-');

        if (parts.Length == 2 &&
            decimal.TryParse(parts[0].Trim(), out var from) &&
            decimal.TryParse(parts[1].Trim(), out var to))
        {
            return (from, to);
        }

        // If there's only one price
        if (decimal.TryParse(cleaned, out var singlePrice))
        {
            return (singlePrice, singlePrice);
        }

        return (0, 0); // fallback on parse failure
    }
}

