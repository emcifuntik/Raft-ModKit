using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WeatherScript
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
        List<string> weathers = new List<string>();
        private bool guiVisible = false;
        void Awake()
        {
            WeatherManager wm = FindObjectOfType<WeatherManager>();
            var cons = wm.GetPrivateField<WeatherConnection[]>("weatherConnections");

            weathers.Clear();
            for (int i = 0; i < cons.Length; ++i)
            {
                weathers.Add(cons[i].weatherObject.name.Split(new char[] { '_' })[1]);
            }
        }

        void OnGUI()
        {
            if (guiVisible)
            {
                int windowWidth = 200;
                int windowHeight = 40 * weathers.Count + 40;
                int windowPosX = Screen.width / 2 - windowWidth / 2;
                int windowPosY = Screen.height / 2 - windowHeight / 2;
                

                GUI.Box(new Rect(windowPosX, windowPosY, windowWidth, windowHeight), "Weather Changer");

                for (int i = 0; i < weathers.Count; ++i)
                {
                    if (GUI.Button(new Rect(windowPosX + 20, windowPosY + 20 + 40*i, 160, 30), weathers[i]))
                    {
                        WeatherManager wm = FindObjectOfType<WeatherManager>();
                        wm.SetWeather("Weather_" + weathers[i], true);
                    }
                }
            }
        }

        void Update()
        {
            if (GameManager.GameMode != GameMode.None)
            {
                if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W))
                {
                    if (!guiVisible)
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
