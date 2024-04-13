using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ApiIsolated
{
    public class Program
    {
        public static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((hostContext, services) =>
                {
                    var conn = hostContext.Configuration["AzureWebJobsStorage"];
                    services.AddAzureClients(c =>
                    {
                        c.AddClient<TableClient, TableClientOptions>((options, cred, client) =>
                        {
                            var tableClient = new TableClient(conn, hostContext.Configuration["AzureStorageTableName"], options);
                            tableClient.CreateIfNotExists();
                            return tableClient;
                        });
                        c.AddClient<BlobContainerClient, BlobClientOptions>((options, cred, client) =>
                        {
                            var blobClient = new BlobContainerClient(conn, hostContext.Configuration["AzureStorageBlobName"], options);
                            blobClient.CreateIfNotExists();
                            return blobClient;
                        });
                    });
                })
                .Build();

            await host.RunAsync();
        }
    }
}