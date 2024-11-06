using System.Text.Json;
using Microsoft.Extensions.Options;
using Serilog;

namespace Zone.Services
{
    /// <summary/>
    public class ConfigRepoService : IConfigRepoService
    {
        private string _rootPath;
        readonly Serilog.ILogger _log = Log.ForContext<ConfigRepoService>();
        private readonly IWebHostEnvironment _hostingEnvironment;
       
        /// <summary/>
        public ConfigRepoService( IWebHostEnvironment hostingEnvironment, IOptions<Models.KVLConfiguration> kvlConfig ){
            _hostingEnvironment = hostingEnvironment;
            var webRoot = _hostingEnvironment.WebRootPath;
            
            _rootPath= System.IO.Path.Combine(webRoot, kvlConfig.Value.ConfigurationRootFolder);

            _log.Information($"env        : {_hostingEnvironment.EnvironmentName}");
            _log.Information($"config root: {_rootPath}");
        }

        /// <summary/>
        public string RootPath
        {
            get { return _rootPath; }
            set { _rootPath = value; }
        }
        /// <summary/>
        public string PresetsPath
        {
            get { return Path.Combine(_rootPath, "presets"); }
        }
        /// <summary/>
        public string RoutesPath
        {
            get { return Path.Combine(_rootPath, "routes"); }
        }
        /// <summary/>
        public string AudioActionsPath
        {
            get { return Path.Combine(_rootPath, "audioactions.json"); }
        }

        /// <summary/>
        
        /// <summary/>
        public T LoadFile<T>( string filePath)
        {
            var contents = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>( contents );

        }
        /// <summary/>
        public object LoadFile( string filePath)
        {
            var contents = File.ReadAllText(filePath);
            var o = JsonDocument.Parse(contents);

            return o;
        }
    }
}