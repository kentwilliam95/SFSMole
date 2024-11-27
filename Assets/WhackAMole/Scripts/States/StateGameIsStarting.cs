using WhackAMole.Interfaces;

namespace WhackAMole.States
{
    public class StateGameIsStarting:IState<GameController>
    {
        private float _countdown;
        public StateGameIsStarting(float delay)
        {
            _countdown = delay;
        }

        public void Enter(GameController t)
        {
            
        }

        public void Update(GameController t)
        {
            
        }

        public void Exit(GameController t)
        {
            
        }
    }
}