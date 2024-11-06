using Microsoft.AspNetCore.Mvc;
using Serilog;
using Zone.Services;

namespace alpha.Controllers
{

    /// <summary/>
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigController : Controller
    {

        readonly Serilog.ILogger _log = Log.ForContext<PresetController>();
        private IConfigRepoService _ConfigRepoService;

        /// <summary>
        /// Public Constructor
        /// </summary>
        public ConfigController(IConfigRepoService configRepoService)
        {
            _ConfigRepoService = configRepoService;
        }


        /// <summary>
        /// Get a list of all available presets
        ///</summary>
        //    GET: api/Config
        [HttpGet()]
        public JsonResult Get()
        {
            string path = Path.ChangeExtension(
                Path.Combine(_ConfigRepoService.RootPath, "config"), ".json");

            Object config = _ConfigRepoService.LoadFile(path);
            var j =Json(config);
            return j;
        }
    }
}
