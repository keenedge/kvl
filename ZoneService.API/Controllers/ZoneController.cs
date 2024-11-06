using alpha.Controllers;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Zone.Models;
using Zone.Services;

namespace Zone.Controllers
{
    /// summary
    [ApiController]
    [Route("api/[controller]")]
     public class ZoneController : Controller
    {
        readonly IZoneManagerService _zoneManagerService;
        readonly Serilog.ILogger _log = Log.ForContext<ZoneController>();

        /// summary
        public ZoneController(IZoneManagerService zoneManagerService){
            _zoneManagerService = zoneManagerService;
        }

        /// <summary>
        /// Update the zone with actions defined in the body of the put request
        /// </summary>
        [HttpPost()]
        public ActionResult Post([FromBody]WallAction wallAction)
        {
            _log.Information("ZoneController POST: {@WallAction}", wallAction);

            List<string> results = new List<string>();
            try {
                if( !_zoneManagerService.ValidateWallAction(wallAction, results )) {
                    return BadRequest(results);
                };
                if( wallAction.Preset == null ) {
                    return BadRequest(results);
                };

                var result1 = _zoneManagerService.ProcessWallActionFile(wallAction.Preset, results); 
                var result2 =  _zoneManagerService.Process(wallAction, results);
                results.ForEach((result) => {
                    _log.Information(result);
                });
                if( result1 && result2 ) {
                    results.ForEach((result) => {
                        _log.Information(result);
                    });
                    return Ok(results);
                }
                else
                {
                    results.ForEach((result) => {
                        _log.Error(result);
                    });
                    return BadRequest(results);
                }
            }
            catch( Exception ex) {
                results.Add("An error occurred while attempting to POST a wall action");
                results.Add(ex.Message);
                return BadRequest(results);
            }
        }
    }
}
