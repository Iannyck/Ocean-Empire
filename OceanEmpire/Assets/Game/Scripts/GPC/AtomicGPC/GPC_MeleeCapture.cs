using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class GPC_MeleeCapture : GPC_CapturableBased
    {
        MeleeCapturable meleeCapturable;

        public GPC_MeleeCapture(SceneManager sceneManager, MeleeCapturable unitPrefab, Vector2 referencePosition) :
            base(sceneManager, unitPrefab.Capturable, referencePosition)
        { }
        public GPC_MeleeCapture(SceneManager sceneManager, MeleeCapturable unit) :
            base(sceneManager, unit.Capturable)
        { }


        protected override void OnFailure()
        {
            base.OnFailure();

            Debug.Log("on fail");
        }

        protected override void OnSuccess()
        {
            base.OnSuccess();

            Debug.Log("on succeed");
        }

        protected override void OnUnitSpawned()
        {
            base.OnUnitSpawned();

            meleeCapturable = unit.GetComponent<MeleeCapturable>();
        }
    }
}