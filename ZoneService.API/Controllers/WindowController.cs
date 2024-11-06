
using Alpha.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;
namespace alpha.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WindowController : Controller
    {
        //private IWindowCommandGeneratorService _commandGeneratorService;
        //private IWindowCommandExecutorService _commandExecutorService;
        private IWindowManagerService _windowManagerService;
//        readonly Models.AlphaServiceConfiguration _alphaConfig;
        static readonly Serilog.ILogger _log = Serilog.Log.ForContext<WindowController>();

        public WindowController( IWindowManagerService windowManagerService )
        {
        // IWindowCommandExecutorService commandExecutorService, IWindowCommandGeneratorService commandGeneratorService, IOptions<Models.AlphaServiceConfiguration> alphaConfig
//            _commandExecutorService = commandExecutorService;
//            _commandGeneratorService = commandGeneratorService;
        
            _windowManagerService = windowManagerService;

            //_alphaConfig = alphaConfig.Value;
        }

        /// <summary>
        /// Update or Create a Window {id} defined by the values in the body
        ///
        /// - if AudioEnable is null or not supplied then AudioEnable is False.
        /// - if AudioEnable is false then AudioEnable is false.
        /// - if AudioEnable is true the AudioEnable is (AudioEnable &amp;&amp; VideoSource.HasAudio).
        ///
        /// - Input is Converted from String to int using Alpha/Input list
        /// </summary>       
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>        /// <remarks>
        /// Sample requests:
        ///   POST /Window
        ///   [
        ///     {
        ///       "formatter": "default",
        ///       "id": 2,
        ///       "input": "HD01",
        ///       "y": 0,
        ///       "x": 0,
        ///       "anchorX": 0,
        ///       "anchorY": 0,
        ///       "height": 0,
        ///       "width": 0,
        ///       "audioEnable": true,
        ///       "osdMessage": "string",
        ///       "osdColor": "string",
        ///       "osdBackgroundColor": "string",
        ///       "osdFont": "string",
        ///       "osdFontSize": 0
        ///     }
        /// ]
        /// </remarks>
        [HttpPost( Name = "PostWindows" )]
        public ActionResult Post([FromBody] Window[] windows)
        {
           
            if (windows == null)
            {
                return BadRequest("The Window command list is empty or malformed");
            }

            IEnumerable<alpha.CommandOutput> output = _windowManagerService.Process( windows.AsEnumerable<Window>());

           
            if( output.Any( i => i.ErrorCode > 200 ))
            {
                return StatusCode( StatusCodes.Status503ServiceUnavailable, new JsonResult(output));
            } else {
                return StatusCode( StatusCodes.Status200OK, new JsonResult(output));

            }

        }

 

        /// <summary>
        /// Close all windows
        /// </summary>
        // DELETE api/window
        [HttpDelete()]
        public ActionResult Delete()
        {
            var commandExecutorOutput = _windowManagerService.CloseAll();
            return new JsonResult(commandExecutorOutput);

        }

        /// <summary>
        /// Delete the window named by ID
        /// </summary>
        // DELETE api/window/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var commandExecutorOutput = _windowManagerService.CloseAll();
            return new JsonResult(commandExecutorOutput);
        }
    }
}
