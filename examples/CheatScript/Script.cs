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
        //private static bool cheatsInited = false;
        //private static bool cheatsActivated = false;
        void Update()
        {
            if (GameManager.GameMode != GameMode.None)
            {
                if (Input.GetKeyDown(KeyCode.N))
                {
                    GameManager.UseCheats = !GameManager.UseCheats;
                    //GameManager gm = FindObjectOfType<GameManager>();
                    //Cheat cheat = gm.GetPrivateField<Cheat>("cheat");

                    //if(!cheatsInited)
                    //{
                    //    cheat.Initialize();
                    //    cheatsInited = true;
                    //}

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
