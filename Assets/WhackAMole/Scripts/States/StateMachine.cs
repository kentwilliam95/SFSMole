using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhackAMole.Interfaces;

namespace WhackAMole
{
    public class StateMachine<T>
    {
        private IState<T> _current;
        private T _owner;

        public StateMachine(T owner)
        {
            _owner = owner;
        }

        public void ChangeState(IState<T> nextState)
        {
            if (_current != null)
            {
                _current.Exit(_owner);
            }

            _current = nextState;
            _current.Enter(_owner);
        }

        public void Update()
        {
            if (_current == null)
            {
                return;
            }
            _current.Update(_owner);
        }
    }
}
