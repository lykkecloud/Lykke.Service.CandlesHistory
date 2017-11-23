﻿using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.CandlesHistory.Core.Domain;
using Lykke.Service.CandlesHistory.Core.Services;

namespace Lykke.Service.CandlesHistory.Services
{
    public class SnapshotSerializer : ISnapshotSerializer
    {
        private readonly ILog _log;

        public SnapshotSerializer(ILog log)
        {
            _log = log;
        }

        public async Task SerializeAsync<TState>(IHaveState<TState> stateHolder, ISnapshotRepository<TState> repository)
        {
            await SerializeAsync(_log.CreateComponentScope($"{nameof(SnapshotSerializer)}[{stateHolder.GetType().Name}]"), stateHolder, repository);
        }

        public async Task<bool> DeserializeAsync<TState>(IHaveState<TState> stateHolder, ISnapshotRepository<TState> repository)
        {
            return await DeserializeAsync(_log.CreateComponentScope($"{nameof(SnapshotSerializer)}[{stateHolder.GetType().Name}]"), stateHolder, repository);
        }

        private static async Task SerializeAsync<TState>(ILog log, IHaveState<TState> stateHolder, ISnapshotRepository<TState> repository)
        {
            await log.WriteInfoAsync(nameof(SerializeAsync), "", "Gettings state...");

            var state = stateHolder.GetState();

            await log.WriteInfoAsync(nameof(SerializeAsync), stateHolder.DescribeState(state), "Saving state...");

            await repository.SaveAsync(state);

            await log.WriteInfoAsync(nameof(SerializeAsync), "", "State saved");
        }

        private async Task<bool> DeserializeAsync<TState>(ILog log, IHaveState<TState> stateHolder, ISnapshotRepository<TState> repository)
        {
            await log.WriteInfoAsync(nameof(DeserializeAsync), "", "Loading state...");

            var state = await repository.TryGetAsync();

            if (state == null)
            {
                await log.WriteWarningAsync("SnapshotSerializer", nameof(DeserializeAsync),
                    stateHolder.GetType().Name, "No snapshot found to deserialize");

                return false;
            }

            await log.WriteInfoAsync(nameof(DeserializeAsync), stateHolder.DescribeState(state), "Settings state...");

            stateHolder.SetState(state);

            await log.WriteInfoAsync(nameof(DeserializeAsync), "", "State was set");

            return true;
        }
    }
}