namespace BinDb;

public class DbConnection
{
    private DapperContext _dapperContext;

    public string Name { get; set; } = string.Empty;

    public string ConnectionString { get; set; } = string.Empty;

    public DbConnectionType ConnectionType{ get; set; }

    public List<string> SqlScripts { get; set; } = new();

    public DapperContext GetDapperContext()
    {
        return _dapperContext ??= CreateDapperContext();
    }

    private DapperContext CreateDapperContext()
    {
        switch (ConnectionType)
        {
	    case DbConnectionType.SqlServer:
                return new BinDbSqlServerDapperContext(ConnectionString);
	    default:
                throw new NotSupportedException("Not support db type!");
        }
    }
}
