// Copyright (c) 2019 Lykke Corp.
// See the LICENSE file in the project root for more information.

namespace Lykke.Service.CandlesHistory.Client.Models
{
    using Lykke.Service;
    using Lykke.Service.CandlesHistory;
    using Lykke.Service.CandlesHistory.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class PersistenceInfo
    {
        /// <summary>
        /// Initializes a new instance of the PersistenceInfo class.
        /// </summary>
        public PersistenceInfo()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the PersistenceInfo class.
        /// </summary>
        public PersistenceInfo(long totalCandlesPersistedCount, long totalCandleRowsPersistedCount, int batchesToPersistQueueLength, int candlesToDispatchQueueLength, Times times = default(Times), Throughput throughput = default(Throughput))
        {
            Times = times;
            Throughput = throughput;
            TotalCandlesPersistedCount = totalCandlesPersistedCount;
            TotalCandleRowsPersistedCount = totalCandleRowsPersistedCount;
            BatchesToPersistQueueLength = batchesToPersistQueueLength;
            CandlesToDispatchQueueLength = candlesToDispatchQueueLength;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Times")]
        public Times Times { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Throughput")]
        public Throughput Throughput { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TotalCandlesPersistedCount")]
        public long TotalCandlesPersistedCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "TotalCandleRowsPersistedCount")]
        public long TotalCandleRowsPersistedCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BatchesToPersistQueueLength")]
        public int BatchesToPersistQueueLength { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CandlesToDispatchQueueLength")]
        public int CandlesToDispatchQueueLength { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Times != null)
            {
                Times.Validate();
            }
            if (Throughput != null)
            {
                Throughput.Validate();
            }
        }
    }
}
