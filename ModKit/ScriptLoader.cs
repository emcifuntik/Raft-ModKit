using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModKit
{
    public class ScriptLoader: UnityEngine.MonoBehaviour
    {
        public ScriptLoader()
        {
            //this.enabled = true;
            Loader.log.Debug += "ScriptLoader ctor called";
        }
        public void Update()
        {
            Loader.log.Debug += "ScriptLoader.Update";
        }

        public void Start()
        {
            Loader.log.Debug += "ScriptLoader.Start";
        }

        public void FixedUpdate()
        {
            Loader.log.Debug += "ScriptLoader.FixedUpdate";
        }
        public void LateUpdate()
        {
            Loader.log.Debug += "ScriptLoader.LateUpdate";
        }
    }
}
