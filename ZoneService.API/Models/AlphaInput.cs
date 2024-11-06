#nullable enable
using System;


namespace alpha.Models
{
    /// <summary/>
    public class AlphaInput
    {
       /// <summary/>
       public string? key { get; set; }
       /// <summary/>
       public bool hasAudio { get; set; }
       /// <summary/>
       public bool is4K { get; set; }
       /// <summary/>
       public string? description { get; set; }
       /// <summary/>
       public int input { get; set; }
    }
}