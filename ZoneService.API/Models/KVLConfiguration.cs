using System;

namespace Zone.Models
{
    /// <summary/>
    public class KVLConfiguration
    {
       /// <summary/>
       public string ConfigurationRootFolder { get; set; }
       /// <summary/>
       public string AlphaServiceUrl { get; set; }
       /// <summary/>
       public string VideoPlayerHost { get; set; }
       /// <summary/>
       /// The folder on z2-ALPHA-VID for videos are stored.null  e.g.null c:\videos
       public string VideoPlayerFolder { get; set; }
    }
}