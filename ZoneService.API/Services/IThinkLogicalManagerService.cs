using System.Collections.Generic;

namespace Zone.Services {
    /// <summary/>
    public interface IThinkLogicalManagerService {
        /// <summary/>
        void ApplyRoutes(string deviceName, IEnumerable<string> commands, List<string> results);
 
    }
}