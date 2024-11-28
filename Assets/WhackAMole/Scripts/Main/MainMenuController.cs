using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands.Config;
using Sfs2X;
using Sfs2X.Entities;
using UnityEngine;

namespace WhackAMole
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private UIRoom _uiRoom;
        [SerializeField] private UILogin _uiLogin;

        private SmartFox Client => SFSController.Instance.Client;

        private int userCount;

        private void Start()
        {
            var client = SFSController.Instance.Client;
            client.AddEventListener(SFSEvent.EXTENSION_RESPONSE, Server_OnResponse);
            client.AddEventListener(SFSEvent.LOGIN, Client_OnLogin);
            client.AddEventListener(SFSEvent.ROOM_JOIN, Client_JoinRoomSuccess);

            PanelFade.Instance.Hide();

            _uiLogin.Show();
            _uiRoom.Hide();

            _uiRoom.OnSemiOfflineGame_Clicked = StartSemiOFflineGame;
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
            client.Send(new ExtensionRequest(Utility.CMD_JOINEDROOM, new SFSObject(), client.LastJoinedRoom));
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
                case Utility.CMD_USERLEAVE:
                    Handle_UserLeave((SFSObject)evt.Params["params"]);
                    break;
            }
        }

        private void Handle_UserLeave(SFSObject obj)
        {
            Debug.Log("Handle User Leave!");

            var arr = obj.GetSFSArray("users");
            GenerateUserListFromServer(arr, out UIRoom.UserListData[] users);
            AudioController.Instance.PlaySFX(SFXData.ID.NegativeDing);
            _uiRoom.SetupUsers(users);
        }

        private void GenerateUserListFromServer(ISFSArray array, out UIRoom.UserListData[] users)
        {
            users = new UIRoom.UserListData[array.Count];
            int i = 0;
            foreach (var VARIABLE in array)
            {
                var sfsObj = (SFSObject)VARIABLE;
                int id = sfsObj.GetInt("id");
                string uname = sfsObj.GetText("username");

                users[i].Id = id;
                users[i].Username = uname;

                i++;
            }
        }

        private void Handle_GameStarting(SFSObject obj)
        {
            Debug.Log("Handle Game Starting");
            Client.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, Server_OnResponse);
            Client.RemoveEventListener(SFSEvent.LOGIN, Client_OnLogin);
            Client.RemoveEventListener(SFSEvent.ROOM_JOIN, Client_JoinRoomSuccess);

            //load the game scene here.
            SceneController.Instance.LoadScene(SceneController.SceneType.Game, null);
        }

        private void Handle_UserJoinedRoom(SFSObject obj)
        {
            Debug.Log("Joined a Room!");
            var arr = obj.GetSFSArray("users");
            GenerateUserListFromServer(arr, out UIRoom.UserListData[] users);

            _uiLogin.Hide();
            _uiRoom.Show();

            _uiRoom.SetupUsers(in users);

            AudioController.Instance.PlaySFX(SFXData.ID.PositiveDing);
        }

        public void StartSemiOFflineGame()
        {
            SFSObject parameters = new SFSObject();
            var client = SFSController.Instance.Client;

            parameters.PutText("cmd", "GAME_START");
            client.Send(
                new Sfs2X.Requests.ExtensionRequest(Utility.CMD_GAMESTARTING, parameters, client.LastJoinedRoom));
        }
    }
}