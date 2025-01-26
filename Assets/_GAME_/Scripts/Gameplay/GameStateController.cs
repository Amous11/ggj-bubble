using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;

namespace GGJ
{
    public class GameStateController : NetworkBehaviour
    {
        enum GameState
        {
            Running,
            Ending
        }

        [SerializeField] private float _gameSessionLength = 180.0f;
        [SerializeField] private float _endDelay = 4.0f;

        [Networked] private TickTimer _timer { get; set; }
        [Networked] private GameState _gameState { get; set; }
        [Networked] private NetworkBehaviourId _winner { get; set; }

        private List<NetworkBehaviourId> _playerDataNetworkedIds = new List<NetworkBehaviourId>();

        public override void Spawned()
        {
            // Initialize the game state as Running immediately
            _gameState = GameState.Running;
            _timer = TickTimer.CreateFromSeconds(Runner, _gameSessionLength);

            // Start player spawner
            FindAnyObjectByType<PlayerSpawner>().StartPlayerSpawner(this);

            // Set simulated to ensure FixedUpdateNetwork runs on all clients
            Runner.SetIsSimulated(Object, true);
        }

        public override void FixedUpdateNetwork()
        {
            if (_gameState == GameState.Running && _timer.ExpiredOrNotRunning(Runner))
            {
                GameHasEnded();
            }
            else if (_gameState == GameState.Ending && _timer.ExpiredOrNotRunning(Runner))
            {
                Runner.Shutdown();
            }
        }

        public void CheckIfGameHasEnded()
        {
            if (Object.HasStateAuthority == false) return;

            int playersAlive = 0;

            for (int i = 0; i < _playerDataNetworkedIds.Count; i++)
            {
                if (Runner.TryFindBehaviour(_playerDataNetworkedIds[i], out PlayerDataNetworked playerDataNetworkedComponent) == false)
                {
                    _playerDataNetworkedIds.RemoveAt(i);
                    i--;
                    continue;
                }

                if (playerDataNetworkedComponent.Lives > 0) playersAlive++;
            }

            foreach (var playerDataNetworkedId in _playerDataNetworkedIds)
            {
                if (Runner.TryFindBehaviour(playerDataNetworkedId, out PlayerDataNetworked playerDataNetworkedComponent) == false)
                    continue;

                if (playerDataNetworkedComponent.Lives > 0 == false) continue;

                _winner = playerDataNetworkedId;
            }

            if (_winner == default)
            {
                _winner = _playerDataNetworkedIds[0];
            }

            GameHasEnded();
        }

        private void GameHasEnded()
        {
            _timer = TickTimer.CreateFromSeconds(Runner, _endDelay);
            _gameState = GameState.Ending;
        }

        public void TrackNewPlayer(NetworkBehaviourId playerDataNetworkedId)
        {
            _playerDataNetworkedIds.Add(playerDataNetworkedId);
        }
    }
}
