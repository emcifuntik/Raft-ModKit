using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ModKit
{
    public class MenuScript: MonoBehaviour
    {
        private bool guiVisible = false;
        private GUIStyle windowStyle = GUI.skin.window;
        private Vector2 scrollPosition = Vector2.zero;

        public void Awake()
        {
            //windowStyle.alignment = TextAnchor.MiddleCenter;

            //windowStyle.fontSize = 12;
            //if (Screen.height >= 800 && Screen.height < 1080)
            //    windowStyle.fontSize = 14;
            //else if (Screen.height >= 1080 && Screen.height < 1440)
            //    windowStyle.fontSize = 18;
            //else if (Screen.height >= 1440 && Screen.height < 2160)
            //    windowStyle.fontSize = 22;
            //else if (Screen.height >= 2160)
            //    windowStyle.fontSize = 24;
        }
        public void OnGUI()
        {
            if (guiVisible)
            {
                int windowPosX = Screen.width / 2 - (Screen.width / 4);
                int windowPosY = Screen.height / 2 - (Screen.height / 4);
                int windowWidth = Screen.width / 2;
                int windowHeight = Screen.height / 2;

                GUI.Box(new Rect(Screen.width / 2 - (Screen.width / 4), Screen.height / 2 - (Screen.height / 4), windowWidth, windowHeight), "Loaded mods");

                int scrollViewHeight = Loader.loadedScripts.Count * 40 + 20;

                int scrollViewWidth = windowWidth / 2 - 40;

                GUI.BeginScrollView(new Rect(windowPosX + 10, windowPosY + 20, windowWidth / 2 - 20, windowHeight - 30), scrollPosition, new Rect(0, 0, scrollViewWidth, scrollViewHeight));
                for(int i = 0; i < Loader.loadedScripts.Count; ++i)
                {
                    GUIStyleState nameStyleState = new GUIStyleState();
                    nameStyleState.textColor = Color.white;

                    GUIStyle nameStyle = new GUIStyle();
                    nameStyle.fontSize = 14;
                    nameStyle.fontStyle = FontStyle.Bold;
                    nameStyle.normal = nameStyleState;

                    GUI.TextArea(new Rect(10, 20 + i * 20, scrollViewWidth / 2 - 20, 20), Loader.loadedScripts[0].Name, nameStyle);
                    if(GUI.Button(new Rect(scrollViewWidth / 2, 20 + i * 20, scrollViewWidth / 4 - 10, 20), "Reload"))
                    {
                        //ClientScene.UnregisterPrefab(Loader.loadedScripts[i].Obj);
                        Destroy(Loader.loadedScripts[i].Obj);


                        Loader.LoadScript(Loader.loadedScripts[i].Path, i);
                        Loader.ExecuteScript(i);
                    }
                    if(GUI.Button(new Rect(scrollViewWidth / 2 + scrollViewWidth / 4, 20 + i * 20, scrollViewWidth / 4 - 10, 20), "Unload"))
                    {
                        //ClientScene.UnregisterPrefab(Loader.loadedScripts[i].Obj);
                        Destroy(Loader.loadedScripts[i].Obj);
                        Loader.loadedScripts.RemoveAt(i);
                    }
                }
                GUI.EndScrollView();

                if (GUI.Button(new Rect(windowPosX + 10 + windowWidth / 2 + 20, windowPosY + 40, windowWidth / 2 - 60, 20), "Reload All Scripts"))
                {
                    //ClientScene.UnregisterPrefab(Loader.loadedScripts[i].Obj);
                    while(Loader.loadedScripts.Count > 0)
                    {
                        Destroy(Loader.loadedScripts[0].Obj);
                        Loader.loadedScripts.RemoveAt(0);
                    }
                    Loader.LoadAllScripts();
                    Loader.ExecuteAllScripts();
                }

                //GUI.Box(new Rect(10, 10, 100, 90), "ModKit Menu");

                //if (GUI.Button(new Rect(20, 40, 80, 20), "Take bow"))
                //{
                //    var netManager = FindObjectOfType<Semih_Network>();
                //    var localPlayer = netManager.GetLocalPlayer();
                //    Item_Base bow = ItemManager.GetItemByNameContains("bow");
                //    if (bow != null)
                //    {
                //        Helper.DropItem(new ItemInstance(bow, 1, bow.MaxUses), localPlayer.transform.position, localPlayer.CameraTransform.forward, localPlayer.Controller.HasRaftAsParent);
                //    }
                //    else
                //    {
                //    }
                //}

                //if (GUI.Button(new Rect(20, 70, 80, 20), "Take arrows"))
                //{
                //    var netManager = FindObjectOfType<Semih_Network>();
                //    var localPlayer = netManager.GetLocalPlayer();
                //    Item_Base arrow = ItemManager.GetItemByNameContains("arrow");
                //    if (arrow != null)
                //    {
                //        Helper.DropItem(new ItemInstance(arrow, 20, arrow.MaxUses), localPlayer.transform.position, localPlayer.CameraTransform.forward, localPlayer.Controller.HasRaftAsParent);
                //    }
                //    else
                //    {
                //    }
                //}
            }
            GUIStyleState styleState = new GUIStyleState();
            styleState.textColor = Color.white;

            GUIStyle style = new GUIStyle();
            style.fontSize = 16;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.normal = styleState;

            GUI.TextArea(new Rect(Screen.width / 2 - 100, 10, 200, 30), "ModKit v.1.0.0.1", style);
        }

        public void Update()
        {
            if (GameManager.GameMode != GameMode.None)
            {
                if (Input.GetKeyDown(KeyCode.M))
                {
                    if (!guiVisible)
                    {
                        Helper.SetCursorVisibleAndLockState(true, CursorLockMode.None);
                        CanvasHelper.ActiveMenu = MenuType.TextWriter;
                        var ch = FindObjectOfType<CanvasHelper>();
                        ch.SetUIState(true);
                    }
                    else
                    {
                        Helper.SetCursorVisibleAndLockState(false, CursorLockMode.Locked);
                        CanvasHelper.ActiveMenu = MenuType.None;
                        var ch = FindObjectOfType<CanvasHelper>();
                        ch.SetUIState(true);
                    }
                    guiVisible = !guiVisible;
                }
            }
        }
    }
}
