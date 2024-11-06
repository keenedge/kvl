using System.Collections.Generic;

namespace alpha
{
    ///<summary/>
    public interface IWindowCommandExecutorService
    {
        ///<summary/>
        IEnumerable<CommandOutput> Execute(string[] commands);
    }
}