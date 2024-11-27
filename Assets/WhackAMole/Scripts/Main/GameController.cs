using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using WhackAMole.Game;
using WhackAMole.Interfaces;
using WhackAMole.States;

namespace WhackAMole
{
    public class GameController : MonoBehaviour
    {
        public SmartFox Client => SFSController.Instance.Client;
        private StateMachine<GameController> _fsmGame;
        [SerializeField] private PoolSpawnID _hitParticle;
        
        [field: SerializeField] public GameAnimatedText TextUI;
        [SerializeField] private UIEndResult _uiEndResult;
        
        public Utility.GameSetting GameSetting { get; private set; }
        private float _delay;
        
        [field: SerializeField] public List<GameMole> Moles { get; private set; }
        private void OnDestroy()
        {
            Client.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, Client_OnServerResponse);
        }

        private void Start()
        {
            PanelFade.Instance.Hide();
            Client.AddEventListener(SFSEvent.EXTENSION_RESPONSE, Client_OnServerResponse);
            Client.Send(new ExtensionRequest(Utility.CMD_GAMEREADY, new SFSObject(), Client.LastJoinedRoom));

            _uiEndResult.OnButtonClick = UIEndResult_OnButtonClick;
            _uiEndResult.Hide();
            GameSetting = new Utility.GameSetting();
            
            _fsmGame = new StateMachine<GameController>(this);
            // _fsmGame.ChangeState(new StateGameWaiting());
            Handle_StartGame(null);
        }

        private void Update()
        {
            UpdateStateMachine();
        }

        private void UpdateStateMachine()
        {
            if (_fsmGame == null)
            {
                return;
            }

            _fsmGame.Update();
        }

        private void Client_OnServerResponse(BaseEvent evt)
        {
            var cmd = evt.Params["cmd"];
            Debug.Log(cmd);
            switch (cmd)
            {
                case Utility.CMD_GAMEREADY:
                    Handle_GameReady((SFSObject)evt.Params["params"]);
                    break;
                case Utility.CMD_STARTGAME:
                    Handle_StartGame((SFSObject)evt.Params["params"]);
                    break;
                case Utility.CMD_GAMEEND:
                    Handle_GameEnd((SFSObject)evt.Params["params"]);
                    break;
            }
        }

        public void HitMole(GameMole mole)
        {
            mole.Hit();
            GameSetting.IncreaseHitCount();
            //send hit to server

            var go = PoolSpawn.Instance.Spawn(_hitParticle);
            go.transform.position = mole.TopPos;
            
            // change state to game end when hit count reach 10
            if (GameSetting.HitCount >= 10)
            {
                AudioController.Instance.PlaySFX(SFXData.ID.GameEnd);
                // _fsmGame.ChangeState(new StateGameEnd());
                Handle_GameEnd(null);
            }
        }
        
        public void ChangeState(IState<GameController> state)
        {
            _fsmGame.ChangeState(state);
        }
        
        private void Handle_GameReady(SFSObject val)
        {
            int[] arr = val.GetIntArray("SpawnSequence");
            for (int i = 0; i < arr.Length; i++)
            {
                GameSetting.AddSpawnSequence(arr[i]);
            }
            Debug.Log("Game Setting Received!");
            Client.Send(new ExtensionRequest(Utility.CMD_GAMESETTINGRECEIVED, new SFSObject(), Client.LastJoinedRoom));
        }

        private void Handle_StartGame(SFSObject val)
        {
            //need to handle lag here to make all client start at the same time
            _fsmGame.ChangeState(new StateGameIsStarting(3));
        }

        private void Handle_GameEnd(SFSObject val)
        {
            _uiEndResult.Show("You Win!");
        }

        private void UIEndResult_OnButtonClick()
        {
            SceneController.Instance.LoadScene(SceneController.SceneType.MainMenu, null);
            Client.RemoveEventListener(SFSEvent.EXTENSION_RESPONSE, Client_OnServerResponse);
            
            Client.Send(new ExtensionRequest(Utility.CMD_USERLEAVE, new SFSObject(), Client.LastJoinedRoom));
            Debug.Log($"Sending exit room request, Client Last Join Room: {Client.LastJoinedRoom}");
        }
    }
}