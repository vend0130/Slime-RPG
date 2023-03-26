using System;
using Code.Game;
using Code.Infrastructure.Factories.UI;
using Code.UI;

namespace Code.Infrastructure.StateMachine.States
{
    public class EndGameState : IPayloadState<EndGameType>, IDisposable
    {
        private readonly IUIFactory _uiFactory;
        private const string LossGameText = "YOU LOSS";
        private const string WindGameText = "YOU WIN!";
        private const string MainSceneName = "Main";

        private IGameStateMachine _stateMachine;
        private EndGame _endGame;

        public EndGameState(IUIFactory uiFactory) =>
            _uiFactory = uiFactory;

        public void InitGameStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void Enter(EndGameType type)
        {
            _endGame = _uiFactory.CreateEndGame();
            _endGame.ChangeText(type == EndGameType.Loss ? LossGameText : WindGameText);

            _endGame.AgainButton.onClick.AddListener(Again);
        }

        public void Dispose()
        {
            Exit();
        }

        public void Exit() =>
            _endGame.AgainButton.onClick.RemoveListener(Again);

        private void Again() =>
            _stateMachine.Enter<LoadLevelState, string>(MainSceneName);
    }
}