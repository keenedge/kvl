using System;
using System.Collections.Generic;

namespace alpha.Models
{
    /// <summary/>
    public class AlphaServiceConfiguration
    {
       /// <summary/>
       public required string WindowCommand { get; set; }
       /// <summary/>
       public required string WindowCommandFormatterDefaultKey { get; set; }
       /// <summary/>
       public required IDictionary<string,string[]> WindowCommandFormatters { get; set; }
       /// <summary/>
       public required IDictionary<string,AlphaInput> Inputs { get; set; }
       /// <summary/>
       public required int ScreenTop { get; set; }
       /// <summary/>
       public required int ScreenLeft { get; set; }
       public required double XScale { get; set; }
       /// <summary/>
       public required double YScale { get; set; }
    }
}