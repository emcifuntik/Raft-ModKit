using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExampleScript
{
    public class Script: MonoBehaviour
    {
        ModKit.Logger log = ModKit.Loader.log;
        bool guiVisible = false;

        void Awake()
        {
            //log.Debug += "CheatScript.Awake called";
        }

        void OnGUI()
        {
            if (guiVisible)
            {
                GUI.Box(new Rect(10, 10, 100, 90), "Mod Menu");

                if (GUI.Button(new Rect(20, 40, 80, 20), "Take bow"))
                {
                    var netManager = FindObjectOfType<Semih_Network>();
                    var localPlayer = netManager.GetLocalPlayer();
                    Item_Base bow = ItemManager.GetItemByNameContains("bow");
                    if (bow != null)
                    {
                        Helper.DropItem(new ItemInstance(bow, 1, bow.MaxUses), localPlayer.transform.position, localPlayer.CameraTransform.forward, localPlayer.Controller.HasRaftAsParent);
                    }
                    else
                    {
                    }
                }

                if (GUI.Button(new Rect(20, 70, 80, 20), "Take arrows"))
                {
                    var netManager = FindObjectOfType<Semih_Network>();
                    var localPlayer = netManager.GetLocalPlayer();
                    Item_Base arrow = ItemManager.GetItemByNameContains("arrow");
                    if (arrow != null)
                    {
                        Helper.DropItem(new ItemInstance(arrow, 20, arrow.MaxUses), localPlayer.transform.position, localPlayer.CameraTransform.forward, localPlayer.Controller.HasRaftAsParent);
                    }
                    else
                    {
                    }
                }
            }

            GUIStyleState styleState = new GUIStyleState();
            styleState.textColor = Color.white;

            GUIStyle style = new GUIStyle();
            style.fontSize = 24;
            style.alignment = TextAnchor.MiddleCenter;
            style.fontStyle = FontStyle.Bold;
            style.normal = styleState;

            GUI.TextArea(new Rect(Screen.width / 2 - 100, 10, 200, 30), "ModKit v.1.0.0.1", style);
        }

        void OnDestroy()
        {
        }

        void Update()
        {
            if (GameManager.GameMode != GameMode.None)
            {
                //if(Input.GetKeyDown(KeyCode.N))
                //{
                //    weatherManager = UnityEngine.Object.FindObjectOfType<WeatherManager>();
                //    //weatherManager.Set
                //}

                if (Input.GetKeyDown(KeyCode.M))
                {
                    if(!guiVisible)
                    {
                        Helper.SetCursorVisibleAndLockState(true, CursorLockMode.None);
                        CanvasHelper.ActiveMenu = MenuType.TextWriter;
                    }
                    else
                    {
                        Helper.SetCursorVisibleAndLockState(false, CursorLockMode.Locked);
                        CanvasHelper.ActiveMenu = MenuType.None;
                    }
                    guiVisible = !guiVisible;
                }
            }
        }
    }
}
