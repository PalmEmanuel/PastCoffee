using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api
{
    public class CoffeeEntryEntity : ITableEntity
    {
        public string ImagePath { get; set; }
        public string CoffeeBeans { get; set; }
        public string TasteNotes { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }
        public int Rating { get; set; }
        public int BrewTime { get; set; }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
