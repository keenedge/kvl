using System.Collections.Generic;
using Zone.Models;

namespace Zone.Services
{
    /// <summary/>
    public interface IZoneManagerService {
        /// <summary/>
        bool ProcessWallActionFile(string presetFileName, List<string> results);
        /// <summary/>
        bool Process(Models.WallAction action, List<string> results );
        public bool ValidateWallAction( WallAction wallAction, List<string> results );
        /// <summary/>
        bool ClearWall(List<string> results);
   }
}