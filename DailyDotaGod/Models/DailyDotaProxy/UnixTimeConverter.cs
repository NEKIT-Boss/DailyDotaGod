using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDotaGod.Models.DailyDotaProxy
{
    class UnixTimeConverter : JsonConverter
    {
        private static readonly DateTime EpochStart =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long timestamp = long.Parse(reader.Value.ToString());
            return EpochStart.AddSeconds(timestamp).ToLocalTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
