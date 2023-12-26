var env = Env.ReadEnv();
Env.Instance = env;

var services = new ServiceCollection()
.AddLogging()
.AddSingleton<Env>(env)
.AddSingleton<CreateConnectionCommandHandler>()
.AddSingleton<UseConnectionCommandHandler>()
.AddSingleton<ScriptCommandHandler>()
.AddSqlServerDapper<BinDbSqlServerDapperContext>(b =>
{
    b.UseSqlServer<BinDbSqlServerDapperContext>("sss");
});

var provider = services.BuildServiceProvider();
env = provider.GetService<Env>()??throw new ArgumentNullException(nameof(Env));
var createConnectionCommandHandler = provider.GetService<CreateConnectionCommandHandler>()??throw new ArgumentNullException(nameof(CreateConnectionCommandHandler));
var useConnectionCommandHandler = provider.GetService<UseConnectionCommandHandler>()??throw new ArgumentNullException(nameof(UseConnectionCommandHandler));
var scriptCommandHandler = provider.GetService<ScriptCommandHandler>()??throw new ArgumentNullException(nameof(ScriptCommandHandler));

var commandHandlerDict = new Dictionary<string,CommandHandler>();
commandHandlerDict.Add(createConnectionCommandHandler.Identifier,createConnectionCommandHandler);
commandHandlerDict.Add(useConnectionCommandHandler.Identifier,useConnectionCommandHandler);
commandHandlerDict.Add(scriptCommandHandler.Identifier,scriptCommandHandler);

Console.WriteLine("Welcome use BinDb! Have Fun ^_^!");

while(!env.Exit)
{
    if(env.CurrentConnectionInfo==null)
    {
	Console.WriteLine($"$ {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}");
	Console.Write("> ");
    }
    else
    {
	Console.WriteLine($"$ {DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} Db: {env.CurrentConnectionInfo.Name} {env.CurrentConnectionInfo.ConnectionType.ToString()}");
	Console.Write("> ");
    }
    
    var command = Console.ReadLine();
    if(string.IsNullOrWhiteSpace(command))
    {
	continue;
    }
    
    if(command == "quit" || command=="exit")
    {
	env.Exit=true;
	break;
    }

    if(!commandHandlerDict.TryGetValue(command,out var commandHandler))
    {
	Console.WriteLine("Wrong command!");
	continue;
    }

    await commandHandler.HandleAsync(command);
    if(env.Exit)
    {
	break;
    }
}

Console.WriteLine("Goodbye!");
