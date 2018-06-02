using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheatScript
{
    public class Script: ModKit.IScript
    {
        ModKit.Logger log = null;
        public void Load(ModKit.Logger logger)
        {
            log = logger;
        }

        public void Update()
        {
            if (GameManager.GameMode == GameMode.Creative && GameManager.UseCheats == false)
            {
                GameManager.UseCheats = true;
                log.Debug += "Cheats activated";
            }
        }
    }
}
