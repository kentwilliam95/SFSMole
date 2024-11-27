using System.Collections;
using UnityEngine;
using WhackAMole.Interfaces;

namespace WhackAMole.States
{
    public class StateGameIsStarting:IState<GameController>
    {
        private int _countdown;
        public StateGameIsStarting(int delay)
        {
            _countdown = delay;
        }

        public void Enter(GameController t)
        {
            t.StartCoroutine(DelayPerTick(t));
        }

        public void Update(GameController t)
        {
           
        }

        public void Exit(GameController t)
        {
            
        }

        private IEnumerator DelayPerTick(GameController t)
        {
            for (int i = 0; i < 3; i++)
            {
                t.TextUI.ShowText(_countdown);
                _countdown -= 1;
                yield return new WaitForSeconds(1);
                AudioController.Instance.PlaySFX(SFXData.ID.Countdown);
            }
            t.TextUI.ShowText("GO!");
            t.ChangeState(new StateGameStart());
        }
    }
}