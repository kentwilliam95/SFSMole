using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
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
            //ISFSObject parameters = SFSObject.NewInstance();
            //parameters.PutInt("v1", 25);
            //parameters.PutInt("v2", 17);

            //_sfs.Send(new Sfs2X.Requests.ExtensionRequest("Sum", parameters));

            //parameters.PutText("cmd", "GAME_START");
            //Debug.Log(_sfs.LastJoinedRoom);
            //_sfs.Send(new Sfs2X.Requests.ExtensionRequest("GameStartHandler", parameters,_sfs.LastJoinedRoom));
        }

        private void Server_OnResponse(BaseEvent evt)
        {
            ISFSObject responseParams = (SFSObject)evt.Params["params"];
            Debug.Log(responseParams.GetUtfString("users"));
        }
    }
}
