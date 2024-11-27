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
using WhackAMole.States;

namespace WhackAMole
{
    public class GameController : MonoBehaviour
    {
        public SmartFox Client => SFSController.Instance.Client;
        private StateMachine<GameController> _fsmGame;
        [SerializeField] private PoolSpawnID _hitParticle; 
            
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

            GameSetting = new Utility.GameSetting();
            
            _fsmGame = new StateMachine<GameController>(this);
            _fsmGame.ChangeState(new StateGameWaiting());
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
                _fsmGame.ChangeState(new StateGameEnd());   
            }
        }
        
        private void Handle_GameReady(SFSObject val)
        {
            int[] arr = val.GetIntArray("SpawnSequence");
            for (int i = 0; i < arr.Length; i++)
            {
                GameSetting.AddSpawnSequence(arr[i]);
            }
            Debug.Log("Send ");
            Client.Send(new ExtensionRequest(Utility.CMD_GAMESETTINGRECEIVED, new SFSObject(), Client.LastJoinedRoom));
            _delay = Time.time;
        }

        private void Handle_StartGame(SFSObject val)
        {
            float delayTime = Time.time - _delay;
            Debug.Log($"Delay Time : {delayTime}, isDelay: {delayTime > 1}");
            _fsmGame.ChangeState(new StateGameIsStarting(5));
        }
    }
}