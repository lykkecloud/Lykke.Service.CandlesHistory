﻿using System;
using Lykke.Service.CandlesHistory.Core.Services;
using Lykke.Service.CandlesHistory.Models.IsAlive;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.SwaggerGen.Annotations;

namespace Lykke.Service.CandlesHistory.Controllers
{
    /// <summary>
    /// Controller to test service is alive
    /// </summary>
    [Route("api/[controller]")]
    public class IsAliveController : Controller
    {
        private readonly IHealthService _healthService;
        private readonly IShutdownManager _shutdownManager;

        public IsAliveController(IHealthService healthService, IShutdownManager shutdownManager)
        {
            _healthService = healthService;
            _shutdownManager = shutdownManager;
        }

        /// <summary>
        /// Checks service is alive
        /// </summary>
        [HttpGet]
        [SwaggerOperation("IsAlive")]
        public IsAliveResponse Get()
        {
            return new IsAliveResponse
            {
                Name = PlatformServices.Default.Application.ApplicationName,
                Version = PlatformServices.Default.Application.ApplicationVersion,
                Env = Environment.GetEnvironmentVariable("ENV_INFO"),
                BatchesToPersistQueueLength = _healthService.BatchesToPersistQueueLength,
                CandlesToDispatchQueueLength = _healthService.CandlesToDispatchQueueLength,
                AveragePersistTime = _healthService.AveragePersistTime,
                AverageCandlesPersistedPersSecond = _healthService.AverageCandlesPersistedPerSecond,
                TotalPersistTime = _healthService.TotalPersistTime,
                TotalCandlesPersistedCount = _healthService.TotalCandlesPersistedCount,
                IsShuttingDown = _shutdownManager.IsShuttingDown,
                IsShuttedDown = _shutdownManager.IsShuttedDown
            };
        }
    }
}