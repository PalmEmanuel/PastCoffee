using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ApiIsolated
{
    public class CoffeeEntryFunction
    {
        private readonly ILogger _logger;
        private readonly TableClient _tableClient;
        private readonly BlobContainerClient _blobClient;

        public CoffeeEntryFunction(ILoggerFactory loggerFactory,
            TableClient tableClient,
            BlobContainerClient blobClient)
        {
            _logger = loggerFactory.CreateLogger<CoffeeEntryFunction>();
            _blobClient = blobClient;
            _tableClient = tableClient;
        }

        [Function("AddCoffeeEntry")]
        public async Task<HttpResponseData> AddCoffeeEntry([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "CoffeeEntry")] HttpRequestData req)
        {
            var entry = await req.ReadFromJsonAsync<CoffeeEntry>();

            var response = req.CreateResponse(HttpStatusCode.OK);

            var entity = new TableEntity(new Dictionary<string, object>
            {
                { "CoffeeBeans", entry.CoffeeBeans },
                { "TasteNotes", entry.TasteNotes },
                { "Description", entry.Description },
                { "Rating", entry.Rating },
                { "BrewTime", entry.BrewTime },
                { "Time", entry.Time },
                { "ImagePath", entry.ImagePath }
            });
            entity.PartitionKey = "PastCoffeeUserId";
            entity.RowKey = Guid.NewGuid().ToString();
            entity.Timestamp = entry.Time;

            _tableClient.AddEntity(entity);

            return response;
        }

        [Function("GetCoffeeEntry")]
        public async Task<HttpResponseData> GetCoffeeEntry([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "CoffeeEntry")] HttpRequestData req)
        {
            var entities = _tableClient.Query<TableEntity>().Select(e =>
            {
                return new CoffeeEntry
                {
                   BrewTime = (int)e["BrewTime"],
                   CoffeeBeans = (string)e["CoffeeBeans"],
                   Description = (string)e["Description"],
                   ImagePath = (string)e["ImagePath"],
                   Rating = (int)e["Rating"],
                   TasteNotes = (string)e["TasteNotes"],
                   Time = (DateTimeOffset)e["Time"]
                };
            }).ToList();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(entities);

            return response;
        }
    }
}
