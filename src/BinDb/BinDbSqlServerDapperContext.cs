
namespace BinDb;

public class BinDbSqlServerDapperContext : SqlServerDapperContext
{
    private readonly string _connectionString;

    public BinDbSqlServerDapperContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void SetOptions(DapperOptions options)
    {
        options.ConnectString = Env.Instance.CurrentConnectionInfo.ConnectionString;
    }
}
