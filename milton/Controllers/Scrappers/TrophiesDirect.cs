using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;



public class TrophiesDirectProductData
{
    public string Log { get; set; }
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

public class TrophiesDirect
{
    private readonly HttpClient _httpClient;
    private readonly ProductSnapshotService _snapshotService;

    public TrophiesDirect(ProductSnapshotService snapshotService)
    {
        _snapshotService = snapshotService;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    }

    public async Task<List<ProductSnapshot>> ScrapeProductsAsync(string url)
    {
        var products = new List<ProductSnapshot>();

        var html = await _httpClient.GetStringAsync(url);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var nodes = doc.DocumentNode.SelectNodes("//span[@class='gtm4wp_productdata']");
        if (nodes != null)
        {
            foreach (var node in nodes)
            {
                string encodedJson = node.GetAttributeValue("data-gtm4wp_product_data", "");
                string decodedJson = WebUtility.HtmlDecode(encodedJson);

                var productData = JsonSerializer.Deserialize<TrophiesDirectProductData>(decodedJson);
                if (productData != null) productData.Log = encodedJson;

                if (productData != null)
                {
                    products.Add(new ProductSnapshot
                    {
                        Source = "TrophiesDirect",
                        ProductId = productData.item_id.ToString(),
                        Name = productData.item_name,
                        SKU = productData.sku,
                        Price = productData.price,
                        Category = productData.item_category,
                        SnapshotDate = DateTime.Now.Date
                    });
                }
            }
        }

        // Save to DB
        if (products.Count > 0)
        {
            await _snapshotService.SaveSnapshotsAsync(products);
        }

        return products;
    }
}


