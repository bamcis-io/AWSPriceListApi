using BAMCIS.AWSPriceListApi.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

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
            JObject obj = JObject.Load(reader);

            if (reader.TokenType == JsonToken.Null)
            {
                return new TermAttributes(0, PurchaseOption.ON_DEMAND, OfferingClass.STANDARD);
            }

            if (reader.TokenType != JsonToken.Null)
            {
                if (!obj.HasValues)
                {
                    return new TermAttributes(0, PurchaseOption.ON_DEMAND, OfferingClass.STANDARD);
                }
            }

            obj.TryGetValue("LeaseContractLength", StringComparison.OrdinalIgnoreCase, out JToken leaseToken);
            obj.TryGetValue("OfferingClass", StringComparison.OrdinalIgnoreCase, out JToken offeringClassToken);
            obj.TryGetValue("PurchaseOption", StringComparison.OrdinalIgnoreCase, out JToken purchaseOptionToken);

            OfferingClass offeringClass = OfferingClass.STANDARD;
            PurchaseOption option = PurchaseOption.ON_DEMAND;
            int lease = 0;

            if (offeringClassToken != null)
            {
                offeringClass = serializer.Deserialize<OfferingClass>(offeringClassToken.CreateReader());
            }

            if (purchaseOptionToken != null)
            {
                option = serializer.Deserialize<PurchaseOption>(purchaseOptionToken.CreateReader());
            }

            if (leaseToken != null)
            {
                JsonSerializer jsonSerializer = new JsonSerializer()
                {
                    Converters = { new LeaseContractLengthConverter() }
                };

                lease = jsonSerializer.Deserialize<int>(leaseToken.CreateReader());
            }

            return new TermAttributes(lease, option, offeringClass);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        #endregion
    }
}
