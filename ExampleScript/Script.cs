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
                    var myInventory = FindObjectOfType<PlayerInventory>();
                    Item_Base bow = ItemManager.GetItemByNameContains("bow");
                    if (bow != null)
                    {
                        log.Debug += bow.name + " found. Unique name: " + bow.UniqueName + ". Unique index: " + bow.UniqueIndex;
                        Helper.DropItem(new ItemInstance(bow, 1, bow.MaxUses), myInventory.hotbar.playerNetwork.transform.position, myInventory.hotbar.playerNetwork.CameraTransform.forward, myInventory.hotbar.playerNetwork.Controller.HasRaftAsParent);
                    }
                    else
                    {
                        log.Debug += "Bow not found";
                    }
                }

                if (GUI.Button(new Rect(20, 70, 80, 20), "Take arrows"))
                {
                    var myInventory = FindObjectOfType<PlayerInventory>();
                    Item_Base arrow = ItemManager.GetItemByNameContains("arrow");
                    if (arrow != null)
                    {
                        log.Debug += arrow.name + " found. Unique name: " + arrow.UniqueName + ". Unique index: " + arrow.UniqueIndex;
                        Helper.DropItem(new ItemInstance(arrow, 20, arrow.MaxUses), myInventory.hotbar.playerNetwork.transform.position, myInventory.hotbar.playerNetwork.CameraTransform.forward, myInventory.hotbar.playerNetwork.Controller.HasRaftAsParent);
                    }
                    else
                    {
                        log.Debug += "Arrow not found";
                    }
                }
            }
        }

        void OnDestroy()
        {
            log.Debug += "Script.OnDestroy called";
        }

        void Update()
        {
            if (GameManager.GameMode == GameMode.Creative && GameManager.UseCheats == false)
            {
                GameManager.UseCheats = true;
                log.Debug += "Cheats activated";
            }
            else if (GameManager.GameMode == GameMode.Creative && GameManager.UseCheats == true)
            {
                //cheat.Update();
            }

            if (GameManager.GameMode != GameMode.None)
            {
                //if (Input.GetKeyDown(KeyCode.Z))
                //{
                //    var items = ItemManager.GetAllItems();
                //    foreach (var item in items)
                //        log.Info += "Item [ " + item.UniqueName + " | " + item.UniqueIndex + " ]";
                //}

                //if (Input.GetKeyDown(KeyCode.B))
                //{
                //    myInventory = UnityEngine.Object.FindObjectOfType<Inventory>();
                //    Item_Base ball = ItemManager.GetItemByNameContains("BeachBall");
                //    if (ball != null)
                //    {
                //        log.Debug += ball.name + " found. Unique name: " + ball.UniqueName + ". Unique index: " + ball.UniqueIndex;
                //        myInventory.AddItem(ball.UniqueName, 1);
                //    }
                //    else
                //    {
                //        log.Debug += "BeachBall not found";
                //    }
                //}

                //if(Input.GetKeyDown(KeyCode.N))
                //{
                //    weatherManager = UnityEngine.Object.FindObjectOfType<WeatherManager>();
                //    //weatherManager.Set
                //}

                //if (Input.GetKeyDown(KeyCode.C))
                //{
                //    NetworkUI ui = UnityEngine.Object.FindObjectOfType<NetworkUI>();
                //    ui.Console("Test");
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
