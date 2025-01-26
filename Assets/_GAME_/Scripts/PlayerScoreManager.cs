using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;

namespace GGJ
{
    public class PlayerScoreManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        #region Serialized Fields
        [SerializeField] private TextMeshProUGUI _playerOverviewEntryPrefab = null;
        [SerializeField] private Transform _scoreboardPanel = null;
        #endregion

        #region Private Fields
        private Dictionary<PlayerRef, TextMeshProUGUI> _playerListEntries = new Dictionary<PlayerRef, TextMeshProUGUI>();
        private Dictionary<PlayerRef, int> _playerScores = new Dictionary<PlayerRef, int>();
        #endregion

        #region INetworkRunnerCallbacks Implementation
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            // Create an entry when a new player joins
            AddEntry(player);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            // Remove the entry when a player leaves
            RemoveEntry(player);
        }

        public void OnSceneLoadDone(NetworkRunner runner) { }
        public void OnSceneLoadStart(NetworkRunner runner) { }
        public void OnGameStarted(NetworkRunner runner, GameMode gameMode) { }
        public void OnGameStopped(NetworkRunner runner) { }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            throw new System.NotImplementedException();
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
            throw new System.NotImplementedException();
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            throw new System.NotImplementedException();
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            throw new System.NotImplementedException();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
            throw new System.NotImplementedException();
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            throw new System.NotImplementedException();
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            throw new System.NotImplementedException();
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, System.ArraySegment<byte> data)
        {
            throw new System.NotImplementedException();
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
            throw new System.NotImplementedException();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            throw new System.NotImplementedException();
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
            throw new System.NotImplementedException();
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            throw new System.NotImplementedException();
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            throw new System.NotImplementedException();
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            throw new System.NotImplementedException();
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Public Methods

        // Handle adding a player to the scoreboard
        public void AddEntry(PlayerRef playerRef)
        {
            if (_playerListEntries.ContainsKey(playerRef)) return;

            // Create the UI entry for the player
            var entry = Instantiate(_playerOverviewEntryPrefab, _scoreboardPanel);
            entry.transform.localScale = Vector3.one; // Reset scale

            // Set default values for the player (you can replace these with real values later)
            int score = 0;

            _playerScores.Add(playerRef, score);
            _playerListEntries.Add(playerRef, entry);

            // Update the UI with the player's name and score
            UpdateEntry(playerRef, entry);
        }

        // Handle removing a player from the scoreboard
        public void RemoveEntry(PlayerRef playerRef)
        {
            if (_playerListEntries.TryGetValue(playerRef, out var entry))
            {
                Destroy(entry.gameObject);
                _playerListEntries.Remove(playerRef);
                _playerScores.Remove(playerRef);
            }
        }

        // Update a player's score in the UI
        public void UpdateScore(PlayerRef playerRef, int score)
        {
            if (_playerScores.ContainsKey(playerRef))
            {
                _playerScores[playerRef] = score;
                UpdateEntry(playerRef, _playerListEntries[playerRef]);
            }
        }

        #endregion

        #region Private Methods
        // Update the player's UI entry (nickname and score)
        private void UpdateEntry(PlayerRef playerRef, TextMeshProUGUI entry)
        {
            var score = _playerScores[playerRef];
            entry.text = $"Player {playerRef}: Score: {score}";
        }
        #endregion
    }
}
