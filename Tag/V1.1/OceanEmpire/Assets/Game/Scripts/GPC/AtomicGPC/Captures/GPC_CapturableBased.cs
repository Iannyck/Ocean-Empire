using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPComponents
{
    public class GPC_CapturableBased : GPC_UnitBased
    {
        protected Capturable capturable;
        protected enum CapturableState { KILLED, CAPTURED, ONGOING }

        public GPC_CapturableBased(SceneManager sceneManager, Capturable unitPrefab, Vector2 referencePosition) :
            base(sceneManager, unitPrefab.gameObject, referencePosition)
        { }
        public GPC_CapturableBased(SceneManager sceneManager, Capturable unit) :
            base(sceneManager, unit.gameObject)
        { }

        private CapturableState state;
        protected CapturableState State
        {
            get { return state; }
            set
            {
                CapturableState wasState = state;
                state = value;

                if (wasState == CapturableState.ONGOING)
                {
                    switch (state)
                    {
                        case CapturableState.KILLED:
                            OnFailure();
                            break;
                        case CapturableState.CAPTURED:
                            OnSuccess();
                            break;
                    }
                }
            }
        }

        public override void Launch()
        {
            base.Launch();
            if (state == CapturableState.ONGOING)
                return;

            State = CapturableState.ONGOING;

            // Succeed if captured
            capturable.OnNextCapture += OnCapture;

            // Fail if killable
            var killable = unit.GetComponent<BaseKillableUnit>();
            if (killable != null)
                killable.OnNextDeath += OnDeath;
        }

        protected virtual void OnDeath(BaseKillableUnit killableUnit)
        {
            if (State == CapturableState.ONGOING)
                State = CapturableState.KILLED;
        }
        protected virtual void OnCapture(Capturable capturable)
        {
            if (State == CapturableState.ONGOING)
                State = CapturableState.CAPTURED;
        }

        protected virtual void OnFailure() { }
        protected virtual void OnSuccess() { }

        public override GPCState Eval()
        {
            if (capturable == null)
                return GPCState.FAILURE;

            switch (State)
            {
                default:
                case CapturableState.KILLED:
                    return GPCState.FAILURE;
                case CapturableState.CAPTURED:
                    return GPCState.SUCCESS;
                case CapturableState.ONGOING:
                    return GPCState.RUNNING;
            }
        }

        protected override void OnUnitSpawned()
        {
            base.OnUnitSpawned();

            capturable = unit.GetComponent<Capturable>();
        }

        public override void Reset()
        {

        }

        public override void Abort()
        {
            // Succeed if captured
            capturable.OnNextCapture -= OnCapture;

            // Fail if killable
            var killable = unit.GetComponent<BaseKillableUnit>();
            if (killable != null)
                killable.OnNextDeath -= OnDeath;
        }
    }
}