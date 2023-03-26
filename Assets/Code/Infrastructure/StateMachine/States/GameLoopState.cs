using System;
using System.Collections.Generic;
using Code.Data.Level;
using Code.Game.Enemies;
using Code.Infrastructure.Factories.Enemy;
using UnityEngine;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IDefaultState, IDisposable
    {
        private readonly IEnemiesPoolable _enemiesPool;

        private LevelConfig _levelConfig;
        private List<EnemyComponent> _enemiesInWave;
        private int _currentWave = 0;

        public GameLoopState(IEnemiesPoolable enemiesPool) =>
            _enemiesPool = enemiesPool;

        public void InitLevelConfig(LevelConfig levelConfig) =>
            _levelConfig = levelConfig;

        public void Enter()
        {
            _currentWave = 0;
            StartWave();
        }

        public void Exit()
        {
            UnSubscribe();
        }

        public void Dispose() =>
            UnSubscribe();

        private void AllEnemiesInCurrentWaveDie()
        {
            if (_currentWave >= _levelConfig.Waves.Length)
            {
                Debug.Log("YOU WIN!");
                return;
            }

            StartWave();
        }

        private void StartWave()
        {
            _enemiesPool.CreateWave(_currentWave);

            foreach (EnemyComponent enemy in _enemiesPool.CurrentWaveEnemies)
            {
                enemy.StartMove();
                enemy.DieHandler += EnemyDie;
            }

            _currentWave++;
        }

        private void EnemyDie(EnemyComponent enemy)
        {
            _enemiesPool.RemoveEnemyInWave(enemy);

            if (_enemiesPool.CurrentWaveEnemies.Count > 0)
                return;

            UnSubscribe();
            AllEnemiesInCurrentWaveDie();
        }

        private void UnSubscribe()
        {
            foreach (EnemyComponent enemy in _enemiesPool.CurrentWaveEnemies)
                enemy.DieHandler -= EnemyDie;
        }
    }
}