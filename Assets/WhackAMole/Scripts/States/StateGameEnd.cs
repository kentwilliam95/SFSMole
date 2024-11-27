using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using WhackAMole.Interfaces;

namespace WhackAMole.States
{
    public class StateGameEnd:IState<GameController>
    {
        public void Enter(GameController t)
        {
            for (int i = 0; i < t.Moles.Count; i++)
            {
                t.Moles[i].Hit(false);
            }
            
            //show end result here
            
            //Send data to server
            SFSObject req = new SFSObject();
            req.PutInt("HitCount", t.GameSetting.HitCount);
            
            t.Client.Send(new ExtensionRequest(Utility.CMD_GAMEEND, req, t.Client.LastJoinedRoom));
            
            Debug.Log("Mole Hit!");
        }

        public void Update(GameController t)
        {
            
        }

        public void Exit(GameController t)
        {
            
        }
    }
}