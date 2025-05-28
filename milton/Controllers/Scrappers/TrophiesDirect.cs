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


