using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;

using Zone.Models;


namespace Zone.Controllers
{
    // an alias for the format used to store a generic KVP list received from the WebAPI
    using MpcBeAction = Dictionary<String, String>;

    /// <summary/>
    /// This controller sens commands to an instance of the MediaPlayer Class Balck Edition running ona remote pc
    ///
    [ApiController]
    [Route("api/[controller]")]
    public class VideoPlayerController : Controller
    {

        private static readonly HttpClient client = new HttpClient();

        private readonly IWebHostEnvironment _hostingEnvironment;

        readonly Serilog.ILogger _log = Log.ForContext<VideoPlayerController>();

        private string _videosFolder = "";

        private string _videoPlayerHost = "";
        private string commandUrl
        {
            get
            {
                return $"{_videoPlayerHost}/command.html";
            }
        }
        private string videoBrowserUrl
        {
            get
            {
                return $"{_videoPlayerHost}/browser.html";
            }
        }
        private Dictionary<String, MpcBeAction> _availableActions = new Dictionary<String, MpcBeAction>();

        /// <summary/>
        public VideoPlayerController(IWebHostEnvironment hostingEnvironment, IOptions<KVLConfiguration> kvlConfig
 )
        {
            _hostingEnvironment = hostingEnvironment;
            _videoPlayerHost = kvlConfig.Value.VideoPlayerHost;
            _videosFolder = kvlConfig.Value.VideoPlayerFolder;

            // TODO: Move this to config file.  Really for now, just a list of examples
            _availableActions.Add("pause", new MpcBeAction { { "wm_command", "888" } });
            _availableActions.Add("play", new MpcBeAction { { "wm_command", "887" } });
            _availableActions.Add("fullscreen", new MpcBeAction { { "wm_command", "830" } });
            _availableActions.Add("stretch", new MpcBeAction { { "wm_command", "838" } });
            _availableActions.Add("volumeup", new MpcBeAction { { "wm_command", "907" } });
            _availableActions.Add("volumedown", new MpcBeAction { { "wm_command", "908" } });
            _availableActions.Add("togglemute", new MpcBeAction { { "wm_command", "909" } });
            _availableActions.Add("seek-01", new MpcBeAction { { "wm_command", "899" } });
            _availableActions.Add("seek+01", new MpcBeAction { { "wm_command", "900" } });
            _availableActions.Add("seek-05", new MpcBeAction { { "wm_command", "901" } });
            _availableActions.Add("seek+05", new MpcBeAction { { "wm_command", "902" } });
            _availableActions.Add("seek-20", new MpcBeAction { { "wm_command", "903" } });
            _availableActions.Add("seek+20", new MpcBeAction { { "wm_command", "904" } });
            _availableActions.Add("speed++", new MpcBeAction { { "wm_command", "895" } });
            _availableActions.Add("speed--", new MpcBeAction { { "wm_command", "894" } });
            _availableActions.Add("frame+1", new MpcBeAction { { "wm_command", "891" } });
            _availableActions.Add("frame-1", new MpcBeAction { { "wm_command", "892" } });
        }

        /// <summary>
        /// Get a list of all defined actions
        /// </summary>
        [HttpGet()]
        [Route("actions")]
        public ActionResult GetActions()
        {
            //Response.ContentType = "application/json";
            return Ok(_availableActions);
        }

        /// <summary>
        /// Get a list of all available audio cues
        /// </summary>
        [HttpGet()]
        [Route("videos")]
        public ActionResult GetVideos()
        {
            //TODO: Load this list from Config.
            List<String> videos = new List<String>();
            videos.Add("VirtualMente_French.mp4");
            videos.Add("cl.mp4");

            Response.ContentType = "application/json";
            return Ok(videos);
        }

