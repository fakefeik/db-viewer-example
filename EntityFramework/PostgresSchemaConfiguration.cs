using SkbKontur.DbViewer.Configuration;
using SkbKontur.DbViewer.Connector;
using SkbKontur.DbViewer.DataTypes;
using SkbKontur.DbViewer.EntityFramework;
using SkbKontur.DbViewer.Schemas;

namespace DbViewerExample.EntityFramework;

public class PostgresSchemaConfiguration : ISchemaConfiguration
{
    public PostgresSchemaConfiguration(EntityFrameworkDbConnectorFactory connectorFactory)
    {
        ConnectorsFactory = connectorFactory;
        PropertyDescriptionBuilder =
            new EntityFrameworkPropertyDescriptionBuilder<IdentityAttribute, IndexedAttribute>();
    }

    public SchemaDescription Description => new()
    {
        AllowReadAll = true,
        CountLimit = 50_000,
        CountLimitForSuperUser = 1_000_000,
        DownloadLimit = 50_000,
        DownloadLimitForSuperUser = 1_000_000,
        SchemaName = "Postgres Objects",
        AllowDelete = true,
        AllowEdit = true,
        AllowSort = true
    };

    public TypeDescription[] Types => EntityFrameworkDbContext.EntityTypes.ToArray().ToTypeDescriptions();

    public IDbConnectorFactory ConnectorsFactory { get; }
    public IPropertyDescriptionBuilder PropertyDescriptionBuilder { get; }
}