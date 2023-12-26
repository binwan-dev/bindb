
namespace BinDb;

public class ScriptCommandHandler : CommandHandler
{
    public ScriptCommandHandler(Env env) : base("script", env)
    {
    }

    public override async Task HandleAsync(string command)
    {
        if (Env.CurrentConnectionInfo == null)
        {
            Console.WriteLine("Plase use connection first!");
            return;
        }

        while (true)
        {
	    Console.WriteLine($"$ {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} Db: {Env.CurrentConnectionInfo.Name} {Env.CurrentConnectionInfo.ConnectionType.ToString()}");
	    Console.Write("Script > ");
            var script = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(script))
            {
                continue;
            }
            if (script == "exit")
            {
                return;
            }

            await ExecuteScriptAsync(script);
        }
    }

    private async Task ExecuteScriptAsync(string script)
    {
        var dapperContext = Env.CurrentConnectionInfo.GetDapperContext();
        if (script.StartsWith("select"))
        {
            await ExecuteQueryAsync(dapperContext, script);
        }
    }

    private async Task ExecuteQueryAsync(DapperContext dapperContext, string script)
    {
        var reader = await dapperContext.ExecuteReaderAsync(script);

        for (var i = 0; i < reader.FieldCount; i++)
        {
            Console.Write($"| {reader.GetName(i)} ");
        }
        Console.Write("\n");

        while (reader.Read())
        {
            for (var j= 0; j < reader.FieldCount; j++)
	    {
                Console.Write($"| {reader[j]}");
            }
            Console.Write("\n");
        }
    }
}
