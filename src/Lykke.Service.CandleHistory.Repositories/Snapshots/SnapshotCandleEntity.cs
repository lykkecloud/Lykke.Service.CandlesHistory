﻿using System;
using Lykke.Job.CandlesProducer.Contract;
using Lykke.Service.CandlesHistory.Core.Domain.Candles;
using MessagePack;

namespace Lykke.Service.CandleHistory.Repositories.Snapshots
{
    [MessagePackObject]
    public class SnapshotCandleEntity : ICandle
    {
        [Key(0)]
        public string AssetPairId { get; set; }

        [Key(1)]
        public CandlePriceType PriceType { get; set; }

        [Key(2)]
        public CandleTimeInterval TimeInterval { get; set; }

        [Key(3)]
        public DateTime Timestamp { get; set; }

        [Key(4)]
        public decimal Open { get; set; }

        [Key(5)]
        public decimal Close { get; set; }

        [Key(6)]
        public decimal High { get; set; }

        [Key(7)]
        public decimal Low { get; set; }

        [Key(8)]
        public decimal TradingVolume { get; set; }

        double ICandle.Open => (double) Open;

        double ICandle.Close => (double) Close;

        double ICandle.High => (double) High;

        double ICandle.Low => (double) Low;

        double ICandle.TradingVolume => (double) TradingVolume;

        public static SnapshotCandleEntity Copy(ICandle candle)
        {
            return new SnapshotCandleEntity
            {
                AssetPairId = candle.AssetPairId,
                PriceType = candle.PriceType,
                TimeInterval = candle.TimeInterval,
                Timestamp = candle.Timestamp,
                Open = ConvertDouble(candle.Open),
                Close = ConvertDouble(candle.Close),
                Low = ConvertDouble(candle.Low),
                High = ConvertDouble(candle.High),
                TradingVolume = ConvertDouble(candle.TradingVolume)
            };
        }

        private static decimal ConvertDouble(double d)
        {
            try
            {
                return Convert.ToDecimal(d);
            }
            catch (OverflowException)
            {
                return d > 0 ? decimal.MaxValue : decimal.MinValue;
            }
        }
    }
}
