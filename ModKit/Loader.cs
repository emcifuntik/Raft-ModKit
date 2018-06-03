using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Win32;
using UnityEngine;
using System.Runtime.Remoting;

/// <summary>
/// 
/// </summary>
namespace ModKit {

    public class Loader {
        public static Logger log = null;

        public static void Load() {
            string ModKitPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\RaftModKit", "ModKitFolder", null);
            log = new Logger(ModKitPath + "\\modlog.txt");
            log += "[Debug] Logger inited" + Environment.NewLine;

            int scriptsCount = 0;

            foreach (string mod in Directory.GetFiles(ModKitPath + "\\plugins"))
            {
                if (mod.EndsWith("Script.dll"))
                {
                    log += "[Debug] Plugin found \"" + mod + "\"";
                    Assembly scriptAssembly = Assembly.LoadFrom(mod);
                    string assemblyName = AssemblyName.GetAssemblyName(mod).Name;
                    assemblyName = assemblyName.Replace("-", "_");
                    log += "Script \"" + assemblyName + "\" successfully loaded.";

                    scriptsCount++;

                    bool loaded = false;
                    ObjectHandle handle = null;

                    try
                    {
                        handle = Activator.CreateInstance(assemblyName, assemblyName + ".FirstCall");
                        log += "Instance of \"" + assemblyName + ".FirstCall\" created";
                        loaded = true;
                    }
                    catch (Exception e)
                    {
                        log += e.Message + Environment.NewLine;
                        loaded = false;
                    }

                    if (loaded)
                    {
                        IScript s = null;
                        bool unwrapped = false;
                        try
                        {
                            s = (IScript)handle.Unwrap();
                            unwrapped = true;
                        }
                        catch (Exception e)
                        {
                            log += e.Message + Environment.NewLine;
                            unwrapped = false;
                        }
                        if (unwrapped)
                        {
                            if (s == null)
                            {
                                log += "Script \"" + mod + "\" cannot be loaded.";
                            }
                            else
                            {
                                log += "Script \"" + mod + "\" successfully loaded.";
                                s.Load();
                            }
                        }
                    }
                }
            }

            if (scriptsCount != 0)
                log += "Found " + scriptsCount + " scripts. Loading...";
            else
                log += "Scripts not found" + Environment.NewLine;
        }
    }
}
