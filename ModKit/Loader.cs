using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Win32;
using UnityEngine;

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

            
            ScriptLoader thisScript = new ScriptLoader();
            GameObject go = new GameObject();
            go.AddComponent<ScriptLoader>();
            //thisScript.StartCoroutine("Update");

            foreach (string mod in Directory.GetFiles(ModKitPath + "\\plugins"))
            {
                if (mod.EndsWith("Script.dll"))
                {
                    log += "[Debug] Plugin found \"" + mod + "\"";
                    Assembly scriptAssembly = Assembly.LoadFrom(mod);
                    string assemblyName = AssemblyName.GetAssemblyName(mod).Name;
                    log += "Script \"" + assemblyName + "\" successfully loaded.";

                    scriptsCount++;
                    //assemblyName = assemblyName.Replace("-", "_");

                    //log += "[Debug] " + assemblyName + " " + "Script";

                    //System.Runtime.Remoting.ObjectHandle handle = null;
                    //bool loaded = false;
                    //try
                    //{
                    //    handle = Activator.CreateInstance(assemblyName, assemblyName + ".Script");
                    //    log += "Instance of \"" + assemblyName + ".Script\" created";
                    //    loaded = true;
                    //}
                    //catch(Exception e)
                    //{
                    //    log += e.Message + Environment.NewLine;
                    //    loaded = false;
                    //}

                    //if(loaded)
                    //{
                    //    IScript s = null;
                    //    bool unwrapped = false;
                    //    try
                    //    {
                    //        s = (IScript)handle.Unwrap();
                    //        unwrapped = true;
                    //    }
                    //    catch(Exception e)
                    //    {
                    //        log += e.Message + Environment.NewLine;
                    //        unwrapped = false;
                    //    }
                    //    if(unwrapped)
                    //    {
                    //        if (s == null)
                    //        {
                    //            log += "Script \"" + mod + "\" cannot be loaded.";
                    //        }
                    //        else
                    //        {
                    //            scripts.Add(s);
                    //            log += "Script \"" + mod + "\" successfully loaded.";
                    //        }
                    //    }
                    //}
                }
            }

            if (scriptsCount != 0)
                log += "Found " + scriptsCount + " scripts. Loading...";
            else
                log += "Scripts not found" + Environment.NewLine;

            //for(int i = 0; i < scripts.Count; ++i)
            //    scripts[i].Load(log);
        }

        //private static void Update(object state)
        //{
        //    for (int i = 0; i < scripts.Count; ++i)
        //        scripts[i].Update();

        //    //if (GameManager.GameMode == GameMode.Creative && GameManager.UseCheats == false)
        //    //{
        //    //    GameManager.UseCheats = true;
        //    //}
        //    updateTimer.Change(0, 1000 / GameManager.targetFrameRate);
        //}
    }

}
