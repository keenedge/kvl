using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


//using Microsoft.AspNetCore.Cors;

using Serilog;

using Zone;
using Zone.Models;
using Zone.Services;

namespace alpha.Controllers
{

    /// <summary/>
    [ApiController]
    [Route("api/[controller]")]
    public class PresetController : Controller
    {

        readonly Serilog.ILogger _log = Log.ForContext<PresetController>();
        private IConfigRepoService _ConfigRepoService;

        /// <summary>
        /// Public Constructor
        /// </summary>
        public PresetController( IConfigRepoService configRepoService ){
            _ConfigRepoService = configRepoService;
        }


        /// <summary>
        /// Get a list of all available presets
        ///</summary>
        //    GET: api/Preset
        [HttpGet()]
        public IEnumerable<string> Get()
        {
            DirectoryInfo root = new DirectoryInfo(_ConfigRepoService.PresetsPath);
            IEnumerable<FileInfo> fileInfos = root.EnumerateFiles();
            IEnumerable<string> results = fileInfos.Select(fi => Path.GetFileNameWithoutExtension(fi.Name));
            return results;
        }


        /// <summary>
        /// Get the details of a preset specified by ID
        ///</summary>
        // GET: api/Preset/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            string path = Path.ChangeExtension(
                Path.Combine(_ConfigRepoService.PresetsPath, id),
                ".json");

            WallAction preset = _ConfigRepoService.LoadFile<WallAction>(path);
            return Ok(preset);
        }

        /// <summary>
        /// Update the defined action for preset specified by ID
        ///</summary>
        // POST: api/Preset
        [HttpPost("{id}")]
        public void Post(string id, [FromBody]WallAction wallAction)
        {
            string path = Path.ChangeExtension(
                Path.Combine(_ConfigRepoService.PresetsPath, id),
                ".json");

            using (StreamWriter file = System.IO.File.CreateText(path))
            {
                JsonSerializer.Serialize(file);
            }
        }

        /// <summary>
        /// Update the defined action for preset specified by ID
        ///</summary>
        // PUT: api/Preset/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody]WallAction wallAction)
        {
            string path = Path.ChangeExtension(
                Path.Combine(_ConfigRepoService.PresetsPath, id),
                ".json");

            using (StreamWriter file = System.IO.File.CreateText(path))
            {   


                JsonSerializer.Serialize(file);
            }
        }

        /// <summary>
        /// Delete the preset specified by ID
        ///</summary>
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            string path = Path.ChangeExtension(
                Path.Combine(_ConfigRepoService.PresetsPath, id), ".json");

            System.IO.File.Delete(path);
        }
    }
}
