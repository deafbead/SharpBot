﻿using Newtonsoft.Json;

namespace TelegramBot.API.Models
{
    /// <summary>
    /// This object represents a point on the map
    /// </summary>
    public class Location
    {
        /// <summary>
        /// Longitude as defined by sender
        /// </summary>
        [JsonProperty("longitude")]
        public float Longitude { get; set; }

        /// <summary>
        /// Latitude as defined by sender
        /// </summary>
        [JsonProperty("latitude")]
        public float Latitude { get; set; }
    }
}
