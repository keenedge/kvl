using alpha;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Services {

    public interface IWindowManagerService {
        public IEnumerable<alpha.CommandOutput> Process( IEnumerable<Window> windows );
        public IEnumerable<alpha.CommandOutput> CloseAll();
        public IEnumerable<alpha.CommandOutput> Close( int id );
        
    }

}