using Code.Game;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class EndGameState : IPayloadState<EndGameType>
    {
        private const string LossGameText = "YOU LOSS";
        private const string WindGameText = "YOU WIN!";

        private IGameStateMachine _stateMachine;

        public void InitGameStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter(EndGameType type)
        {
            Debug.Log(type == EndGameType.Loss ? LossGameText : WindGameText);
        }

        public void Exit()
        {
        }
    }
}