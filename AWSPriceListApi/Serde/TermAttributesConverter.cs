using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMCIS.AWSPriceListApi.Serde
{
    public class TermAttributesConverter : JsonConverter
    {
        #region Public Properties

        public override bool CanRead => true;

        public override bool CanWrite => false;

        #endregion

        #region Public Methods

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject Obj = JObject.Load(reader);

            if (reader.TokenType == JsonToken.Null)
            {
                return new TermAttributes(0, PurchaseOption.ON_DEMAND, OfferingClass.STANDARD);
            }

            if (reader.TokenType != JsonToken.Null)
            {
                if (!Obj.HasValues)
                {
                    return new TermAttributes(0, PurchaseOption.ON_DEMAND, OfferingClass.STANDARD);
                }
            }

            Obj.TryGetValue("LeaseContractLength", StringComparison.OrdinalIgnoreCase, out JToken LeaseToken);
            Obj.TryGetValue("OfferingClass", StringComparison.OrdinalIgnoreCase, out JToken OfferingClassToken);
            Obj.TryGetValue("PurchaseOption", StringComparison.OrdinalIgnoreCase, out JToken PurchaseOptionToken);

            OfferingClass Class = OfferingClass.STANDARD;
            PurchaseOption Option = PurchaseOption.ON_DEMAND;
            int Lease = 0;

            if (OfferingClassToken != null)
            {
                Class = serializer.Deserialize<OfferingClass>(OfferingClassToken.CreateReader());
            }

            if (PurchaseOptionToken != null)
            {
                Option = serializer.Deserialize<PurchaseOption>(PurchaseOptionToken.CreateReader());
            }

            if (LeaseToken != null)
            {
                JsonSerializer Serde = new JsonSerializer()
                {
                    Converters = { new LeaseContractLengthConverter() }
                };

                Lease = Serde.Deserialize<int>(LeaseToken.CreateReader());
            }

            return new TermAttributes(Lease, Option, Class);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        #endregion
    }
}
