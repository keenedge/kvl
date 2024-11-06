using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using alpha;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Serilog;

namespace alpha.Models
{

    /// <summary/>
    [Route("api/[controller]")]
    public class InputController : Controller
    {
        private Models.AlphaServiceConfiguration _alphaConfig;
        //readonly ILogger<InputController> _log;

        /// <summary/>
        public InputController(IOptions<Models.AlphaServiceConfiguration> alphaConfig)
        {
            //_log = log;
            _alphaConfig = alphaConfig.Value;
            Log.Information( "Input: {inputs}", _alphaConfig.ToString()  );
        }

        /// <summary>
        /// Update or Create a Window {id} defined by the values in the body
        /// </summary>       
        // GET api/input
        [HttpGet()]
        public JsonResult  Get()
        {
            Log.Information( "Input: " );

            return Json(_alphaConfig.Inputs);
        }

        
    }
}
