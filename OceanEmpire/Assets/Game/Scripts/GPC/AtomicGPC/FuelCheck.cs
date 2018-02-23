using UnityEngine;

namespace GPComponents
{
    public class FuelCheck : AbstractGPCBehaviour
    {
        GazTank gazTank;

        public FuelCheck(SceneManager sceneManager) : base(sceneManager) { }

        public override void Abort()
        {

        }

        public override GPCState Eval()
        {
            if (gazTank != null)
                return gazTank.GetGazRatio() > 0 ? GPCState.RUNNING : GPCState.SUCCESS;
            else
                return GPCState.FAILURE;
        }

        public override void Launch()
        {
            FetchPlayer();
        }

        public override void Reset()
        {
            FetchPlayer();
        }

        void FetchPlayer()
        {
            var player = sceneManager.Read<GameObject>("player");
            if(player != null)
            {
                gazTank = player.GetComponent<SubmarinParts>().GetGazTank();
            }
        }
    }
}
