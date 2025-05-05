using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using HtmlAgilityPack;




public class ProductScraper
{

    //public async Task ScrapeAllFromDb(string sourceName)
    //{
    //    var sources = await _db.ScrapeSources
    //        .Where(s => s.SourceName == sourceName && s.Active)
    //        .ToListAsync();

    //    foreach (var src in sources)
    //    {
    //        var results = await ScrapeProductsAsync(src.Url);
    //        foreach (var r in results)
    //        {
    //            r.Source = sourceName;
    //            r.Category ??= src.Category;
    //        }

    //        if (results.Any())
    //            await _snapshotService.SaveSnapshotsAsync(results);
    //    }
    //}

}

