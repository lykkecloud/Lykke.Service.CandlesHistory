﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Domain.Prices;
using Lykke.Service.CandlesHistory.Core.Domain.Candles;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace Lykke.Service.CandleHistory.Repositories
{
    public class FeedBidAskHistoryEntity : ITableEntity
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string ETag { get; set; }
        public string AssetPair => PartitionKey;
        public CandleHistoryItem[] BidCandles { get; set; } = Array.Empty<CandleHistoryItem>();
        public CandleHistoryItem[] AskCandles { get; set; } = Array.Empty<CandleHistoryItem>();

        public DateTime DateTime
        {
            get
            {
                if (!string.IsNullOrEmpty(RowKey) && DateTime.TryParseExact(RowKey, "s", DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out var date))
                    return date;

                return default(DateTime);
            }
        }

        public static string GeneratePartitionKey(string assetPair)
        {
            return assetPair;
        }

        public static string GenerateRowKey(DateTime date)
        {
            return date.ToString("s");
        }

        public static FeedBidAskHistoryEntity Create(string assetPair, DateTime date, List<ICandle> askCandles, List<ICandle> bidCandles)
        {
            var entity = new FeedBidAskHistoryEntity
            {
                PartitionKey = GeneratePartitionKey(assetPair),
                RowKey = GenerateRowKey(date),
            };

            if (askCandles != null && askCandles.Any())
                entity.AskCandles = askCandles.Select(item => item.ToItem(TimeInterval.Sec)).ToArray();

            if (bidCandles != null && bidCandles.Any())
                entity.BidCandles = bidCandles.Select(item => item.ToItem(TimeInterval.Sec)).ToArray();

            return entity;
        }

        public IFeedBidAskHistory ToDomain()
        {
            return new FeedBidAskHistory
            {
                AssetPair = AssetPair,
                DateTime = DateTime,
                AskCandles = AskCandles.Select(item => item.ToCandle(AssetPair, PriceType.Ask, DateTime, TimeInterval.Sec)).ToArray(),
                BidCandles = BidCandles.Select(item => item.ToCandle(AssetPair, PriceType.Bid, DateTime, TimeInterval.Sec)).ToArray()
            };
        }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            EntityProperty property;

            if (properties.TryGetValue("AskCandles", out property))
            {
                var json = property.StringValue;
                if (!string.IsNullOrEmpty(json))
                {
                    AskCandles = JsonConvert.DeserializeObject<CandleHistoryItem[]>(json);
                }
            }

            if (properties.TryGetValue("BidCandles", out property))
            {
                var json = property.StringValue;
                if (!string.IsNullOrEmpty(json))
                {
                    BidCandles = JsonConvert.DeserializeObject<CandleHistoryItem[]>(json);
                }
            }
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            var askCandles = JsonConvert.SerializeObject(AskCandles);
            var bidCandles = JsonConvert.SerializeObject(BidCandles);

            var dict = new Dictionary<string, EntityProperty>
            {
                {"AskCandles", new EntityProperty(askCandles)},
                {"BidCandles", new EntityProperty(bidCandles)}
            };

            return dict;
        }
    }

    public class FeedBidAskHistoryRepository : IFeedBidAskHistoryRepository
    {
        private readonly INoSQLTableStorage<FeedBidAskHistoryEntity> _tableStorage;

        public FeedBidAskHistoryRepository(INoSQLTableStorage<FeedBidAskHistoryEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task AddHistoryItemAsync(string assetPair, DateTime date, List<ICandle> askCandles, List<ICandle> bidCandles)
        {
            await _tableStorage.InsertOrReplaceAsync(FeedBidAskHistoryEntity.Create(assetPair, date, askCandles, bidCandles));
        }

        public Task GetHistoryByChunkAsync(string assetPair, DateTime startDate, DateTime endDate, Func<IEnumerable<IFeedBidAskHistory>, Task> chunkCallback)
        {
            return _tableStorage.GetDataByChunksAsync(FeedBidAskHistoryEntity.GeneratePartitionKey(assetPair), async chunk =>
            {
                var yieldResult = new List<IFeedBidAskHistory>();

                foreach (var historyItem in chunk.Where(item => item.DateTime >= startDate && item.DateTime <= endDate))
                {
                    yieldResult.Add(historyItem.ToDomain());
                }

                if (yieldResult.Count > 0)
                {
                    await chunkCallback(yieldResult);
                    yieldResult.Clear();
                }
            });
        }
    }
}
