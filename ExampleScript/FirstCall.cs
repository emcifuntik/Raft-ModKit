using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace ExampleScript
{
    public class FirstCall: ModKit.IScript
    {
        public static GameObject go;
        public void Load()
        {
            
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
        }

        private void SceneManager_activeSceneChanged(UnityEngine.SceneManagement.Scene oldScene, UnityEngine.SceneManagement.Scene newScene)
        {
            if(newScene.name == "MainMenuScene")
            {

            }
            else if(newScene.name == "MainScene")
            {
                go = new GameObject();
                go.AddComponent<Script>();
                ClientScene.RegisterPrefab(go);
            }
        }
    }
}
