using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AdiIRCAPIv2.Arguments.Aliasing;
using AdiIRCAPIv2.Interfaces;

using Microsoft.ClearScript;
using Microsoft.ClearScript.V8;

namespace js4irc
{
    public class js4irc : IPlugin
    {
        public string PluginName => "js4irc";
        public string PluginDescription => "V8 Javascript engine for AdiIRC";
        public string PluginAuthor => "JD";
        public string PluginVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public string PluginEmail => "";

        private V8ScriptEngine engine;
        private IPluginHost _host;
        private string _error = "";

        /// <summary>
        /// Called by AdiIRC to initialise plugin
        /// </summary>
        /// <param name="host"></param>
        public void Initialize(IPluginHost host)
        {
            engine = new V8ScriptEngine();
            _host = host;

            // Exposes $js.execScript and $js.error to AdiIRC
            _host.HookIdentifier("js.execScript", execScript);
            _host.HookIdentifier("js.error", getLastError);

            // Exposes adi.eval and adi.exec to Javascript
            engine.AddHostObject("adi", new
            {
                eval = new Func<string, string>((command) => { return this.evaluate(command); }),
                exec = new Action<string>((text) => { this.executeCommand(text); })
            });
        }

        /// <summary>
        /// Gets the error (if any) from the last executeScript() call
        /// </summary>
        /// <param name="argument"></param>
        private void getLastError(RegisteredIdentifierArgs argument)
        {
            argument.ReturnString = this._error;
        }

        /// <summary>
        /// Executes a script in the V8 Engine
        /// </summary>
        /// <param name="argument"></param>
        private void execScript(RegisteredIdentifierArgs argument)
        {
            var code = argument.InputParameters.First();
            try
            {
                var result = engine.Evaluate(code);
                _error = "";
                argument.ReturnString = $"{result}";
            }
            catch (ScriptEngineException err)
            {
                _error = $"{err}";
                argument.ReturnString = "";
            }
        }

        /// <summary>
        /// Executes a command in AdiIRC
        /// </summary>
        /// <param name="command">Command to be executed in AdiIRC</param>
        private void executeCommand(string command)
        {
            _host.ActiveIWindow.ExecuteCommand(command);
        }

        /// <summary>
        /// Evaluates a string in AdiIRC
        /// </summary>
        /// <param name="text">Text to be evaluated in AdiIRC</param>
        /// <returns></returns>
        private string evaluate(string text)
        {
            return _host.ActiveIWindow.Evaluate(text, "");
        }

        /// <summary>
        /// Called by AdiIRC on shutdown
        /// </summary>
        public void Dispose()
        {
        }
    }
}
