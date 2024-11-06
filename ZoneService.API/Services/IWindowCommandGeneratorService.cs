using System.Collections.Generic;

namespace alpha
{
    ///<summary/>
    public interface IWindowCommandGeneratorService
    {
        ///<summary/>
        string GenerateCloseCommand(int Id);
        ///<summary/>
        string GenerateCloseAllCommand();
        ///<summary/>
        List<string> GenerateWindowCommands(Window window);        
    }
}