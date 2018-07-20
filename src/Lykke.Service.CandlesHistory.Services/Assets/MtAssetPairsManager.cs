﻿using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using MarginTrading.SettingsService.Contracts;
using Polly;
using System;
using Lykke.Service.CandlesHistory.Core.Services.Assets;

namespace Lykke.Service.CandlesHistory.Services.Assets
{
    public class MtAssetPairsManager : IAssetPairsManager
    {
        private readonly IAssetPairsApi _apiService;
        private readonly ILog _log;

        public MtAssetPairsManager(ILog log, IAssetPairsApi apiService)
        {
            _apiService = apiService;
            _log = log;
        }

        public async Task<AssetPair> TryGetEnabledPairAsync(string assetPairId)
        {
            var pair = await _apiService.Get(assetPairId);

            return pair == null ? null : MapAssetPair(pair);


        }

        public Task<AssetPair> TryGetAssetPairAsync(string assetPairId)
        {
            return Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timespan) => _log.WriteErrorAsync("Get asset pair with retry", assetPairId, exception))
                .ExecuteAsync(() => TryGetEnabledPairAsync(assetPairId));
        }


        public Task<IEnumerable<AssetPair>> GetAllEnabledAsync()
        {

            return Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timespan) => _log.WriteErrorAsync("Get all asset pairs with retry", string.Empty, exception))
                .ExecuteAsync(async () => (await _apiService.List()).Select(pair => MapAssetPair(pair)));
        }

        public AssetPair MapAssetPair(MarginTrading.SettingsService.Contracts.AssetPair.AssetPairContract pair)
        {
            return new AssetPair
            {
                Id = pair.Id,
                Name = pair.Name,
                BaseAssetId = pair.BaseAssetId,
                QuotingAssetId = pair.QuoteAssetId,
                Accuracy = pair.Accuracy,
                InvertedAccuracy = pair.Accuracy
            };
        }

    }
}