namespace BinDb;

public class Env
{
    public static string EnvFile = $"{Environment.CurrentDirectory}/env.json";

    public static Env Instance{ get; set; }

    [JsonIgnore]
    public bool Exit{ get; set; }

    public List<DbConnection> Connections { get; set; } = new();

    public DbConnection? CurrentConnectionInfo{ get; set; }

    [JsonIgnore]
    public Dictionary<string, DapperContext> DapperContextDict { get; set; } = new();

    public void WriteEnv()
    {
        File.WriteAllText(EnvFile, JsonConvert.SerializeObject(this));
    }

    public static Env ReadEnv()
    {
        if (!File.Exists(EnvFile))
        {
            return new Env();
        }
	
        var data = File.ReadAllText(EnvFile);
        return JsonConvert.DeserializeObject<Env>(data)??new Env();
    }
}
