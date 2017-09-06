
namespace CCC.Manager
{
    public class SpriteBankManager : BaseManager<SpriteBankManager>
    {
        public SpriteBank bank;
        public override void Init()
        {
            bank.SetAsInstance();
            CompleteInit();
        }
    }
}
