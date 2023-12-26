
namespace BinDb;

public class CreateConnectionCommandHandler : CommandHandler
{
    public CreateConnectionCommandHandler(Env env) : base("create connection", env)
    {
    }

    public override Task HandleAsync(string command)
    {
        var connectionName = ReadConnectionName();
        if (connectionName == null)
        {
	    Env.Exit = true;
            return Task.CompletedTask;
        }

        var connectionType = ReadConnectionType();
        if (connectionType == null)
        {
            Env.Exit = true;
            return Task.CompletedTask;
        }

        var connectionString = ReadConnectionString();
        if (connectionString == null)
        {
            Env.Exit = true;
            return Task.CompletedTask;
        }

        Env.Connections.Add(new()
        {
            Name = connectionName,
            ConnectionType = connectionType.Value,
            ConnectionString = connectionString
        });
        Env.WriteEnv();
        Console.WriteLine($"Create connection success!");
        return Task.CompletedTask;
    }

    private string? ReadConnectionName()
    {
        while (true)
        {
	    if (!ReadConsoleLine("Please set Connection name!", out var input))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Connection name cannot be empty!");
                continue;
            }

            if (Env.Connections.Any(p => p.Name == input))
            {
                Console.WriteLine("Already exist connection name, please set other one!");
                continue;
	    }

            return input;
        }
    }

    private DbConnectionType? ReadConnectionType()
    {
        while (true)
        {
            if (!ReadConsoleLine("Please select db type! [(1-sqlserver) (2-mysql) (3-sqllite)]", out var input))
            {
                return null;
            }
	    
            int.TryParse(input, out int dbTypeInt);
            var dbType = (DbConnectionType)dbTypeInt;
            if (dbType == DbConnectionType.Unknow)
            {
                Console.WriteLine("Wrong db type!");
                continue;
            }
            return dbType;
        }
    }

    private string? ReadConnectionString()
    {
        while (true)
        {
            if (!ReadConsoleLine("Please set db connection!",out var input))
	    {
                return null;
            }

            return input;
        }
    }
}
