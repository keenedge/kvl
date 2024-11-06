#nullable enable
using System.Collections.Generic;

namespace alpha {
    ///<summary/>
    public class CommandOutput {
        ///<summary/>
        public int ErrorCode { get; set; }
        ///<summary/>
        public string? Output { get; set; }
        ///<summary/>
        public string? Error { get; set; }
        ///<summary/>
        public string? Command { get; set; }
        ///<summary/>
        public string? Args { get; set; }
    }
}