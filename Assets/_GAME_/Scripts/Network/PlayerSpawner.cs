using Fusion;
using UnityEngine;

namespace GGJ
{
    // The SpaceshipSpawner, just like the AsteroidSpawner, only executes on the Host.
    // Therefore none of its parameters need to be [Networked].
    public class PlayerSpawner : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        // References to the NetworkObject prefab to be used for the players' spaceships.
        [SerializeField] private NetworkPrefabRef _spaceshipNetworkPrefab = NetworkPrefabRef.Empty;

        private bool _gameIsReady = false;
        private GameStateController _gameStateController = null;

        private SpawnPoint[] _spawnPoints = null;

        public override void Spawned()
        {
            Debug.Log("player spawner started");
            if (Object.HasStateAuthority == false) return;
            // Collect all spawn points in the scene.
            _spawnPoints = FindObjectsOfType<SpawnPoint>();
        }

        // The spawner is started when the GameStateController switches to GameState.Running.
        public void StartPlayerSpawner(GameStateController gameStateController)
        {
            Debug.Log("StartPlayerSpawner");
            _gameIsReady = true;
            _gameStateController = gameStateController;
            foreach (var player in Runner.ActivePlayers)
            {
                Debug.Log("spawning pid " + player.PlayerId);
                SpawnPlayer(player);
            }
        }

        // Spawns a new spaceship if a client joined after the game already started
        public void PlayerJoined(PlayerRef player)
        {
            if (_gameIsReady == false) return;
            SpawnPlayer(player);
        }

        // Spawns a spaceship for a player.
        // The spawn point is chosen in the _spawnPoints array using the implicit playerRef to int conversion 
        private void SpawnPlayer(PlayerRef player)
        {
            // Modulo is used in case there are more players than spawn points.
            int index = player.PlayerId % _spawnPoints.Length;
            var spawnPosition = _spawnPoints[index].transform.position;
            
            Debug.Log("Spawning player " + player.PlayerId + " at position " + spawnPosition);
            
            var playerObject = Runner.Spawn(_spaceshipNetworkPrefab, spawnPosition, Quaternion.identity, player);
            // Set Player Object to facilitate access across systems.
            Runner.SetPlayerObject(player, playerObject);

            // Add the new spaceship to the players to be tracked for the game end check.
            _gameStateController.TrackNewPlayer(playerObject.GetComponent<PlayerDataNetworked>().Id);
        }

        // Despawns the spaceship associated with a player when their client leaves the game session.
        public void PlayerLeft(PlayerRef player)
        {
            DespawnPlayer(player);
        }

        private void DespawnPlayer(PlayerRef player)
        {
            if (Runner.TryGetPlayerObject(player, out var spaceshipNetworkObject))
            {
                Runner.Despawn(spaceshipNetworkObject);
            }

            // Reset Player Object
            Runner.SetPlayerObject(player, null);
        }
    }
}