        /// <summary>
        /// Send the command to MPC to open a video from the {videosFolder}
        /// </summary>
        [HttpPost()]
        [Route("open")]
        public async Task<ActionResult> PostOpenVideo([FromBody] String videoFileName)
        {
            if (videoFileName == null)
            {
                throw new ArgumentNullException(nameof(videoFileName));
            }

            var filePath = $"{_videosFolder}\\{videoFileName}";
            var url = $"{videoBrowserUrl}?path={filePath}";
            _log.Debug($"PostOpenVideo: {url}");
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            Response.ContentType = "application/json";
            return Ok(responseString);            
        }


        /// <summary>
        /// Call ApplyActions with a list of actions recieved in the body of 
        /// </summary>
        [HttpPost()]
        [Route("action-list")]
        public async Task<ActionResult> PostActionList([FromBody] MpcBeAction[] actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }
            List<string> results = new List<string>();

            await ApplyActions(results, actions);
            return Ok(results);
        }


        /// <summary>
        /// Convert an Action name into the correspoinding action defined in availabeActions.null  The ApplyAction
        /// </summary>
        [HttpPost()]
        [Route("action")]
        public async Task<ActionResult> PostAction([FromBody] String playerAction)
        {
            if (playerAction == null)
            {
                throw new ArgumentNullException(nameof(playerAction));
            }


            List<string> results = new List<string>();
            if (_availableActions.ContainsKey(playerAction))
            {
                var command = _availableActions[playerAction];
                await ApplyAction(results, command);
            }
            else
            {
                _log.Error($"VideoPlayerController.PostAction(\"{playerAction}\") is not a valid action");
                throw new ArgumentOutOfRangeException(nameof(playerAction));
            }

            return Ok(results);
        }

        /// <summary>
        /// Seek to a time in the currently running video. Position should be a string such as hh:mm:ss
        /// </summary>
        [HttpPost()]
        [Route("position")]
        public async Task<ActionResult> PostPosition([FromBody] String position)
        {
            if (position == null)
            {
                throw new ArgumentNullException(nameof(position));
            }

            List<string> results = new List<string>();
            var action = new MpcBeAction{
                { "wm_command", "-1"},{ "position", position}
            };
            await ApplyAction(results, action);
            return Ok(results);
        }

        /// <summary>
        /// Set Volume Level
        /// </summary>
        [HttpPost()]
        [Route("volume")]
        public async Task<ActionResult> PostVolume([FromBody] String volume)
        {
            if (volume == null)
            {
                throw new ArgumentNullException(nameof(volume));
            }

            List<string> results = new List<string>();
            var action = new MpcBeAction{
                { "wm_command", "-2"},{ "volume", volume}
            };
            await ApplyAction(results, action);
            return Ok(results);
        }




        /// <summary>
        /// Iterate over a list of MPC-BE web server commands.null 
        /// see _availableCommands for examples.
        /// </summary>
        private async Task<bool> ApplyActions(List<string> results, IEnumerable<MpcBeAction> actions)
        {
            if (actions == null)
            {
                return false;
            }

            if (!_hostingEnvironment.IsProduction())
            {
                var message = $"Video commands cannot be applied in a non production environment. [{_hostingEnvironment.EnvironmentName}]";
                _log.Warning(message);

                results.Add(message);
                return false;
            }

            _log.Debug("ApplyActions {@host}", commandUrl);

            try
            {
                foreach (MpcBeAction action in actions)
                {
                    await ApplyAction(results, action);
                }
                return true;
            }
            catch (Exception ex)
            {
                results.Add($"Exception Processing Video Commands for host:[{commandUrl}], {ex.Message}");
                _log.Error("Exception Processing Video Commands for host:[{@host}], {@exception}", _videoPlayerHost, ex);
            }

            return false;
        }

        /// <summary>
        /// Send the MPC-BE command to the MPC-BE web server
        /// </summary>        
        private async Task ApplyAction(List<string> results, MpcBeAction action)
        {
            _log.Debug(action.ToString());
            results.Add($"Run: {action}");

            var content = new FormUrlEncodedContent(action);
            var response = await client.PostAsync($"{commandUrl}", content);
            var responseString = await response.Content.ReadAsStringAsync();
            results.Add(responseString);

            _log.Debug(responseString);
        }
    }
}
