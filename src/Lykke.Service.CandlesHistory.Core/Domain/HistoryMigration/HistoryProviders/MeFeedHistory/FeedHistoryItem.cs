﻿using System;
using Lykke.Job.CandlesProducer.Contract;
using Lykke.Service.CandlesHistory.Core.Domain.Candles;

namespace Lykke.Service.CandlesHistory.Core.Domain.HistoryMigration.HistoryProviders.MeFeedHistory
{
    public class FeedHistoryItem
    {
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public int Tick { get; set; }

        public ICandle ToCandle(string assetPairId, CandlePriceType priceType, DateTime baseTime)
        {
            return new Candle(
                open: Open,
                close: Close,
                high: High,
                low: Low,
                assetPair: assetPairId,
                priceType: priceType,
                timeInterval: CandleTimeInterval.Sec,
                timestamp: baseTime.AddSeconds(Tick),
                tradingVolume: 0);
        }
    }
}
