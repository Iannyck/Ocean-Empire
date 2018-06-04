using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class GPC_HarpoonCapture : GPC_CapturableBased
    {
        //HarpoonCapturable harpoonCapturable;

        public GPC_HarpoonCapture(SceneManager sceneManager, HarpoonCapturable unitPrefab, Vector2 referencePosition) :
            base(sceneManager, unitPrefab.Capturable, referencePosition)
        { }
        public GPC_HarpoonCapture(SceneManager sceneManager, HarpoonCapturable unit) :
            base(sceneManager, unit.Capturable)
        { }


        protected override void OnFailure()
        {
            base.OnFailure();

            //Debug.Log("Failed harpoon capture");
        }

        protected override void OnSuccess()
        {
            base.OnSuccess();

            //Debug.Log("Succeeded harpoon capture");
        }

        protected override void OnUnitSpawned()
        {
            base.OnUnitSpawned();

            //harpoonCapturable = unit.GetComponent<HarpoonCapturable>();
        }
    }
}
