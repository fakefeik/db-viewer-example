using System.Reflection;
using GroBuf;
using GroBuf.DataMembersExtracters;
using SkbKontur.DbViewer.Configuration;

namespace DbViewerExample.EntityFramework;

public class CustomPropertyConfigurationProvider : ICustomPropertyConfigurationProvider
{
    private static readonly ISerializer Serializer = new Serializer(new AllPropertiesExtractor());

    public CustomPropertyConfiguration? TryGetConfiguration(object @object, PropertyInfo propertyInfo)
    {
        return TryGetConfiguration(propertyInfo);
    }

    public CustomPropertyConfiguration? TryGetConfiguration(PropertyInfo propertyInfo)
    {
        var serializedAttribute = propertyInfo.GetCustomAttribute<SerializedAttribute>();
        if (serializedAttribute == null)
            return null;

        return new CustomPropertyConfiguration
        {
            ResolvedType = serializedAttribute.Type,
            StoredToApi = @object => Serializer.Deserialize(serializedAttribute.Type, (byte[]?)@object),
            ApiToStored = @object => Serializer.Serialize(serializedAttribute.Type, @object)
        };
    }

    public CustomPropertyConfiguration? TryGetConfiguration(Type propertyType)
    {
        return null;
    }
}