using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Config;
using UnityEngine;

namespace WhackAMole
{
    public enum RoomResponse
    {

    }

    public class MainMenuController : MonoBehaviour
    {
        private void Start()
        {
            var client = SFSController.Instance.Client;
            client.AddEventListener(SFSEvent.EXTENSION_RESPONSE, Server_OnResponse);
            client.AddEventListener(SFSEvent.LOGIN, Client_OnLogin);
            client.AddEventListener(SFSEvent.ROOM_JOIN, Client_JoinRoomSuccess);
            PanelFade.Instance.Hide();
        }

        private void Client_OnLogin(BaseEvent evt)
        {
            Debug.Log($"[SFS] Login Success");
            var client = SFSController.Instance.Client;

            SFSObject req = new SFSObject();
            req.PutUtfString("room", "WhackRoom");

            client.Send(new ExtensionRequest("CustomJoinRoom", req));
        }

        private void Client_JoinRoomSuccess(BaseEvent evt)
        {
            Debug.Log("Join Room Success!");
            var client = SFSController.Instance.Client;
            client.Send(new ExtensionRequest("JoinedRoom", new SFSObject(), client.LastJoinedRoom));
            
            // Handle_UserJoinedRoom((SFSObject)evt.Params["params"]);
        }

        private void Server_OnResponse(BaseEvent evt)
        {
            var cmd = evt.Params["cmd"];
            Debug.Log(cmd);
            switch (cmd)
            {
                case Utility.CMD_GAMESTARTING:
                    Handle_GameStarting((SFSObject)evt.Params["params"]);
                    break;

                case Utility.CMD_JOINEDROOM:
                    Handle_UserJoinedRoom((SFSObject)evt.Params["params"]);
                    break;

                case Utility.CMD_HIT:
                    break;

                case Utility.CMD_GAMEEND:
                    break;

                case Utility.CMD_USERLEAVE:
                    
                    Handle_UserLeave((SFSObject)evt.Params["params"]);
                    break;
            }
        }

        private void Handle_UserLeave(SFSObject obj)
        {
            Debug.Log("Handle User Leave!");
        }

        private void Handle_GameStarting(SFSObject obj)
        {
            Debug.Log("Handle Game Starting");
            //load the game scene here.
            SceneController.Instance.LoadScene(SceneController.SceneType.Game, null);
        }

        private void Handle_UserJoinedRoom(SFSObject obj)
        {
            Debug.Log("Joined a Room!");
            var arr = obj.GetSFSArray("users");
            foreach (var VARIABLE in arr)
            {
                var sfsObj = (SFSObject)VARIABLE;
                Debug.Log($"Username: {sfsObj.GetText("username")}, ID: {sfsObj.GetInt("id")}");
            }
            // SFSObject parameters = new SFSObject();
            // parameters.PutText("cmd", "GAME_START");
            // client.Send(new Sfs2X.Requests.ExtensionRequest("GameStartHandler", parameters, client.LastJoinedRoom));
        }
    }
}
