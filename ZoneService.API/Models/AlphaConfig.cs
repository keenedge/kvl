using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zone.Models
{
    /// <summary/>
    public class AlphaConfig
    {
        /// <summary/>
        public bool CloseAll { get; set; }
        /// <summary/>
        public List<alpha.Window> AlphaActions { get; set; }
    }
}