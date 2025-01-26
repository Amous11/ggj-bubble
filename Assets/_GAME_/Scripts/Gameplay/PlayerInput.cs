using Fusion;

namespace GGJ
{
    enum PlayerButtons
    {
        Fire,
        Jump
    }
    
    public struct PlayerInput : INetworkInput
    {
        public float HorizontalInput;
        public float VerticalInput;
        public NetworkButtons Buttons;
    }
}
