using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Microsoft.Extensions.Options;
using Serilog.Context;

namespace alpha
{
    ///<summary/>
    public class WindowCommandExecutorService : IWindowCommandExecutorService
    {
        private Models.AlphaServiceConfiguration _alphaConfig;
        //static readonly Serilog.ILogger _log = Serilog.Log.ForContext<WindowCommandExecutorService>();

        ///<summary/>
        public WindowCommandExecutorService(IOptions<Models.AlphaServiceConfiguration> alphaConfig)
        {
            _alphaConfig = alphaConfig.Value;
        }

        ///<summary/>
        public IEnumerable<CommandOutput> Execute(string[] commands)
        {
            List<CommandOutput> output = new List<CommandOutput>();
            Log.Information($"Execute {commands.Length} commands");
            foreach (string command in commands)
            {
                output.Add(Execute(command));
            }

            return output;
        }

       ///<summary/>
        public CommandOutput Execute(string alphaWindowCommandArgs)
        {
            var alphaWindowCommand = _alphaConfig.WindowCommand;

            Log.Information($"{alphaWindowCommand} {alphaWindowCommandArgs}");

            CommandOutput output = new CommandOutput()
            {
                ErrorCode = 200,
                Error = "ex.Message",
                Command = alphaWindowCommand,
                Args = alphaWindowCommandArgs
            };

            try
            {
                List<string> commandOutput = new List<string>();
                List<string> commandErrors = new List<string>();
                if (!String.IsNullOrEmpty(alphaWindowCommandArgs))
                {
                    if (!File.Exists(alphaWindowCommand))
                    {
                        output.ErrorCode = StatusCodes.Status503ServiceUnavailable;
                        output.Output = "";
                        output.Error = $"File Not Found [{alphaWindowCommand}]";
                    }
                    else
                    {
                        Process process = new Process();
                        process.StartInfo.FileName = alphaWindowCommand;
                        process.StartInfo.Arguments = alphaWindowCommandArgs;
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.CreateNoWindow = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;

                        process.OutputDataReceived += new DataReceivedEventHandler((s, e) =>
                        {
                            if (e.Data != null)
                            {
                                Log.Debug(e.Data);
                                commandOutput.Add(e.Data);
                            }
                        });
                        process.ErrorDataReceived += new DataReceivedEventHandler((s, e) =>
                        {
                            if (e.Data != null)
                            {
                                Log.Debug(e.Data);
                                commandErrors.Add(e.Data);
                            }
                        });

                        try
                        {
                            process.Start();
                            process.BeginOutputReadLine();
                            process.BeginErrorReadLine();
                            process.WaitForExit();

                            output.Output = String.Join('\n', commandOutput.ToArray());
                            output.Error = String.Join('\n', commandErrors.ToArray());
                            output.ErrorCode = process.ExitCode;
                        }
                        catch (Exception ex)
                        {
                            var message = $"ExecuteCommand: Error executing command [{alphaWindowCommand} {alphaWindowCommandArgs}] {ex.Message}";
                            output.ErrorCode = StatusCodes.Status500InternalServerError;
                            output.Output = "";
                            output.Error = message;
                            output.Command = alphaWindowCommand;
                            output.Args = alphaWindowCommandArgs;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output.ErrorCode = -97;
                output.Output = "";
                output.Error = ex.Message;
            }

            return output;
        }        
 
    }
}
