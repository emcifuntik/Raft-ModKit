using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NoSharkScript
{
    public class Script: MonoBehaviour
    {
        public void Update()
        {
            if(GameManager.GameMode != GameMode.None)
            {
                Shark shark = FindObjectOfType<Shark>();
                shark.state = SharkState.None;
            }
        }
    }
}
