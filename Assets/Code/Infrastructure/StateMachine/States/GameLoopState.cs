using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IDefaultState
    {
        public void Enter()
        {
            Debug.Log("Enter game loop");
        }

        public void Exit()
        {
        }
    }
}