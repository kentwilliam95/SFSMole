using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhackAMole.Interfaces
{
    public interface IState<T>
    {
        void Enter(T t);
        void Update(T t);
        void Exit(T t);
    }
}
