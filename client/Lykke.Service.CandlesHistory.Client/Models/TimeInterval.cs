﻿// Code generated by Microsoft (R) AutoRest Code Generator 1.0.1.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.CandlesHistory.Client.Models
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for TimeInterval.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeInterval
    {
        [EnumMember(Value = "Unspecified")]
        Unspecified,
        [EnumMember(Value = "Sec")]
        Sec,
        [EnumMember(Value = "Minute")]
        Minute,
        [EnumMember(Value = "Min5")]
        Min5,
        [EnumMember(Value = "Min15")]
        Min15,
        [EnumMember(Value = "Min30")]
        Min30,
        [EnumMember(Value = "Hour")]
        Hour,
        [EnumMember(Value = "Hour4")]
        Hour4,
        [EnumMember(Value = "Hour6")]
        Hour6,
        [EnumMember(Value = "Hour12")]
        Hour12,
        [EnumMember(Value = "Day")]
        Day,
        [EnumMember(Value = "Week")]
        Week,
        [EnumMember(Value = "Month")]
        Month
    }
}