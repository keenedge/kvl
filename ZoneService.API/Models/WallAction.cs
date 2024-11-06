using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Zone.Models
{
    /// <summary/>
    public class WallAction
    {
        /// <summary/>
        public string Name { get; set; }
        /// <summary/>
        public string Description { get; set; }
        /// <summary/>
        public string Preset { get; set; }
        /// <summary/>
        public ThinkLogicalConfig ThinkLogicalConfig { get; set; }
        /// <summary/>
        public AudioConfig AudioConfig { get; set; }
        /// <summary/>
        public AlphaConfig AlphaConfig { get; set; }
    }
}