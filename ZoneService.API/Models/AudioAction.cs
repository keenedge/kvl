using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zone.Models
{
    /// <summary/>
    public class AudioAction
    {
        /// <summary/>
        public string Zone { get; set; }
        /// <summary/>
        public string Id { get; set; }
        /// <summary/>
        public string Description { get; set; }
        /// <summary/>
        public string Device { get; set; }
        /// <summary/>
        public int Cue { get; set; }
        /// <summary/>
        public int? CueList { get; set; }
    }
}