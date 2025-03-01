using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace GGJ
{
    // Holds the player's information and ensures it is replicated to all clients.
    public class PlayerDataNetworked : NetworkBehaviour
    {
        // Global static setting
        private const int STARTING_LIVES = 3;

        // Local Runtime references
        // private Scoreboard _scoreboard = null;

        private ChangeDetector _changeDetector;

        // Game Session SPECIFIC Settings are used in the UI.
        // The method passed to the OnChanged attribute is called everytime the [Networked] parameter is changed.
        [HideInInspector]
        [Networked]
        public NetworkString<_2> NickName { get; private set; }

        [HideInInspector]
        [Networked]
        public int Lives { get; private set; }

        [HideInInspector]
        [Networked]
        public int Score { get; private set; }

        public override void Spawned()
        {
            // --- Client
            // Find the local non-networked PlayerData to read the data and communicate it to the Host via a single RPC 
            if (Object.HasInputAuthority)
            {
                var nickName = FindObjectOfType<PlayerData>().GetNickName();
                RpcSetNickName(nickName);
            }

            // --- Host
            // Initialized game specific settings
            if (Object.HasStateAuthority)
            {
                Lives = STARTING_LIVES;
                Score = 0;
            }

            // --- Host & Client
            // Set the local runtime references.
            // _scoreboard = FindObjectOfType<Scoreboard>();
            
            // Add an entry to the local Overview panel with the information of this spaceship
            // _scoreboard.AddEntry(Object.InputAuthority, this);
            
            // Refresh panel visuals in Spawned to set to initial values.
            // _scoreboard.UpdateNickName(Object.InputAuthority, NickName.ToString());
            // _scoreboard.UpdateLives(Object.InputAuthority, Lives);
            // _scoreboard.UpdateScore(Object.InputAuthority, Score);
            
            _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
        }
        
        public override void Render()
        {
            foreach (var change in _changeDetector.DetectChanges(this, out var previousBuffer, out var currentBuffer))
            {
                switch (change)
                {
                    case nameof(NickName):
                        // _scoreboard.UpdateNickName(Object.InputAuthority, NickName.ToString());
                        break;
                    case nameof(Score):
                        // _scoreboard.UpdateScore(Object.InputAuthority, Score);
                        break;
                    case nameof(Lives):
                        // _scoreboard.UpdateLives(Object.InputAuthority, Lives);
                        break;
                }
            }
        }

        // Remove the entry in the local Overview panel for this spaceship
        public override void Despawned(NetworkRunner runner, bool hasState)
        {
            // _scoreboard.RemoveEntry(Object.InputAuthority);
        }

        // Increase the score by X amount of points
        public void AddToScore(int points)
        {
            Score += points;
        }

        // Decrease the current Lives by 1
        public void SubtractLife()
        {
            Lives--;
        }

        // RPC used to send player information to the Host
        [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)]
        private void RpcSetNickName(string nickName)
        {
            if (string.IsNullOrEmpty(nickName)) return;
            NickName = nickName;
        }
    }
}
