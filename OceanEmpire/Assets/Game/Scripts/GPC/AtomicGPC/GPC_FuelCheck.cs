using UnityEngine;

namespace GPComponents
{
    public class GPC_FuelCheck : AbstractGPCBehaviour
    {
        GazTank gazTank;
        SceneManager sceneManager;

        public GPC_FuelCheck(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

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
                gazTank = player.GetComponent<SubmarinParts>().GazTank;
            }
        }
    }
}
