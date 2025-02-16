using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace _Scripts.Core.SettingsCore
{
    public class TypedDictionaryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Dictionary<string, object>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var dictionary = new Dictionary<string, object>();
            foreach (var property in jsonObject.Properties())
            {
                var valueToken = property.Value["value"];
                var typeToken = property.Value["type"];
                if (valueToken == null || typeToken == null)
                {
                    throw new JsonSerializationException($"Invalid format for key '{property.Name}'. Expected 'value' and 'type' properties.");
                }

                var typeName = typeToken.Value<string>();
                var propertyType = Type.GetType(typeName);
                if (propertyType == null)
                {
                    throw new JsonSerializationException($"Type '{typeName}' not found.");
                }

                var value = valueToken.ToObject(propertyType, serializer);
                dictionary[property.Name] = value;
            }

            return dictionary;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var dictionary = (Dictionary<string, object>)value;
            var jsonObject = new JObject();
            foreach (var kvp in dictionary)
            {
                var valueToken = JToken.FromObject(kvp.Value, serializer);
                var typeToken = new JValue(kvp.Value.GetType().AssemblyQualifiedName);
                var propertyObject = new JObject
                {
                    ["value"] = valueToken,
                    ["type"] = typeToken
                };

                jsonObject[kvp.Key] = propertyObject;
            }

            jsonObject.WriteTo(writer);
        }
    }
}