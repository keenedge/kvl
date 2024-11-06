using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Zone.Models
{
    /// <summary/>
    public class AlphaActionxx
    {
        /// <summary/>
        public int? Id { get; set; }
        /// <summary/>
        public string Input { get; set; }
        /// <summary/>
        public int? Y { get; set; }
        /// <summary/>
        public int? X { get; set; }
        /// <summary/>

        public int? AnchorX { get; set; }
        /// <summary/>
        public int? AnchorY { get; set; }

        /// <summary/>
        public int? Width { get; set; }
        /// <summary/>
        public int? Height { get; set; }
        /// <summary/>
        public bool? AudioEnable { get; set; }
        
        /// <summary/>
        public string OSDMessage { get; set; }

        /// <summary/>
        public string OSDColor { get; set; }

        /// <summary/>
        public string OSDBackgroundColor { get; set; }

        /// <summary/>
        public string OSDFont { get; set; }

        /// <summary/>
        public int? OSDFontSize { get; set; }
    }
}