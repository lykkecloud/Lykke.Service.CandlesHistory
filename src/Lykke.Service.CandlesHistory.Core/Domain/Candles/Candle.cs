using System;
using Lykke.Domain.Prices;

namespace Lykke.Service.CandlesHistory.Core.Domain.Candles
{
    public class Candle : ICandle
    {
        public string AssetPairId { get; set; }
        public PriceType PriceType { get; set; }
        public TimeInterval TimeInterval { get; set; }
        public DateTime Timestamp { get; set; }
        public double Open { get; set; }
        public double Close { get; set; }
        public double High { get; set; }
        public double Low { get; set; }

        public static Candle Create(ICandle candle)
        {
            return new Candle
            {
                AssetPairId = candle.AssetPairId,
                PriceType = candle.PriceType,
                TimeInterval = candle.TimeInterval,
                Timestamp = candle.Timestamp,
                Open = candle.Open,
                Close = candle.Close,
                Low = candle.Low,
                High = candle.High
            };
        }
    }
}