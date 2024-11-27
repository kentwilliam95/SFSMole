using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using WhackAMole.Interfaces;

namespace WhackAMole.States
{
    public class StateGameWaiting:IState<GameController>
    {
        public StateGameWaiting()
        {
            
        }

        public void Enter(GameController t)
        {
            PanelLoading.Instance.Show("Waiting for Server!");
            t.Client.Send(new ExtensionRequest(Utility.CMD_GAMEREADY, new SFSObject(), t.Client.LastJoinedRoom));
        }

        public void Update(GameController t)
        {
            
        }

        public void Exit(GameController t)
        {
            PanelLoading.Instance.Hide();
        }
    }
}