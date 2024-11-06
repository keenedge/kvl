using alpha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Serilog;


namespace Alpha.Services {

    public class WindowManagerService : IWindowManagerService {

        private readonly IWindowCommandGeneratorService _commandGeneratorService;
        private readonly IWindowCommandExecutorService _commandExecutorService;
        readonly alpha.Models.AlphaServiceConfiguration _alphaConfig;

        public WindowManagerService( IWindowCommandGeneratorService windowCommandGenerator, IWindowCommandExecutorService windowCommandExecutor, IOptions<alpha.Models.AlphaServiceConfiguration> alphaConfig ){
            _commandGeneratorService = windowCommandGenerator;
            _commandExecutorService = windowCommandExecutor;
            _alphaConfig = alphaConfig.Value;

            
        }
        public IEnumerable<alpha.CommandOutput> Process( IEnumerable<Window> windows ){

           Log.Information("Windows: {@Windows}", windows);

            List<string> closeWindowCommands = _commandGeneratorService.GenerateWindowCommands(new Window() { Formatter="closeAllWindows", Input="" });
            ConvertWindowsToCommands(windows, out List<CommandOutput> output, out List<string> commands);
            commands.InsertRange(0, closeWindowCommands );

            output.AddRange(_commandExecutorService.Execute(commands.ToArray()));
            Log.Information( "Command output:");

            return output;
           // DebugHelpers.LogObject( output, Serilog.Events.LogEventLevel.Information );
            
        }

        public IEnumerable<alpha.CommandOutput> CloseAll() {
            List<string> windowCommands = _commandGeneratorService.GenerateWindowCommands(new Window() { Formatter="closeAllWindows", Input="" });
            IEnumerable<CommandOutput> commandExecutorOutput = _commandExecutorService.Execute(windowCommands.ToArray());
            //DebugHelpers.LogObject( commandExecutoroutput, Serilog.Events.LogEventLevel.Debug );
            return commandExecutorOutput;

        }
        public IEnumerable<alpha.CommandOutput> Close( int id ) {
            List<string> windowCommands = _commandGeneratorService.GenerateWindowCommands(new Window() { Formatter="closeWindow", Id=id, Input="" });
            IEnumerable<CommandOutput> commandExecutorOutput = _commandExecutorService.Execute(windowCommands.ToArray());
            return commandExecutorOutput;
        }
       private void ConvertWindowsToCommands( IEnumerable<Window> windows, out List<CommandOutput> output, out List<string> commands)
        {
            output = new List<CommandOutput>();
            commands = new List<string>();
            foreach (Window window in windows)
                ConvertWindowToCommands(window, output, commands );
        }

        private void ConvertWindowToCommands(Window window, List<CommandOutput> output, List<string> commands )
        {
            if (!window.Id.HasValue)
            {
                AddMessage( output, $"Ignored window with Null Id" );
                return;
            }
            else if (window.Id.Value < 1)
            {
                AddMessage(output, $"Ignored window with Invalid Id ({window.Id.Value})");
                return;
            }
            else if ((window.Input != null) && !_alphaConfig.Inputs.ContainsKey(window.Input)) {
                AddMessage( output, $"Ignored window with invalid input ({window.Input})");
                return;
            }

            else if (window.Input == null)
            {
                // the window object does not contain an "input" so this is an "Update Window" command 
                List<string> windowCommands = _commandGeneratorService.GenerateWindowCommands(window);
                commands.AddRange(windowCommands);
                Log.Information($"{windowCommands}");
            }
            else
            {
                if (_alphaConfig.Inputs.ContainsKey(window.Input))
                {
                    var inputConfig = _alphaConfig.Inputs[window.Input];
                    window.Input = inputConfig.input.ToString();

                    // coerce audio
                    // if the input has no audio disbale it even if requsted 
                    if (window.AudioEnable.HasValue)
                    {
                        window.AudioEnable = (inputConfig.hasAudio && window.AudioEnable.Value);
                    }
                    else
                    {
                        window.AudioEnable = false;
                    }

                    List<string> windowCommands = _commandGeneratorService.GenerateWindowCommands(window);
                    commands.AddRange(windowCommands);

                    Log.Information("Commands:");
                    DebugHelpers.LogObject( windowCommands, Serilog.Events.LogEventLevel.Information );
                }
            }
        }

        private static void AddMessage(List<CommandOutput> output, string message)
        {
            output.Add(new CommandOutput()
            {
                Output = message
            });
            Log.Information(message);
        }        
    }

}