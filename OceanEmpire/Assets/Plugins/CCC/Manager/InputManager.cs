using System.Collections.Generic;
using CCC.Input;

namespace CCC.Manager
{
    public class InputManager : BaseBankManager<InputMapping>
    {
        public bool overwriteDefaultsOnLaunch = true;

        public override void Init()
        {
            List<InputMapping> startMappings = ToList();

            //Ceci marche parce que le jeu va toujours load avec les parametre preset du scriptableObject
            if (overwriteDefaultsOnLaunch)
                foreach (InputMapping mapping in startMappings)
                {
                    mapping.SaveAsDefaults();
                }

            foreach (InputMapping mapping in startMappings)
            {
                mapping.Load();
            }

            CompleteInit();
        }

        protected override string Convert(InputMapping obj)
        {
            return obj.displayName;
        }
    }
}
