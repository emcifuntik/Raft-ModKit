using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CheatScript
{
    public class Script: MonoBehaviour
    {
        ModKit.Logger log = ModKit.Loader.log;
        Inventory myInventory = null;
        bool guiVisible = false;
        
        void Start()
        {
            log.Debug += "CheatScript.Start called";
        }

        void Awake()
        {
            log.Debug += "CheatScript.Awake called";
        }

        void OnGUI()
        {
            log.Debug += "CheatScript.OnGUI called";
            if(guiVisible)
            {
                if (GUI.Button(new Rect(10, 10, 150, 100), "I am a button"))
                {
                    print("You clicked the button!");
                }
            }
        }

        void Update()
        {
            log.Debug += "CheatScript.Update called";
            //if(guiVisible)
            //{
            //    GUI.Box(new Rect(10, 10, 100, 90), "Mod Menu");

            //    if (GUI.Button(new Rect(20, 40, 80, 20), "Take bow"))
            //    {
            //        myInventory = UnityEngine.Object.FindObjectOfType<Inventory>();
            //        Item_Base bow = ItemManager.GetItemByNameContains("bow");
            //        if (bow != null)
            //        {
            //            log.Debug += bow.name + " found. Unique name: " + bow.UniqueName + ". Unique index: " + bow.UniqueIndex;
            //            myInventory.AddItem(bow.UniqueName, 1);
            //        }
            //        else
            //        {
            //            log.Debug += "Bow not found";
            //        }
            //    }

            //    if (GUI.Button(new Rect(20, 70, 80, 20), "Take arrows"))
            //    {
            //        myInventory = UnityEngine.Object.FindObjectOfType<Inventory>();
            //        Item_Base arrow = ItemManager.GetItemByNameContains("arrow");
            //        if (arrow != null)
            //        {
            //            log.Debug += arrow.name + " found. Unique name: " + arrow.UniqueName + ". Unique index: " + arrow.UniqueIndex;
            //            myInventory.AddItem(arrow.UniqueName, 20);
            //        }
            //        else
            //        {
            //            log.Debug += "Arrow not found";
            //        }
            //    }
            //}

            if (GameManager.GameMode == GameMode.Creative && GameManager.UseCheats == false)
            {
                GameManager.UseCheats = true;
                log.Debug += "Cheats activated";
            }
            else if(GameManager.GameMode == GameMode.Creative && GameManager.UseCheats == true)
            {
                //cheat.Update();
            }

            if(GameManager.GameMode != GameMode.None)
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
                    guiVisible = !guiVisible;
                }
            }
        }
    }
}
