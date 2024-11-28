using System;
using System.Text;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using WhackAMole.Interfaces;

namespace WhackAMole.States
{
    public class StateGameEnd : IState<GameController>
    {
        private ISFSArray _result;

        public StateGameEnd(ISFSArray array)
        {
            _result = array;
        }

        public void Enter(GameController t)
        {
            for (int i = 0; i < t.Moles.Count; i++)
            {
                t.Moles[i].Hit(false);
            }

            StringBuilder sb = new StringBuilder();

            foreach (var res in _result)
            {
                var data = (SFSObject)res;
                sb.Append(data.GetInt("Id"));
                sb.Append("\t\t");
                sb.Append(data.GetText("Username"));
                sb.Append("\t");
                sb.Append(data.GetInt("HitCount").ToString());
                sb.Append("\t");

                bool isWin = data.GetBool("IsWinner");
                string winnerText = isWin ? "Win" : "Lose";
                sb.Append(winnerText);
            }

            t.UiEndResult.Show(sb.ToString());
        }

        public void Update(GameController t) { }

        public void Exit(GameController t) { }
    }
}