namespace BinDb;

public abstract class CommandHandler
{
    private readonly string _identifier;
    protected readonly Env Env;
    private string v;

    public CommandHandler(string identifier, Env env)
    {
        _identifier = identifier;
        Env = env;
    }

    public string Identifier => _identifier;

    public abstract Task HandleAsync(string command);

    protected bool ReadConsoleLine(string tips, out string input)
    {
	Console.WriteLine(tips);
	Console.Write("> ");
        input = Console.ReadLine() ?? string.Empty;
        if (input == "exit")
        {
            return false;
        }
        return true;
    }
}
