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
        private Shark shark = null;
        public void Awake()
        {
            shark = FindObjectOfType<Shark>();
            shark.enabled = false;
        }
    }
}
