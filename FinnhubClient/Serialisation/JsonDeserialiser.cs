﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InvestmentSimulator.Finnhub.Client.Serialisation
{
    public class JsonDeserialiser : IDeserialiser
    {
        internal static readonly JsonDeserialiser Default = new JsonDeserialiser();

        private readonly JsonSerializer _serializer = new JsonSerializer();

        public virtual async Task<TResponse> Deserialize<TResponse>(HttpContent responseContent)
        {
            using (var contentStream = await responseContent.ReadAsStreamAsync().ConfigureAwait(false))
            using (var streamReader = new StreamReader(contentStream))
            {
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    return _serializer.Deserialize<TResponse>(reader);
                }
            }
        }
    }
}