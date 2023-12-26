
namespace BinDb;

public class UseConnectionCommandHandler : CommandHandler
{
    public UseConnectionCommandHandler(Env env) : base("use connection", env)
    {
    }

    public override Task HandleAsync(string command)
    {
        var selectConnection = ReadSelectConnection();
        if (selectConnection == null)
        {
            return Task.CompletedTask;
        }

        Env.CurrentConnectionInfo = selectConnection;
        return Task.CompletedTask;
    }

    private DbConnection? ReadSelectConnection()
    {
	var index = 1;
        foreach (var item in Env.Connections)
        {
            Console.WriteLine($"{index++} {item.Name}");
        }
	
        while (true)
        {
            if (!ReadConsoleLine("Please select db connection!", out var input))
            {
                Env.Exit = true;
                return null;
            }

            if (!int.TryParse(input, out var idx) || idx > Env.Connections.Count || idx < 1)
            {
                Console.WriteLine("Invalid index!");
                continue;
            }

            return Env.Connections.ElementAt(idx - 1);
        }
    }
}
