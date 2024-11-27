using System.Collections.Generic;
using UnityEngine;
using WhackAMole.Game;
using WhackAMole.Interfaces;

namespace WhackAMole.States
{
    public class StateGameStart:IState<GameController>
    {
        private float _moleDuration = 2f;
        private float _countdown;
        private RaycastHit[] _hitResult;
        public void Enter(GameController t)
        {
            _countdown = _moleDuration;
            _hitResult = new RaycastHit[8];
            
            AudioController.Instance.PlaySFX(SFXData.ID.GameStart);
        }

        public void Update(GameController t)
        {
            HandleInput(t);
            _countdown -= Time.deltaTime;
            if (_countdown <= 0)
            {
                SpawnMoles(t);
                _countdown = _moleDuration;
            }
        }

        private void SpawnMoles(GameController t)
        {
            Shuffle(t.Moles);
            var randCount = t.GameSetting.GetSpawnSequence();
            int spawnCount = 0;
            for (int i = 0; i < t.Moles.Count; i++)
            {
                if (spawnCount >= randCount) { break;}
                if (t.Moles[i].IsShowing) { continue; }
            
                t.Moles[i].Show(2f);
                spawnCount++;
            }
        }

        private void HandleInput(GameController t)
        {
            if (!Input.GetMouseButtonDown(0)) { return; }

            var cam = MainCamera.Instance.Cam;
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, _hitResult);

            for (int i = 0; i < hitCount; i++)
            {
                GameMole mole = _hitResult[i].collider.GetComponentInParent<GameMole>();
                if (!mole) { continue; }
                if (!mole.IsShowing) { continue;}
                
                AudioController.Instance.PlaySFX(SFXData.ID.Button);
                t.HitMole(mole);
                break;
            }
        }

        private void Shuffle(List<GameMole> gameMoles)
        {
            for (int i = 0; i < gameMoles.Count; i++)
            {
                var randIndex = Random.Range(0, gameMoles.Count);
                var temp = gameMoles[randIndex];
                gameMoles[randIndex] = gameMoles[i];
                gameMoles[i] = temp;
            }
        }

        public void Exit(GameController t)
        {
        }
    }
}