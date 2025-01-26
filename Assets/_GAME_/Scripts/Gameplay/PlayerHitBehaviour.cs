using Fusion;
using UnityEngine;

namespace GGJ
{
    public class PlayerHitBehaviour : NetworkBehaviour
    {
        // The _points variable can be a local private variable as it will only be used to add points to the score
        // The score itself is networked and any increase or decrease will be propagated automatically.
        [SerializeField] private int _points = 1;

        // Used to delay the despawn after the hit and play the destruction animation.
        [Networked] private NetworkBool _wasHit { get; set; }

        [Networked] private TickTimer _despawnTimer { get; set; }


        public bool IsAlive => !_wasHit;

        // When the asteroid gets hit by another object, this method is called to decide what to do next.
        public void HitPlayer(PlayerRef player)
        {
            // The asteroid hit only triggers behaviour on the host and if the asteroid had not yet been hit.
            if (Object == null) return;
            if (Object.HasStateAuthority == false) return;
            if (_wasHit) return;

            // If this hit was triggered by a projectile, the player who shot it gets points
            // The player object is retrieved via the Runner.
            if (Runner.TryGetPlayerObject(player, out var playerNetworkObject))
            {
                playerNetworkObject.GetComponent<PlayerDataNetworked>().AddToScore(_points);
            }

            _wasHit = true;
            _despawnTimer = TickTimer.CreateFromSeconds(Runner, .2f);
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority && _wasHit && _despawnTimer.Expired(Runner))
            {
                _wasHit = false;
                _despawnTimer = TickTimer.None;

                // Big asteroids tell the AsteroidSpawner to spawn multiple small asteroids as it breaks up.

                Runner.Despawn(Object);
            }
        }
    }
}
