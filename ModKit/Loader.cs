using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Win32;
using UnityEngine;
using System.Runtime.Remoting;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Linq;

/// <summary>
/// 
/// </summary>
namespace ModKit {

    public class Loader {
        public static Logger log = null;
        private static GameObject go = null;
        public static List<LoadedScript> loadedScripts = new List<LoadedScript>();
        public static string ModKitPath = "";

        public static void Load() {
            ModKitPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\RaftModKit", "ModKitFolder", null);
            log = new Logger(ModKitPath + "\\modlog.txt");
            log += "[Debug] Logger inited" + Environment.NewLine;

            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;


            LoadAllScripts();
            ExecuteAllScripts();
        }

        public static void LoadAllScripts()
        {
            foreach (string mod in Directory.GetFiles(ModKitPath + "\\plugins"))
            {
                if (mod.EndsWith("Script.dll") && IsManagedAssembly(mod))
                    LoadScript(mod);
            }

            if (loadedScripts.Count != 0)
                log += "Found " + loadedScripts.Count + " scripts. Loading...";
            else
                log += "Scripts not found" + Environment.NewLine;
        }

        //https://github.com/crosire/scripthookvdotnet/blob/dev_v3/source/core/ScriptDomain.cs#L305
        public static bool IsManagedAssembly(string fileName)
        {
            using (Stream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                if (fileStream.Length < 64)
                {
                    return false;
                }

                //PE Header starts @ 0x3C (60). Its a 4 byte header.
                fileStream.Position = 0x3C;
                uint peHeaderPointer = binaryReader.ReadUInt32();
                if (peHeaderPointer == 0)
                {
                    peHeaderPointer = 0x80;
                }

                // Ensure there is at least enough room for the following structures:
                //     24 byte PE Signature & Header
                //     28 byte Standard Fields         (24 bytes for PE32+)
                //     68 byte NT Fields               (88 bytes for PE32+)
                // >= 128 byte Data Dictionary Table
                if (peHeaderPointer > fileStream.Length - 256)
                {
                    return false;
                }

                // Check the PE signature.  Should equal 'PE\0\0'.
                fileStream.Position = peHeaderPointer;
                uint peHeaderSignature = binaryReader.ReadUInt32();
                if (peHeaderSignature != 0x00004550)
                {
                    return false;
                }

                // skip over the PEHeader fields
                fileStream.Position += 20;

                const ushort PE32 = 0x10b;
                const ushort PE32Plus = 0x20b;

                // Read PE magic number from Standard Fields to determine format.
                var peFormat = binaryReader.ReadUInt16();
                if (peFormat != PE32 && peFormat != PE32Plus)
                {
                    return false;
                }

                // Read the 15th Data Dictionary RVA field which contains the CLI header RVA.
                // When this is non-zero then the file contains CLI data otherwise not.
                ushort dataDictionaryStart = (ushort)(peHeaderPointer + (peFormat == PE32 ? 232 : 248));
                fileStream.Position = dataDictionaryStart;

                uint cliHeaderRva = binaryReader.ReadUInt32();
                if (cliHeaderRva == 0)
                {
                    return false;
                }

                return true;
            }
        }

        public static void ExecuteAllScripts()
        {
            for (var i = 0; i < loadedScripts.Count; ++i)
            {
                loadedScripts[i].Obj = new GameObject();
                loadedScripts[i].Obj.AddComponent(loadedScripts[i].ScriptType);
                ClientScene.RegisterPrefab(loadedScripts[i].Obj);
            }
        }

        public static void ExecuteScript(int id)
        {
            loadedScripts[id].Obj = new GameObject();
            loadedScripts[id].Obj.AddComponent(loadedScripts[id].ScriptType);
            ClientScene.RegisterPrefab(loadedScripts[id].Obj);
        }

        public static int LoadScript(string path, int pos = -1)
        {
            Assembly scriptAssembly = Assembly.Load(File.ReadAllBytes(path));
            string assemblyName = scriptAssembly.FullName.Split(new char[] { ',' })[0];
            assemblyName = assemblyName.Replace("-", "_");

            try
            {
                foreach (var type in scriptAssembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(MonoBehaviour)))
                    {
                        LoadedScript scr = new LoadedScript
                        {
                            Name = assemblyName,
                            Path = path,
                            ScriptType = type
                        };

                        if(pos == -1)
                            loadedScripts.Add(scr);
                        else
                            loadedScripts[pos] = scr;
                        log.Info += "Script \"" + type.FullName + "\" from \"" + Path.GetFileName(path) + "\" successfully loaded.";
                        return loadedScripts.IndexOf(scr);
                    }
                }
            }
            catch (Exception e)
            {
                log.Error += e.Message;
            }
            return -1;
        }

        private static void SceneManager_activeSceneChanged(Scene oldScene, Scene newScene)
        {
            ExecuteAllScripts();

            if (newScene.name == "MainMenuScene")
            {

            }
            else if (newScene.name == "MainScene")
            {
                go = new GameObject();
                go.AddComponent<MenuScript>();
                ClientScene.RegisterPrefab(go);
            }
        }
    }
}
