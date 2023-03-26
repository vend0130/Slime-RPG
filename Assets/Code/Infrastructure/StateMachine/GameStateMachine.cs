using System;
using System.Collections.Generic;
using Code.Infrastructure.StateMachine.States;

namespace Code.Infrastructure.StateMachine
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;

        private IState _activeState;

        public GameStateMachine(LoadLevelState loadLevelState, GameLoopState gameLoopState, EndGameState endGameState)
        {
            _states = new Dictionary<Type, IState>()
            {
                [typeof(LoadLevelState)] = loadLevelState,
                [typeof(GameLoopState)] = gameLoopState,
                [typeof(EndGameState)] = endGameState,
            };

            loadLevelState.InitGameStateMachine(this);
            gameLoopState.InitGameStateMachine(this);
            endGameState.InitGameStateMachine(this);
        }

        public void Enter<TState>() where TState : class, IDefaultState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>
        {
            var state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IState =>
            _states[typeof(TState)] as TState;
    }
}