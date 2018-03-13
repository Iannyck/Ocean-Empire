using UnityEngine;

namespace GPComponents
{
    public class GPC_FuelCheck : AbstractGPCBehaviour
    {
        GazTank gazTank;
        SceneManager sceneManager;

        private GPCState wasState;

        public GPC_FuelCheck(SceneManager sceneManager)
        {
            this.sceneManager = sceneManager;
        }

        public override void Abort()
        {

        }

        public override GPCState Eval()
        {
            GPCState newState = GPCState.RUNNING;

            if (gazTank != null && gazTank.GetGazRatio() <= 0)
                newState = GPCState.SUCCESS;
            
            if(newState == GPCState.SUCCESS && wasState != GPCState.SUCCESS)
            {
                sceneManager.Read<Recolte_UI>("ui").feedbacks.ShowTimeUp(null);
            }

            wasState = newState;
            return newState;
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
            if (player != null)
            {
                gazTank = player.GetComponent<SubmarinParts>().GazTank;
            }
        }
    }
}
