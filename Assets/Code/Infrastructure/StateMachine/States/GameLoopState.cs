using Code.Infrastructure.Factories.Enemy;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IDefaultState
    {
        private readonly IEnemiesPoolable _enemiesPool;

        public GameLoopState(IEnemiesPoolable enemiesPool)
        {
            _enemiesPool = enemiesPool;
        }
        
        public void Enter()
        {
            _enemiesPool.StartWave();
        }

        public void Exit()
        {
        }
    }
}