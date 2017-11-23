﻿using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.CandlesHistory.Services.Settings
{
    public class DbSettings
    {
        public string LogsConnectionString { get; set; }

        public string SnapshotsConnectionString { get; set; }

        [Optional]
        public string FeedHistoryConnectionString { get; set; }
    }
}