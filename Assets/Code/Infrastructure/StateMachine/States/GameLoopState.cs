using System;
using System.Collections.Generic;
using Code.Data.Level;
using Code.Game;
using Code.Game.Enemies;
using Code.Game.Hero;
using Code.Infrastructure.Factories.Enemy;

namespace Code.Infrastructure.StateMachine.States
{
    public class GameLoopState : IPayloadState<HeroComponent>, IDisposable
    {
        private readonly IEnemiesPoolable _enemiesPool;

        private HeroComponent _hero;
        private LevelConfig _levelConfig;
        private List<EnemyComponent> _enemiesInWave;
        private int _currentWave = 0;
        private CameraFollow _cameraFollow;
        private IGameStateMachine _stateMachine;

        public GameLoopState(IEnemiesPoolable enemiesPool) =>
            _enemiesPool = enemiesPool;

        public void InitGameStateMachine(IGameStateMachine stateMachine) =>
            _stateMachine = stateMachine;

        public void InitLevel(LevelConfig levelConfig, CameraFollow cameraFollow)
        {
            _levelConfig = levelConfig;
            _cameraFollow = cameraFollow;
        }

        public void Enter(HeroComponent hero)
        {
            _hero = hero;
            _cameraFollow.InitTarget(_hero.Current);
            _currentWave = 0;
            StartWave();

            _hero.DieHandler += HeroDie;
        }

        public void Dispose() =>
            Exit();

        public void Exit()
        {
            UnSubscribe();
            _hero.DieHandler -= HeroDie;
        }

        private void HeroDie() =>
            _stateMachine.Enter<EndGameState, EndGameType>(EndGameType.Loss);

        private void AllEnemiesInCurrentWaveDie()
        {
            if (_currentWave >= _levelConfig.Waves.Length)
            {
                _stateMachine.Enter<EndGameState, EndGameType>(EndGameType.Win);
                return;
            }

            _hero.StartMove();
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