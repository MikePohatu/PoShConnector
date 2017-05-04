using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using core;

namespace PoShConnectorCmd
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                List<string> properties = new List<string>();
                List<string> scriptargs = new List<string>();
                Connector connector = new Connector(args[0]);

                #region
                int i = 1;
                while (i < args.Length)
                {
                    string arg = args[i];
                    switch (arg)
                    {
                        case "-bypass":
                            connector.Bypass = true;
                            break;
                        case "-property":
                            properties.Add(args[i + 1]);
                            i = i + 2;
                            continue;
                        default:
                            scriptargs.Add(arg);
                            break;
                    }
                    i++;
                }
                #endregion

                try { connector.Execute(); }
                catch (PSSecurityException)
                {
                    Console.WriteLine("Execution policy does not allow running current script. Please try the -bypass option");
                    Environment.Exit(1);
                }
                
                foreach (string property in properties)
                {
                    Console.WriteLine("{0}: {1}", property, connector.GetPropertyString(property));
                }
            }
        }
    }
}
