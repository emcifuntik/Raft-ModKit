using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CheatScript
{
    public static class Utils
    {
        public static T GetPrivateField<T>(this object obj, string name)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            FieldInfo field = type.GetField(name, flags);
            return (T)field.GetValue(obj);
        }

        public static T GetPrivateProperty<T>(this object obj, string name)
        {
            BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            Type type = obj.GetType();
            PropertyInfo field = type.GetProperty(name, flags);
            return (T)field.GetValue(obj, null);
        }
    }

    public class Script: MonoBehaviour
    {
        private static int stopShowTextOn = 0;
        private static string textToShow = "";
        private static DisplayTextManager textManager = null;
        private static bool textShown = false;
        private static bool cheatsInited = false;
        //private static bool cheatsActivated = false;

        public void Awake()
        {
            textManager = FindObjectOfType<DisplayTextManager>();
        }
        public void Update()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            if (unixTimestamp < stopShowTextOn)
            {
                textManager.ShowText(textToShow);
                textShown = true;
            }
            else if (textShown)
            {
                textShown = false;
                textManager.HideDisplayTexts();
            }
            if (GameManager.GameMode != GameMode.None)
            {
                if (Input.GetKeyDown(KeyCode.B))
                {
                    GameManager.UseCheats = !GameManager.UseCheats;
                    stopShowTextOn = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + 5;
                    if (GameManager.UseCheats)
                        textToShow = "Cheatmode enabled";
                    else
                        textToShow = "Cheatmode disabled";

                    GameManager gm = FindObjectOfType<GameManager>();
                    Cheat cheat = gm.GetPrivateField<Cheat>("cheat");

                    if (!cheatsInited)
                    {
                        cheat.Initialize();
                        cheatsInited = true;
                    }

                    //if(cheatsActivated)
                    //{
                    //    GameManager.UseCheats = true;
                    //    cheatsActivated = false;
                    //}
                    //else
                    //{
                    //    GameManager.UseCheats = true;
                    //    cheatsActivated = true;
                    //}
                }
            }
        }
    }
}
