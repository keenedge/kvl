
using Microsoft.Extensions.Options;
using Serilog;
using SmartFormat;

namespace alpha
{
    ///<summary/>
    public class WindowCommandGeneratorService : IWindowCommandGeneratorService
    {
        //static readonly Serilog.ILogger _log = Serilog.Log.ForContext<WindowCommandGeneratorService>();

        private Models.AlphaServiceConfiguration _alphaConfig;

        ///<summary/>
        public WindowCommandGeneratorService( IOptions<Models.AlphaServiceConfiguration> alphaConfig)
        {
            _alphaConfig = alphaConfig.Value;
        }

        ///<summary/>
        public string GenerateCloseAllCommand()
        {
            return $"-closewindows";
        }
        public string GenerateCloseCommand(int Id)
        {
            //            return Enumerable.Range(1, maxId).Select(command => $"-id={command} -close");
            return $"-id={Id} -close";
        }

        public List<String> GetFormattedWindowCommands(Window window)
        {       
            string formatterKey = _alphaConfig.WindowCommandFormatterDefaultKey;
            if( formatterKey == null)
                Log.Warning("WindowCommandFormatterDefaultKey is not dedined in alpha config");


            string[] commandFormats = [];

            if (window.Formatter != null ){
                formatterKey = window.Formatter;
            }
            if( _alphaConfig.WindowCommandFormatters.ContainsKey(formatterKey )) {
                commandFormats = _alphaConfig.WindowCommandFormatters[formatterKey];
            }

            if( commandFormats.Length < 1)
                Log.Warning("No command formatters are defined for this window");

            // now generate commands for each command formatter and add them to the list.
            List<String> commands = [];
            foreach (var commandFormat in commandFormats)
            {
                var command = Smart.Format(commandFormat, window);
                commands.Add(command);

                Log.Debug("Format:\n  '{commandFormat}'\n ==>\n  Command: '{command}'", commandFormat, command);
            }

            return commands;
        }

        //<summary>
        public void CoerceWindowPositionFromAnchors(Window window){

            int screenTop = _alphaConfig.ScreenTop;
            int screenLeft = _alphaConfig.ScreenLeft;
            double xScale = _alphaConfig.XScale;
            double yScale = _alphaConfig.YScale;

            int windowTop = window.Y;
            int windowLeft = window.X;
            int windowWidth = window.Width;
            int windowHeight = window.Height;
            
            // anchor is as follows
            // -1,-1 (left,top)     0,-1 (middle, top)      1,-1 (right,top)
            // -1,0 (left,middle)   0,0  (middel, middle)   1,0  (right, middle)
            // -1,1 (left,bottom)   0,1  (middle, bottom)   1,1 (right,bottom)
            if( window.AnchorX.HasValue) {
                var anchorX = window.AnchorX.Value;
                // magic to offset the window X based on the anchorX
                windowLeft = windowLeft - ((window.Width/2)*(anchorX+1));                
            }

            if( window.AnchorY.HasValue) {
                var anchorY = window.AnchorY.Value;
                // magic to offset the window Y based on the anchorY
                windowTop = windowTop - ((window.Height/2)*(anchorY+1));                
            }

            windowTop += screenTop;
            windowLeft += screenLeft;

            window.Y = (int)Math.Round(windowTop * yScale);
            window.X = (int)Math.Round(windowLeft * xScale );
            window.Height = (int)Math.Round(window.Height * yScale);
            window.Width = (int)Math.Round(window.Width * xScale);
            Log.Debug("GenerateWindowCommand-AnchorAdjustedWindow");
            //DebugHelpers.LogObject( window, LogEventLevel.Debug );
        }

        ///<summary/>
        public List<string> GenerateWindowCommands(Window window)
        {
            IList<string> args = new List<string>();

            CoerceWindowPositionFromAnchors( window );

            List<string> commands = GetFormattedWindowCommands( window );
            return commands;
        }
    }
}
