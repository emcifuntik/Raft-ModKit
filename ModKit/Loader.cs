using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Win32;

/// <summary>
/// 
/// </summary>
namespace ModKit {

    public class Loader {
        private static Timer updateTimer = null;
        private static AutoResetEvent autoEvent = new AutoResetEvent(false);
        private static List<IScript> scripts = new List<IScript>();
        private static Logger log = null;
        public static void Load() {
            string ModKitPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\RaftModKit", "ModKitFolder", null);
            log = new Logger(ModKitPath + "\\modlog.txt");
            log += "[Debug] Logger inited" + Environment.NewLine;

            updateTimer = new Timer(Update, autoEvent, 0, 1000 / GameManager.targetFrameRate);

            foreach (string mod in Directory.GetFiles(ModKitPath + "\\plugins"))
            {
                log += "[Debug] Plugin found \"" + mod + "\"";
                if (mod.EndsWith(".dll"))
                {
                    Assembly myassembly = Assembly.LoadFrom(mod);
                    string assemblyName = AssemblyName.GetAssemblyName(mod).Name;
                    assemblyName = assemblyName.Replace("-", "_");

                    log += "[Debug] " + assemblyName + " " + "Script";

                    System.Runtime.Remoting.ObjectHandle handle = null;
                    bool loaded = false;
                    try
                    {
                        handle = Activator.CreateInstance(assemblyName, assemblyName + ".Script");
                        log += "Instance of \"" + assemblyName + ".Script\" created";
                        loaded = true;
                    }
                    catch(Exception e)
                    {
                        log += e.Message + Environment.NewLine;
                        loaded = false;
                    }

                    if(loaded)
                    {
                        IScript s = null;
                        bool unwrapped = false;
                        try
                        {
                            s = (IScript)handle.Unwrap();
                            unwrapped = true;
                        }
                        catch(Exception e)
                        {
                            log += e.Message + Environment.NewLine;
                            unwrapped = false;
                        }
                        if(unwrapped)
                        {
                            if (s == null)
                            {
                                log += "Script \"" + mod + "\" cannot be loaded.";
                            }
                            else
                            {
                                scripts.Add(s);
                                log += "Script \"" + mod + "\" successfully loaded.";
                            }
                        }
                    }
                }
            }

            if (scripts.Count != 0)
                log += "Found " + scripts.Count + " scripts. Loading...";
            else
                log += "Scripts not found" + Environment.NewLine;

            for(int i = 0; i < scripts.Count; ++i)
                scripts[i].Load(log);
        }

        private static void Update(object state)
        {
            for (int i = 0; i < scripts.Count; ++i)
                scripts[i].Update();

            //if (GameManager.GameMode == GameMode.Creative && GameManager.UseCheats == false)
            //{
            //    GameManager.UseCheats = true;
            //}
            updateTimer.Change(0, 1000 / GameManager.targetFrameRate);
        }

        /// <summary>
        /// Unload methods can also be called remotely by MonoJunkie.
        /// </summary>
        public static void Unload() {

			//...

		}

    }

}
