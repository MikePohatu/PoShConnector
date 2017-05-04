using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using Microsoft.PowerShell;

namespace core
{
    public class Connector
    {
        private Runspace _runspace = RunspaceFactory.CreateRunspace();
        private string _command;
        private ICollection<PSObject> _results;

        public bool Bypass { get; set; }
        public List<string> Arguments { get; set; }

        public Connector(string command)
        {
            this._command = command;
        }

        public void Execute()
        {
            this._runspace.Open();
            if (this.Bypass == true)
            {
                RunspaceInvoke spaceinvoker = new RunspaceInvoke();
                spaceinvoker.Invoke("Set-ExecutionPolicy Unrestricted -Scope Process");
            }
            Pipeline pipeline = _runspace.CreatePipeline();
            pipeline.Commands.AddScript(this.BuildCommandLine());
            this._results = pipeline.Invoke();
        }

        public string GetPropertyString(string property)
        {
            return this.GetPropertyValue(property)?.Value.ToString();
        }

        public PSMemberInfo GetPropertyValue(string property)
        {
            foreach (PSObject obj in this._results)
            {
                foreach (PSMemberInfo psinfo in obj.Properties)
                {
                    if (psinfo.Name.ToUpper() == property.ToUpper())
                    { return psinfo; }
                }
            }

            return null;
            //return this._runspace.SessionStateProxy.PSVariable.GetValue(property);
        }

        private string BuildCommandLine()
        {
            string commandline = this._command;
            if (this.Arguments != null)
            {
                foreach (string arg in this.Arguments)
                {
                    commandline = commandline + " " + arg;
                }
            }
            return commandline;
        }
    }
}
