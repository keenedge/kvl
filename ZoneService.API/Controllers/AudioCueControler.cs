using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Serilog;

using Zone;
using Zone.Models;
using Zone.Services;


namespace Zone.Controllers
{
/// <summary/>
    [ApiController]
    [Route("api/[controller]")]
     public class AudioCueController : Controller
    {
        private IConfigRepoService _ConfigRepoService;
        private Dictionary<string,AudioAction> _Sources = null;

//        readonly Serilog.ILogger _log = Log.ForContext<AudioCueController>();

        /// <summary/>
        public AudioCueController( IConfigRepoService configRepoService ){
            
            _ConfigRepoService = configRepoService;

            string path = _ConfigRepoService.AudioActionsPath;
            IList<AudioAction> sources = _ConfigRepoService.LoadFile<IList<AudioAction>>(path);
            _Sources = sources.ToDictionary(x => x.Id, x => x);
        }

        /// <summary>
        /// Get a list of all available audio cues
        /// </summary>
        [HttpGet()]
        public ActionResult Get()
        {
            Response.ContentType = "application/json";
            return Ok(_Sources.Values);
        }


        /// <summary>
        /// Get details of the Audio Cue Named by ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            Response.ContentType = "application/json";
            if( _Sources.ContainsKey(id)){
                return Ok(_Sources[id]);
            }
            else{
                return NotFound(id);
            }
        }

        
    }
}
