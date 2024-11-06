#nullable enable
namespace alpha {
    /// <summary/>
    public class Window {
        public string? Formatter { get; set; }
        /// <summary/>
        public int? Id { get; set; }
        /// <summary/>
        public required string Input { get; set; }
        /// <summary/>
        public int Y { get; set; }
        /// <summary/>
        public int X { get; set; }
        /// <summary/>
        public int? AnchorX { get; set; }
        /// <summary/>
        public int? AnchorY { get; set; }

        /// <summary/>
        public int Height { get; set; }
        /// <summary/>
        public int Width { get; set; }
        /// <summary/>
        public bool? AudioEnable { get; set; }
        /// <summary/>
        public string? OSDMessage { get; set; }
        /// <summary/>
        public string? OSDColor { get; set; }
        /// <summary/>
        public string? OSDBackgroundColor { get; set; }
        /// <summary/>
        public string? OSDFont { get; set; }
        /// <summary/>
        public int? OSDFontSize { get; set; }
    }
}