using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExampleScript
{
    public class Script : MonoBehaviour
    {
        private bool guiVisible = false;
        private List<Item_Base> items = null;
        private Vector2 scrollPosition = Vector2.zero;

        public void Awake()
        {
            items = ItemManager.GetAllItems();
        }
        public void OnGUI()
        {
            if (guiVisible)
            {
                int windowWidth = Screen.width / 4;
                int windowHeight = Screen.height / 2;
                int windowPosX = Screen.width / 2 - windowWidth / 2;
                int windowPosY = Screen.height / 2 - windowHeight / 2;
                
                GUI.Box(new Rect(windowPosX, windowPosY, windowWidth, windowHeight), "Object spawner");

                int scrollViewHeight = items.Count * 20 + 40;

                int scrollViewWidth = windowWidth - 40;

                scrollPosition = GUI.BeginScrollView(new Rect(windowPosX + 10, windowPosY + 20, windowWidth - 20, windowHeight - 30), scrollPosition, new Rect(0, 0, scrollViewWidth, scrollViewHeight));
                for (int i = 0; i < items.Count; ++i)
                {
                    GUIStyleState nameStyleState = new GUIStyleState();
                    nameStyleState.textColor = Color.white;

                    GUIStyle nameStyle = new GUIStyle();
                    nameStyle.fontSize = 14;
                    nameStyle.fontStyle = FontStyle.Bold;
                    nameStyle.normal = nameStyleState;

                    GUI.Box(new Rect(10, 20 + i * 20, scrollViewWidth / 2 - 20, 20), items[i].name, nameStyle);
                    if (GUI.Button(new Rect(scrollViewWidth / 2, 20 + i * 20, scrollViewWidth / 2 - 10, 20), "Spawn"))
                    {
                        var netManager = FindObjectOfType<Semih_Network>();
                        var localPlayer = netManager.GetLocalPlayer();
                        Helper.DropItem(new ItemInstance(items[i], 1, items[i].MaxUses), localPlayer.transform.position, localPlayer.CameraTransform.forward, localPlayer.Controller.HasRaftAsParent);
                    }
                }
                GUI.EndScrollView();
            }
        }

        public void Update()
        {
            if (GameManager.GameMode != GameMode.None)
            {
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.O))
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
                else if (Input.GetKeyDown(KeyCode.Escape) && guiVisible)
                {
                    Helper.SetCursorVisibleAndLockState(false, CursorLockMode.Locked);
                    CanvasHelper.ActiveMenu = MenuType.None;
                    guiVisible = !guiVisible;
                }
            }
        }
    }

}
