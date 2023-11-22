using SkbKontur.DbViewer.Connector;
using SkbKontur.DbViewer.EntityFramework;

namespace DbViewerExample.EntityFramework;

public class EntityFrameworkDbConnectorFactory : IDbConnectorFactory
{
    private readonly Func<EntityFrameworkDbContext> _createContext;

    public EntityFrameworkDbConnectorFactory(Func<EntityFrameworkDbContext> createContext)
    {
        _createContext = createContext;
    }

    public IDbConnector CreateConnector<T>() where T : class
    {
        return new EntityFrameworkDbConnector<T>(_createContext);
    }
}