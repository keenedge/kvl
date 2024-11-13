using System.Text;
using System.Net;
using Serilog;
using Microsoft.Extensions.Options;
using System.Text.Json;

using Zone.Models;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json.Serialization;
using Alpha.Services;

namespace Zone.Services
{


    /// <summary/>
    public sealed class ZoneManagerService : IZoneManagerService
    {
        private static HttpClient httpClient = new HttpClient();

        string alphaServiceUrl = "";
        IConfigRepoService _configRepoService;
        IWebHostEnvironment _hostingEnvironment = null;

        
        IThinkLogicalManagerService _thinkLogicalManagerService;
        IWindowManagerService _WindowManagerService;
        readonly Serilog.ILogger _log = Log.ForContext<ZoneManagerService>();
        KVLConfiguration _kvlConfiguration = null;
        /// <summary/>
        public ZoneManagerService(
            IConfigRepoService configRepoService,
            IThinkLogicalManagerService thinkLogcicalManagerService,
            IWindowManagerService windowManagerService,
            IOptions<KVLConfiguration> kvlConfig,
            IWebHostEnvironment hostingEnvironment
        )
        {
            _configRepoService = configRepoService;
            _thinkLogicalManagerService = thinkLogcicalManagerService;
            _WindowManagerService = windowManagerService;
            _hostingEnvironment = hostingEnvironment;

            _kvlConfiguration = kvlConfig.Value;
            alphaServiceUrl = kvlConfig.Value.AlphaServiceUrl;

            // httpClient.Timeout = new System.TimeSpan(0,0,0,0,1000);
        }

        /// <summary/>
        public bool ProcessWallActionFile(string presetFileName, List<string> results)
        {
            var result = true;

            if (String.IsNullOrEmpty(presetFileName))
            {
                var message = $"Preset file name not supplied... skipping ProcessWallActionFile(string presetFileName)";
                results.Add(message);
                result = false;
            }
            else
            {


                var presetPath = Path.ChangeExtension(Path.Combine(_configRepoService.PresetsPath, presetFileName), ".json");

                if (!File.Exists(presetPath))
                {
                    var message = $"Preset file not found... skipping ProcessWallActionFile('{presetPath}')";
                    results.Add(message);
                    result = false;

                }
                else
                {
                    Models.WallAction presetWallAction = LoadWallActionFile(presetPath);
                    result = Process(presetWallAction, results);
                }
            }
            return result;
        }

        /// <summary/>
        public Models.WallAction LoadWallActionFile(string presetFile)
        {

            using (StreamReader sr = new StreamReader(presetFile))
            {
                var presetFileContents = sr.ReadToEnd();
                if( presetFileContents.Length < 1 ){
                    throw new Exception("Preset file is emty");
                }
                return ParseJsonWallAction(presetFileContents);
            }
        }

        /// <summary/>
        public Models.WallAction ParseJsonWallAction(string json)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var o = JsonDocument.Parse(json);
            Models.WallAction action = o.Deserialize<Models.WallAction>( options );;
            return action;
        }

        /// <summary/>
        public bool Process(Models.WallAction action, List<string> results)
        {
            if (action == null) return false;

            Process(action.AlphaConfig, results);
            Process(action.ThinkLogicalConfig, results);
            return true;
        }


        /// <summary/>
        public void Process(Models.ThinkLogicalConfig thinkLogicalConfig, List<string> results)
        {
            if (thinkLogicalConfig == null) return;
            Process(thinkLogicalConfig.ThinkLogicalActions, results);
        }

        /// <summary/>
        public void Process(List<Models.ThinkLogicalAction> actions, List<string> results)
        {
            if (actions == null)
                return;
            actions.ForEach(thinkLogicalAction =>
            {
                Process(thinkLogicalAction, results);
            });
        }

        /// <summary/>
        public void Process(Models.ThinkLogicalAction thinkLogicalAction, List<string> results)
        {
            var routesFileName = thinkLogicalAction.path;
            var deviceName = thinkLogicalAction.device.ToLower();

            if (!String.IsNullOrEmpty(routesFileName))
            {
                var routesFilePath = Path.Combine(_configRepoService.RoutesPath, deviceName, routesFileName);
                routesFilePath = Path.ChangeExtension(routesFilePath, ".txt");
                if (!File.Exists(routesFilePath))
                {
                    throw new FileNotFoundException($"ThinkLogical routes file does not exist: [{routesFilePath}]");
                }
                else
                {
                    using (StreamReader sr = new StreamReader(routesFilePath))
                    {
                        string[] routes = File.ReadAllLines(routesFilePath);
                        _thinkLogicalManagerService.ApplyRoutes(deviceName, routes.ToList(), results);
                    }
                }
            }

            _thinkLogicalManagerService.ApplyRoutes(deviceName, thinkLogicalAction.list, results);
        }


        /// <summary/>
        public bool Process(Models.AlphaConfig alphaConfig, List<string> results)
        {
            if (alphaConfig == null) return false;

            if (alphaConfig.CloseAll)
            {
                ClearWall(results);
            }

            return Process(alphaConfig.AlphaActions, results);
        }

        /// <summary/>
        public bool Process(IEnumerable<alpha.Window> alphaActions, List<string> results)
        {

            if (alphaActions == null)
                return false;

            if (!_hostingEnvironment.IsProduction())
            {
                var message = $"AlphaApi commands cannot be applied in a non production environment. [{_hostingEnvironment.EnvironmentName}]";
                _log.Warning(message);

                results.Add(message);
//                return false;
            }
            try
            {
                /// this block was use to call an external api.
                /* 
                    var options = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
                    var body = JsonSerializer.Serialize(alphaActions, options);
                    var content = new StringContent(body, Encoding.UTF8, "application/json");
                    var result = httpClient.PostAsync($"{alphaServiceUrl}/Window", content).Result;


                    if (result.StatusCode == HttpStatusCode.OK)
                    {
                        var r = result.Content.ReadAsStringAsync().Result;
                        results.Add(r);
                        return true;
                    }
                    else
                    {
                        results.Add($"Error in request to {alphaServiceUrl}. Response: {result.StatusCode}, {result.ReasonPhrase}");
                    }
                */
                var commandOutput = _WindowManagerService.Process( alphaActions  );
                foreach (var item in commandOutput)
                {
                    results.Add( $"{item.ErrorCode}[{item.Error}] | {item.Command} {item.Args} => {item.Output}" );
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, "Process(IEnumerable<Models.AlphaAction> alphaActions, List<string> results)");
                results.Add($"Process AlphaAction Failed: {ex.Message}");
            }

            results.Add($"Alpha Actions Complete");
            return false;
        }

        public bool ValidateWallAction( WallAction wallAction, List<string> results ) {
            bool result = true;

            if ( wallAction == null ) {
                results.Add($"POST not valid with null value.  Check the format of JSON in the request body" );
                result = false;
            }
            return result;
        }
        /// <summary/>
        public bool ClearWall(List<string> results)
        {
            if (!_hostingEnvironment.IsProduction())
            {
                var message = $"AlphaApi commands cannot be applied in a non production environment. [{_hostingEnvironment.EnvironmentName}]";
                _log.Warning(message);

                results.Add(message);
                //return false;
            }

            try
            {
                _WindowManagerService.CloseAll();
                return true;
            }
            catch (Exception ex)
            {
                _log.Error(ex, "ClearWall");
                results.Add($"ClearWall Failed: {ex.Message}");
            }
            return false;
        }
    }
}



