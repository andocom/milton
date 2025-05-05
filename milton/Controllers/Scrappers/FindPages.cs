using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;

public class FindPages
    {
    private readonly HttpClient _httpClient;

    public FindPages()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
    }


    public async Task<List<string>> ScrapeLinks(string url, string linkPath)
    {
        var products = new List<ProductSnapshot>();

        var html = await _httpClient.GetStringAsync(url);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var urls = new List<string>();

        //var nodes = doc.DocumentNode.SelectNodes("//a[contains(@class, 'mega-menu-link')]");
        var nodes = doc.DocumentNode.SelectNodes(linkPath);

        if (nodes != null)
        {
            foreach (var node in nodes)
            {

                //Add url to urls
                urls.Add(node.GetAttributeValue("href", ""));
            }
        }

        return urls;
    }
}

