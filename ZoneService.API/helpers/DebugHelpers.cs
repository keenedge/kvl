using System.Text.Json;
using Serilog;

public class DebugHelpers
{
    public static void LogObject( Object o, Serilog.Events.LogEventLevel level = Serilog.Events.LogEventLevel.Information)
    {
        var json = JsonSerializer.Serialize( o );
        //string json = Newtonsoft.Json.JsonConvert.SerializeObject(o,
            // new Newtonsoft.Json.JsonSerializerSettings
            // {
            //     NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
            //     Formatting = Newtonsoft.Json.Formatting.Indented
            // }
        //);
        json.Split("\r\n").ToList().ForEach((s) =>
        {
            Log.Write(level, "{message}", s);
        });

    }
}