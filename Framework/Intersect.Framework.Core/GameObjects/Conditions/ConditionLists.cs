﻿using Intersect.Framework.Core.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Intersect.GameObjects.Conditions;

[JsonConverter(typeof(ConditionListsSerializer))]
public partial class ConditionLists
{
    public List<ConditionList> Lists { get; set; } = [];

    public ConditionLists()
    {
    }

    public ConditionLists(string data)
    {
        Load(data);
    }

    public int Count => Lists?.Count ?? 0;

    public void Load(string data)
    {
        Lists.Clear();
        JsonConvert.PopulateObject(
            data,
            Lists,
            new JsonSerializerSettings()
            {
                SerializationBinder = new IntersectTypeSerializationBinder(),
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
            }
        );
    }

    public string Data()
    {
        return JsonConvert.SerializeObject(
            Lists,
            new JsonSerializerSettings()
            {
                SerializationBinder = new IntersectTypeSerializationBinder(),
                TypeNameHandling = TypeNameHandling.Auto,
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
            }
        );
    }
}

public partial class ConditionListsSerializer : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return true;
    }

    public override object ReadJson(
        JsonReader reader,
        Type objectType,
        object existingValue,
        JsonSerializer serializer
    )
    {
        var jsonObject = JObject.Load(reader);
        var properties = jsonObject.Properties().ToList();
        var lists = existingValue != null ? (ConditionLists) existingValue : new ConditionLists();
        lists.Load((string) properties[0].Value);

        return lists;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Lists");
        serializer.Serialize(writer, ((ConditionLists)value).Data());
        writer.WriteEndObject();
    }
}
