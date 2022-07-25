using System;
using System.Collections.Generic;
using System.Linq;
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
        public string PluginVersion => "0.01";
        public string PluginEmail => "";

        private V8ScriptEngine engine;
        private IPluginHost _host;
        private string _error = "";

        public void Initialize(IPluginHost host)
        {
            engine = new V8ScriptEngine();
            _host = host;
            _host.ActiveIWindow.OutputText("* js4irc loaded");
            _host.HookIdentifier("js.execScript", execute);
            _host.HookIdentifier("js.error", getLastError);
        }

        private void getLastError(RegisteredIdentifierArgs argument)
        {
            argument.ReturnString = this._error;
        }

        private void execute(RegisteredIdentifierArgs argument)
        {
            var input = argument.InputParameters.First();
            try
            {
                var result = engine.Evaluate(input);
                _error = "";
                argument.ReturnString = $"{result}";
            }
            catch (ScriptEngineException err)
            {
                _error = $"{err}";
                argument.ReturnString = "";
            }
        }

        public void Dispose()
        {
        }
    }
}
